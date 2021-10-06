using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCabinetApp.Entities;
using FileCabinetApp.Readers;
using FileCabinetApp.Services;
using FileCabinetApp.Utility;
using FileCabinetApp.Validators.InputValidators;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Implement Update command.
    /// </summary>
    /// <seealso cref="CabinetServiceCommandHandlerBase" />
    public class UpdateCommandHandler : CabinetServiceCommandHandlerBase
    {
        private static InputValidator inputValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">The file cabinet service.</param>
        /// <param name="validator">The validator.</param>
        public UpdateCommandHandler(IFileCabinetService fileCabinetService, InputValidator validator)
            : base(fileCabinetService)
        {
            inputValidator = validator;
        }

        private static string ExceptionMessage
        {
            get
            {
                var sb = new StringBuilder();
                sb.AppendLine("Unknown command. The input string must be in the following format:");
                sb.AppendLine("> update set field = 'value', field = 'value' ... where [condition]");
                sb.AppendLine();
                sb.AppendLine(" - each [field = 'value'] pair must be separated from each other with '=', a value must be placed into single quotes (').");
                sb.AppendLine();

                return sb.ToString();
            }
        }

        /// <summary>
        /// Handles the specified application command request.
        /// </summary>
        /// <param name="appCommandRequest">The application command request.</param>
        /// <exception cref="System.ArgumentNullException">appCommandRequest.</exception>
        public override void Handle(AppCommandRequest appCommandRequest)
        {
            if (appCommandRequest == null)
            {
                throw new ArgumentNullException(nameof(appCommandRequest));
            }

            if (appCommandRequest.Command.Equals("update", StringComparison.OrdinalIgnoreCase))
            {
                var sb = new StringBuilder();

                try
                {
                    var dataStrings = GetDataStrings(appCommandRequest.Parameters);
                    List<FileCabinetRecord> recordsToChange = this.GetRecordsToUpdate(dataStrings[1]).ToList();

                    if (!recordsToChange.Any())
                    {
                        Console.WriteLine($"A record with {appCommandRequest.Parameters.Split("where")[1].Trim()} wasn't found.");
                        return;
                    }

                    var fieldValueDictionary = GetFieldValueDictionary(dataStrings[0]);

                    if (recordsToChange.Count == 1)
                    {
                        int id = recordsToChange.First().Id;
                        this.Update(recordsToChange, fieldValueDictionary);
                        sb.AppendLine($"Record #{id} was updated.");
                    }
                    else
                    {
                        sb.Append("Records ");

                        sb.Append(this.Update(recordsToChange, fieldValueDictionary));

                        sb.Remove(sb.Length - 2, 1);
                        sb.Append("were updated.");
                    }
                }
                catch (Exception e) when (e is ArgumentException || e is FormatException || e is IOException)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Please, try again.");
                    return;
                }
                catch (KeyNotFoundException)
                {
                    Console.WriteLine($"A record with {appCommandRequest.Parameters.Split("where")[1].Trim()} wasn't found.");
                    return;
                }

                Console.WriteLine(sb.ToString());
                return;
            }

            base.Handle(appCommandRequest);
        }

        private static ParametersContainer GetInputData(FileCabinetRecord record, Dictionary<string, string> fieldValueDictionary)
        {
            var container = new ParametersContainer(record.FirstName, record.LastName, record.DateOfBirth, record.WorkingHoursPerWeek, record.AnnualIncome, record.DriverLicenseCategory);

            foreach (KeyValuePair<string, string> pair in fieldValueDictionary)
            {
                switch (pair.Key.ToUpperInvariant())
                {
                    case "FIRSTNAME":
                        container.FirstName = ParameterReaders.ReadInput(pair.Value, Converter.StringConverter, inputValidator.FirstNameValidator);
                        break;
                    case "LASTNAME":
                        container.LastName = ParameterReaders.ReadInput(pair.Value, Converter.StringConverter, inputValidator.LastNameValidator);
                        break;
                    case "ACCOUNTTYPE":
                        container.DriverLicenseCategory = ParameterReaders.ReadInput(pair.Value, Converter.CharConverter, inputValidator.DriverLicenseCategoryValidator);
                        break;
                    case "BONUSES":
                        container.WorkingHoursPerWeek = ParameterReaders.ReadInput(pair.Value, Converter.ShortConverter, inputValidator.WorkingHoursValidator);
                        break;
                    case "DATEOFBIRTH":
                        container.DateOfBirthday = ParameterReaders.ReadInput(pair.Value, Converter.DateConverter, inputValidator.DateOfBirthValidator);
                        break;
                    case "MONEY":
                        container.AnnualIncome = ParameterReaders.ReadInput(pair.Value, Converter.DecimalConverter, inputValidator.AnnualIncomeValidator);
                        break;
                    case "ID":
                        throw new ArgumentException("Can't update a record ID.");

                    default:
                        throw new ArgumentException("Error in field name. Possible field options: firstname, lastname, dateofbirth, accountType, bonuses, money.");
                }
            }

            return container;
        }

        private static Dictionary<string, string> GetFieldValueDictionary(string dataStrings)
        {
            string[] fieldsToUpdate = dataStrings.RemoveSpecialCharacters().Split("',", StringSplitOptions.RemoveEmptyEntries);

            if (fieldsToUpdate.Any(x => !x.Contains('=', StringComparison.InvariantCultureIgnoreCase) || !x.Contains('\'', StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new ArgumentException(ExceptionMessage);
            }

            Dictionary<string, string> fieldValueDictionary = new Dictionary<string, string>();

            foreach (string s in fieldsToUpdate)
            {
                string[] data = s.Replace("'", string.Empty, StringComparison.InvariantCultureIgnoreCase).Split('=', StringSplitOptions.RemoveEmptyEntries);

                if (data.Length != 2)
                {
                    throw new ArgumentException("Unknown field key-value pair. The input string must be in the following format: update set fieldName = 'value' ...");
                }

                fieldValueDictionary.Add(data[0], data[1]);
            }

            return fieldValueDictionary;
        }

        private static string[] GetDataStrings(string parameters)
        {
            if (!parameters.Contains("set", StringComparison.InvariantCultureIgnoreCase) || !parameters.Contains("where", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ArgumentException(ExceptionMessage);
            }

            string[] dataStrings = parameters.Replace("set", string.Empty, StringComparison.InvariantCultureIgnoreCase).Split("where", StringSplitOptions.RemoveEmptyEntries);

            if (dataStrings.Length != 2)
            {
                throw new ArgumentException(ExceptionMessage);
            }

            return dataStrings;
        }

        private string Update(List<FileCabinetRecord> recordsToChange, Dictionary<string, string> fieldValueDictionary)
        {
            var sb = new StringBuilder(recordsToChange.Count);
            foreach (FileCabinetRecord record in recordsToChange)
            {
                this.FileCabinetService.EditRecord(record.Id, GetInputData(record, fieldValueDictionary));
                sb.Append($"#{record.Id}, ");
            }

            return sb.ToString();
        }

        private IEnumerable<FileCabinetRecord> GetRecordsToUpdate(string conditionDataString)
        {
            int equalSignCount = conditionDataString.Count(x => x.Equals('='));

            if (equalSignCount == 0 || !conditionDataString.Contains('\'', StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ArgumentException(ExceptionMessage);
            }

            if (equalSignCount > 1 && !conditionDataString.Contains(" and ", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ArgumentException("The condition 'AND' is only supported.");
            }

            string[] recordsToFindCondition = conditionDataString.Split(" and ", StringSplitOptions.RemoveEmptyEntries);

            IEnumerable<FileCabinetRecord> recordsToChange = new List<FileCabinetRecord>();

            foreach (string s in recordsToFindCondition)
            {
                string[] strings = s.Replace("'", string.Empty, StringComparison.InvariantCultureIgnoreCase).Split('=', StringSplitOptions.RemoveEmptyEntries);
                string key = strings[0].Trim();
                string value = strings[1].Trim();

                recordsToChange = recordsToChange.Union(this.FindBy(key, value));
            }

            return recordsToChange;
        }

        private IEnumerable<FileCabinetRecord> FindBy(string key, string parameterValue)
        {
            IEnumerable<FileCabinetRecord> records;

            switch (key.ToUpperInvariant())
            {
                case "ID":
                    int id = ParameterReaders.ReadInput(parameterValue, Converter.IntConverter, InputValidator.IdValidator);
                    records = this.FileCabinetService.FindById(id);
                    break;
                case "FIRSTNAME":
                    var firstName = ParameterReaders.ReadInput(parameterValue, Converter.StringConverter, inputValidator.FirstNameValidator);
                    records = this.FileCabinetService.FindByFirstName(firstName);
                    break;
                case "LASTNAME":
                    var lastName = ParameterReaders.ReadInput(parameterValue, Converter.StringConverter, inputValidator.LastNameValidator);
                    records = this.FileCabinetService.FindByLastName(lastName);
                    break;
                case "DATEOFBIRTH":
                    var dateOfBirthday = ParameterReaders.ReadInput(parameterValue, Converter.DateConverter, inputValidator.DateOfBirthValidator);
                    records = this.FileCabinetService.FindByDateOfBirthday(dateOfBirthday);
                    break;
                case "WORKINGHOURS":
                    var workingHoursPerWeek = ParameterReaders.ReadInput(parameterValue, Converter.ShortConverter, inputValidator.WorkingHoursValidator);
                    records = this.FileCabinetService.FindByWorkingHours(workingHoursPerWeek);
                    break;
                case "ANNUALINCOME":
                    var annualIncome = ParameterReaders.ReadInput(parameterValue, Converter.DecimalConverter, inputValidator.AnnualIncomeValidator);
                    records = this.FileCabinetService.FindByAnnualIncome(annualIncome);
                    break;
                case "DRIVERCATEGORY":
                    var driverLicenseCategory = ParameterReaders.ReadInput(parameterValue, Converter.CharConverter, inputValidator.DriverLicenseCategoryValidator);
                    records = this.FileCabinetService.FindByDriverCategory(driverLicenseCategory);
                    break;
                default:
                    throw new ArgumentException("Error in field name. Possible field naming options: id, firstname, lastname, accountType, bonuses, dateofbirth, money.");
            }

            return records;
        }
    }
}
