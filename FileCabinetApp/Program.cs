using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using FileCabinetApp.CommandHandlers;
using FileCabinetApp.Entities;
using FileCabinetApp.Printers;
using FileCabinetApp.Services;
using FileCabinetApp.Services.FileService;
using FileCabinetApp.Services.MemoryService;
using FileCabinetApp.Services.SnapshotServices;
using FileCabinetApp.Utility;
using FileCabinetApp.Validators;

namespace FileCabinetApp
{
    /// <summary>
    /// Class <c>Program</c> - initial class.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Ilya Gerasimchik";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";

        private static bool isRunning = true;

        private static IFileCabinetService fileCabinetService;

        private static IRecordValidator validator;

        /// <summary>
        ///   <para>
        /// Gets the validator.
        /// </para>
        /// </summary>
        /// <value>The validator.</value>
        public static IRecordValidator Validator
        {
            get
            {
                return validator ??= fileCabinetService switch
                {
                    FileCabinetMemoryCustomService _ => new CustomValidator(),
                    _ => new DefaultValidator(),
                };
            }
        }

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            fileCabinetService = InitFileCabinetService(args);
            var commandHandler = CreateCommandHandler();

            do
            {
                    Console.Write("> ");
                    var inputs = Console.ReadLine().Split(' ', 2);
                    const int commandIndex = 0;
                    const int parameterIndex = 1;

                    commandHandler.Handle(new AppCommandRequest(inputs[commandIndex], inputs.Length > 1 ? inputs[parameterIndex] : string.Empty));
            }
            while (isRunning);
        }

        /// <summary>
        /// Gets the valid input parameters.
        /// </summary>
        /// <returns>Return container with parameters.</returns>
        public static ParametersContainer GetValidInputParameters()
        {
            var container = new ParametersContainer();

            Console.Write("First name: ");
            container.FirstName = ReadInput(Converter.StringConverter, Validator.FirstNameValidator);

            Console.Write("Last name: ");
            container.LastName = ReadInput(Converter.StringConverter, Validator.LastNameValidator);

            Console.Write("Date of birth: ");
            container.DateOfBirthday = ReadInput(Converter.DateConverter, Validator.DateOfBirthValidator);

            Console.Write("Working Hours Per Week: ");
            container.WorkingHoursPerWeek = ReadInput(Converter.ShortConverter, Validator.WorkingHoursValidator);

            Console.Write("Annual Income: ");
            container.AnnualIncome = ReadInput(Converter.DecimalConverter, Validator.AnnualIncomeValidator);

            Console.Write("Driver License Category: ");
            container.DriverLicenseCategory = ReadInput(Converter.CharConverter, Validator.DriverLicenseCategoryValidator);

            return container;
        }

        private static ICommandHandler CreateCommandHandler()
        {
            var commandHandler = new CreateCommandHandler(fileCabinetService);
            var defaultPrinter = new DefaultRecordPrinter();

            commandHandler.SetNext(new HelpCommandHandler())
                .SetNext(new StatCommandHandler(fileCabinetService))
                .SetNext(new ExitCommandHandler(isR => isRunning = isR))
                .SetNext(new ListCommandHandler(fileCabinetService, defaultPrinter))
                .SetNext(new EditCommandHandler(fileCabinetService))
                .SetNext(new FindCommandHandler(fileCabinetService, defaultPrinter))
                .SetNext(new ExportCommandHandler(fileCabinetService))
                .SetNext(new ImportCommandHandler(fileCabinetService))
                .SetNext(new RemoveCommandHandler(fileCabinetService))
                .SetNext(new PurgeCommandHandler(fileCabinetService));

            return commandHandler;
        }

        private static IFileCabinetService InitFileCabinetService(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            if (args?.Length != 0)
            {
                const int initialCommandIndex = 0;
                const int initialCommandValueIndex = 1;
                string parameter = args![initialCommandIndex];
                var parameterValue = args.Length > 1 ? args[initialCommandValueIndex] : string.Empty;
                if (string.IsNullOrEmpty(parameterValue))
                {
                    string[] splitParameter = parameter.Split('=', 2);
                    parameter = splitParameter[initialCommandIndex];
                    parameterValue = splitParameter[initialCommandValueIndex];
                }

                if (parameter.Equals("-s", StringComparison.OrdinalIgnoreCase) || parameter.Equals("--storage", StringComparison.OrdinalIgnoreCase))
                {
                    if (parameterValue.Equals("file", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("Using file service");
                        Console.WriteLine("Using default validation rules");
                        return new FileCabinetFilesystemService(File.Open("cabinet-records.db", FileMode.Create));
                    }
                }

                if (parameter.Equals("-v", StringComparison.OrdinalIgnoreCase) || parameter.Equals("--validation-rules", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Using memory service");
                    if (parameterValue.Equals("custom", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("Using custom validation rules");
                        return new FileCabinetMemoryCustomService();
                    }

                    return new FileCabinetMemoryDefaultService();
                }
            }

            Console.WriteLine("Using memory service");
            Console.WriteLine("Using default validation rules");
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();
            return new FileCabinetMemoryDefaultService();
        }

        private static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        {
            do
            {
                T value;

                var input = Console.ReadLine();
                var conversionResult = converter(input);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                    continue;
                }

                value = conversionResult.Item3;

                var validationResult = validator(value);
                if (!validationResult.Item1)
                {
                    Console.WriteLine($"Validation failed: {validationResult.Item2}. Please, correct your input.");
                    continue;
                }

                return value;
            }
            while (true);
        }
    }
}