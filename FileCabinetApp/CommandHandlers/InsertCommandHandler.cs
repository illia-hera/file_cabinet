using System;
using System.Collections.Generic;
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
    /// Implement insert command.
    /// </summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.CabinetServiceCommandHandlerBase" />
    public class InsertCommandHandler : CabinetServiceCommandHandlerBase
    {
        private const int CountOfParameters = 7;
        private static InputValidator inputValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertCommandHandler" /> class.
        /// </summary>
        /// <param name="fileCabinetService">The file cabinet service.</param>
        /// <param name="validator">The validator.</param>
        public InsertCommandHandler(IFileCabinetService fileCabinetService, InputValidator validator)
            : base(fileCabinetService)
        {
            inputValidator = validator;
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

            if (appCommandRequest.Command.Equals("insert", StringComparison.OrdinalIgnoreCase))
            {
                ParametersContainer container = new ParametersContainer();
                var tuple = ParseParameters(appCommandRequest.Parameters);
                if (tuple is null)
                {
                    Console.WriteLine($"Please, correct your input. Count of parameters must be equals {CountOfParameters}");
                    return;
                }

                var (names, values) = tuple;
                for (int i = 0; i < names.Length; i++)
                {
                    try
                    {
                        TryInitializeParameters(container, names[i], values[i]);
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine(e.Message);
                        return;
                    }
                }

                Console.WriteLine(this.FileCabinetService.InsertRecord(container)
                    ? $"Record #{container.Id} is inserted."
                    : $"Record with #{container.Id} is exist.");

                return;
            }

            base.Handle(appCommandRequest);
        }

        private static Tuple<string[], string[]> ParseParameters(string parameters)
        {
            string[] inputs = parameters.Split(" values ");
            if (inputs.Length != 2)
            {
                return null;
            }

            int paramNamesIndex = 0;
            int paramValuesIndex = 1;

            string[] names = inputs[paramNamesIndex].Trim('(', ')').Split(", ");
            string[] values = inputs[paramValuesIndex].Trim('(', ')').Split(", ");

            for (int i = 0; i < values.Length; i++)
            {
                values[i] = values[i].Trim('\'');
            }

            return names.Length == values.Length && names.Length == CountOfParameters ? new Tuple<string[], string[]>(names, values) : null;
        }

        private static void TryInitializeParameters(ParametersContainer container, string parameterName, string parameterValue)
        {
            switch (parameterName.ToUpperInvariant())
            {
                case "ID":
                    container.Id = ParameterReaders.ReadInput(parameterValue, Converter.IntConverter, InputValidator.IdValidator);
                    break;
                case "FIRSTNAME":
                    container.FirstName = ParameterReaders.ReadInput(parameterValue, Converter.StringConverter, inputValidator.FirstNameValidator);
                    break;
                case "LASTNAME":
                    container.LastName = ParameterReaders.ReadInput(parameterValue, Converter.StringConverter, inputValidator.LastNameValidator);
                    break;
                case "DATEOFBIRTH":
                    container.DateOfBirthday = ParameterReaders.ReadInput(parameterValue, Converter.DateConverter, inputValidator.DateOfBirthValidator);
                    break;
                case "WORKINGHOURS":
                    container.WorkingHoursPerWeek = ParameterReaders.ReadInput(parameterValue, Converter.ShortConverter, inputValidator.WorkingHoursValidator);
                    break;
                case "ANNUALINCOME":
                    container.AnnualIncome = ParameterReaders.ReadInput(parameterValue, Converter.DecimalConverter, inputValidator.AnnualIncomeValidator);
                    break;
                case "DRIVERCATEGORY":
                    container.DriverLicenseCategory = ParameterReaders.ReadInput(parameterValue, Converter.CharConverter, inputValidator.DriverLicenseCategoryValidator);
                    break;
            }
        }
    }
}
