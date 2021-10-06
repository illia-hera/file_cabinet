using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using FileCabinetApp.Entities;
using FileCabinetApp.Services;
using FileCabinetApp.Utility;
using FileCabinetApp.Validators.InputValidators;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Delete command implementation.
    /// </summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.CabinetServiceCommandHandlerBase" />
    public class DeleteCommandHandler : CabinetServiceCommandHandlerBase
    {
        private static InputValidator inputValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCommandHandler" /> class.
        /// </summary>
        /// <param name="fileCabinetService">The file cabinet service.</param>
        /// <param name="validator">The validator.</param>
        public DeleteCommandHandler(IFileCabinetService fileCabinetService, InputValidator validator)
            : base(fileCabinetService)
        {
            inputValidator = validator;
        }

        /// <summary>
        ///     Handles the specified application command request.
        /// </summary>
        /// <param name="appCommandRequest">The application command request.</param>
        public override void Handle(AppCommandRequest appCommandRequest)
        {
            if (appCommandRequest == null)
            {
                throw new ArgumentNullException(nameof(appCommandRequest));
            }

            if (appCommandRequest.Command.Equals("delete", StringComparison.OrdinalIgnoreCase))
            {
                StringBuilder sb = new StringBuilder();

                var tuple = ParseData(appCommandRequest.Parameters);
                var (keys, values) = tuple;

                IEnumerable<FileCabinetRecord> records = new List<FileCabinetRecord>();
                for (int i = 0; i < keys.Length; i++)
                {
                    this.GetRecords(keys[i], values[i], ref records);
                }

                foreach (var fileCabinetRecord in records)
                {
                    this.FileCabinetService.RemoveRecord(fileCabinetRecord.Id);
                    sb.Append($"#{fileCabinetRecord.Id}, ");
                }

                Console.WriteLine($"Records {sb.ToString()}are deleted");
                return;
            }

            base.Handle(appCommandRequest);
        }

        private static Tuple<string[], string[]> ParseData(string parameters)
        {
            string parameterString = parameters.Replace("where ", string.Empty, StringComparison.InvariantCultureIgnoreCase);
            string[] dataStrings = parameterString.Replace("'", string.Empty, StringComparison.InvariantCultureIgnoreCase).Split('=', StringSplitOptions.RemoveEmptyEntries);

            if (dataStrings.Length % 2 != 0)
            {
                throw new ArgumentException("the input string must be in the following format: delete where field1='value1'");
            }

            string[] keys = new string[dataStrings.Length / 2];
            string[] values = new string[dataStrings.Length / 2];

            for (int i = 0, j = 0; i < dataStrings.Length; i += 2, j++)
            {
                keys[j] = dataStrings[i];
                values[j] = dataStrings[i + 1];
            }

            return new Tuple<string[], string[]>(keys, values);
        }

        private void GetRecords(string key, string parameterValue, ref IEnumerable<FileCabinetRecord> records)
        {
            switch (key.ToUpperInvariant())
            {
                case "ID":
                    int id = Program.ReadInput(parameterValue, Converter.IntConverter, InputValidator.IdValidator);
                    records = records.Union(this.FileCabinetService.FindById(id));
                    break;
                case "FIRSTNAME":
                    var firstName = Program.ReadInput(parameterValue, Converter.StringConverter, inputValidator.FirstNameValidator);
                    records = records.Union(this.FileCabinetService.FindByFirstName(firstName));
                    break;
                case "LASTNAME":
                    var lastName = Program.ReadInput(parameterValue, Converter.StringConverter, inputValidator.LastNameValidator);
                    records = records.Union(this.FileCabinetService.FindByLastName(lastName));
                    break;
                case "DATEOFBIRTH":
                    var dateOfBirthday = Program.ReadInput(parameterValue, Converter.DateConverter, inputValidator.DateOfBirthValidator);
                    records = records.Union(this.FileCabinetService.FindByDateOfBirthday(dateOfBirthday));
                    break;
                case "WORKINGHOURS":
                    var workingHoursPerWeek = Program.ReadInput(parameterValue, Converter.ShortConverter, inputValidator.WorkingHoursValidator);
                    records = records.Union(this.FileCabinetService.FindByWorkingHours(workingHoursPerWeek));
                    break;
                case "ANNUALINCOME":
                    var annualIncome = Program.ReadInput(parameterValue, Converter.DecimalConverter, inputValidator.AnnualIncomeValidator);
                    records = records.Union(this.FileCabinetService.FindByAnnualIncome(annualIncome));
                    break;
                case "DRIVERCATEGORY":
                    var driverLicenseCategory = Program.ReadInput(parameterValue, Converter.CharConverter, inputValidator.DriverLicenseCategoryValidator);
                    records = records.Union(this.FileCabinetService.FindByDriverCategory(driverLicenseCategory));
                    break;
            }
        }
    }
}