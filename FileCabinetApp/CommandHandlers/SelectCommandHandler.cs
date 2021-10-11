using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ConsoleTables;
using FileCabinetApp.Entities;
using FileCabinetApp.Entities.XmlSerialization;
using FileCabinetApp.Readers;
using FileCabinetApp.Services;
using FileCabinetApp.Utility;
using FileCabinetApp.Validators.InputValidators;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Implementation selection command.
    /// </summary>
    public class SelectCommandHandler : CabinetServiceCommandHandlerBase
    {
        private const string Example = "\nExample - select id, firstname, lastname where firstname = 'John' and/or lastname = 'Doe'";
        private readonly InputValidator inputValidator;

        /// <summary>Initializes a new instance of the <see cref="SelectCommandHandler" /> class.</summary>
        /// <param name="fileCabinetService">The file cabinet service.</param>
        /// <param name="validator">Current validator.</param>
        public SelectCommandHandler(IFileCabinetService fileCabinetService, InputValidator validator)
            : base(fileCabinetService)
        {
            this.inputValidator = validator;
        }

        /// <summary>
        /// Handles the specified application command request.
        /// </summary>
        /// <param name="appCommandRequest">The application command request.</param>
        public override void Handle(AppCommandRequest appCommandRequest)
        {
            if (appCommandRequest == null)
            {
                throw new ArgumentNullException(nameof(appCommandRequest));
            }

            if (appCommandRequest.Command.Equals("select", StringComparison.OrdinalIgnoreCase))
            {
                var tuple = this.ParseData(appCommandRequest.Parameters);
                if (tuple != null)
                {
                    WriteRecords(tuple.Item1, tuple.Item2);
                }

                return;
            } // select id, firstname, lastname where firstname = 'John' and lastname = 'Doe'

            base.Handle(appCommandRequest);
        }

        private static void WriteRecords(ConsoleTable table, FileCabinetRecord[] records)
        {
            foreach (var record in records)
            {
                table.AddRow(record.ToParametersValue(table.Columns).ToArray());
            }

            table.Write(Format.Alternative);
        }

        private static ConsoleTable CreateTable(string tableParams)
        {
            var parametrs = GetTableParams(tableParams);
            if (parametrs is null)
            {
                return null;
            }

            var table = new ConsoleTable(parametrs);
            return table;
        }

        private static Tuple<List<string>, List<string>, string> GetSearchedParams(string parameters)
        {
            string separator = null;
            string keysAndValues = parameters.Replace("\'", string.Empty, StringComparison.OrdinalIgnoreCase)
                .Replace(" = ", " ", StringComparison.OrdinalIgnoreCase).Trim();
            string[] keysAndValuesArr = keysAndValues.Split(" ");
            if (keysAndValuesArr.Length > 2 && !TryGetSeparator(keysAndValues, ref keysAndValuesArr, ref separator))
            {
                return null;
            }

            var result = new Tuple<List<string>, List<string>, string>(new List<string>(), new List<string>(), separator);
            for (int i = 0; i < keysAndValuesArr.Length; i += 2)
            {
                result.Item1.Add(keysAndValuesArr[i]);
                result.Item2.Add(keysAndValuesArr[i + 1]);
            }

            return result;
        }

        private static bool TryGetSeparator(string keysAndValues, ref string[] keysAndValuesArr, ref string separator)
        {
            string[] separators = { " or ", " and " };
            foreach (var sep in separators)
            {
                if (keysAndValues.Contains(sep, StringComparison.OrdinalIgnoreCase))
                {
                    separator = sep.Trim();
                    keysAndValuesArr = keysAndValues.Replace(sep, " ", StringComparison.OrdinalIgnoreCase).Trim().Split(' ');
                }
            }

            if (separator is null)
            {
                Console.WriteLine($"Invalid input. It must contain 'or' or 'and' between multiple searched parameters.{Example}");
                return false;
            }

            return true;
        }

        private static string[] GetTableParams(string parameters)
        {
            string[] tableParams = parameters.Split(',');
            for (int i = 0; i < tableParams.Length; i++)
            {
                switch (tableParams[i].ToUpper(CultureInfo.InvariantCulture).Trim())
                {
                    case "FIRSTNAME":
                        tableParams[i] = "FirstName";
                        break;
                    case "LASTNAME":
                        tableParams[i] = "LastName";
                        break;
                    case "DATEOFBIRTH":
                        tableParams[i] = "DateOfBirth";
                        break;
                    case "ANNUALINCOME":
                        tableParams[i] = "AnnualIncome";
                        break;
                    case "WORKINGHOURS":
                        tableParams[i] = "WorkingHoursPerWeek";
                        break;
                    case "DRIVERCATEGORY":
                        tableParams[i] = "DriverLicenseCategory";
                        break;
                    case "ID":
                        tableParams[i] = "Id";
                        break;
                    default:
                        Console.WriteLine($"Invalid input - '{tableParams[i]}'. Parameters name can be:\n'FirstName', \n'LastName', \n'AnnualIncome', \n'DateOfBirth', \n'DriverLicenseCategory', \n'WorkingHoursPerWeek'{Example}");
                        return null;
                }
            }

            return tableParams;
        }

        private Tuple<ConsoleTable, FileCabinetRecord[]> ParseData(string parameters)
        {
            if (!parameters.Contains(" where ", StringComparison.CurrentCulture))
            {
                Console.WriteLine($"Invalid input. It must contain ' where ' between table parameters and searched parameters.{Example}");
                return null;
            }

            string[] parametersArray = parameters.Split(" where ", 2);
            const int searchedParamsIndex = 1;
            const int tableParamsIndex = 0;
            ConsoleTable table = CreateTable(parametersArray[tableParamsIndex]);
            FileCabinetRecord[] records = this.GetRecords(parametersArray[searchedParamsIndex]);
            if (records is null || table is null)
            {
                return null;
            }

            return new Tuple<ConsoleTable, FileCabinetRecord[]>(table, records);
        }

        private FileCabinetRecord[] GetRecords(string parameters)
        {
            IEnumerable<FileCabinetRecord> result = new List<FileCabinetRecord>();
            Tuple<List<string>, List<string>, string> tuple = GetSearchedParams(parameters);
            (string[] searchKeys, string[] searchValues, string separator) = (tuple.Item1.ToArray(), tuple.Item2.ToArray(), tuple.Item3);

            for (int i = 0; i < searchKeys.Length; i++)
            {
                IEnumerable<FileCabinetRecord> current;
                try
                {
                    current = this.FindBy(searchKeys[i], searchValues[i]);
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }

                if (i == 0)
                {
                    result = current;
                }
                else if (separator.Equals("and", StringComparison.OrdinalIgnoreCase))
                {
                    result = result.Where(x => current.Contains(x));
                }
                else if (separator.Equals("or", StringComparison.OrdinalIgnoreCase))
                {
                    result = result.Union(current);
                }
            }

            return result.ToArray();
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
                    var firstName = ParameterReaders.ReadInput(parameterValue, Converter.StringConverter, this.inputValidator.FirstNameValidator);
                    records = this.FileCabinetService.FindByFirstName(firstName);
                    break;
                case "LASTNAME":
                    var lastName = ParameterReaders.ReadInput(parameterValue, Converter.StringConverter, this.inputValidator.LastNameValidator);
                    records = this.FileCabinetService.FindByLastName(lastName);
                    break;
                case "DATEOFBIRTH":
                    var dateOfBirthday = ParameterReaders.ReadInput(parameterValue, Converter.DateConverter, this.inputValidator.DateOfBirthValidator);
                    records = this.FileCabinetService.FindByDateOfBirthday(dateOfBirthday);
                    break;
                case "WORKINGHOURS":
                    var workingHoursPerWeek = ParameterReaders.ReadInput(parameterValue, Converter.ShortConverter, this.inputValidator.WorkingHoursValidator);
                    records = this.FileCabinetService.FindByWorkingHours(workingHoursPerWeek);
                    break;
                case "ANNUALINCOME":
                    var annualIncome = ParameterReaders.ReadInput(parameterValue, Converter.DecimalConverter, this.inputValidator.AnnualIncomeValidator);
                    records = this.FileCabinetService.FindByAnnualIncome(annualIncome);
                    break;
                case "DRIVERCATEGORY":
                    var driverLicenseCategory = ParameterReaders.ReadInput(parameterValue, Converter.CharConverter, this.inputValidator.DriverLicenseCategoryValidator);
                    records = this.FileCabinetService.FindByDriverCategory(driverLicenseCategory);
                    break;
                default:
                    throw new ArgumentException($"Error in field name. Possible field naming options: id, firstname, lastname, drivercagetogy, workinghours, dateofbirth, annualincome.{Example}");
            }

            return records;
        }
    }
}
