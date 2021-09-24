using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using CommandLine;
using FileCabinetApp.CommandHandlers;
using FileCabinetApp.Entities;
using FileCabinetApp.Printers;
using FileCabinetApp.Services;
using FileCabinetApp.Services.FileService;
using FileCabinetApp.Services.MemoryService;
using FileCabinetApp.Services.SnapshotServices;
using FileCabinetApp.Utility;
using FileCabinetApp.Validators;
using FileCabinetApp.Validators.InputValidators;
using FileCabinetApp.Validators.InputValidators.ValidationRule;
using FileCabinetApp.Validators.RecordValidator;

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

        private static InputValidator validator;

        /// <summary>
        ///   <para>
        /// Gets the validator.
        /// </para>
        /// </summary>
        /// <value>The validator.</value>
        public static InputValidator Validator
        {
            get
            {
                return validator ??= fileCabinetService switch
                {
                    FileCabinetMemoryCustomService _ => new InputValidator(new ValidationCustomRules()),
                    _ => new InputValidator(new ValidationDefaultRules()),
                };
            }
        }

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {DeveloperName}");
            InitFileCabinetService(args);
            ICommandHandler commandHandler = CreateCommandHandler();
            Console.WriteLine(HintMessage);
            Console.WriteLine();

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

            commandHandler.SetNext(new HelpCommandHandler())
                .SetNext(new StatCommandHandler(fileCabinetService))
                .SetNext(new ExitCommandHandler(isR => isRunning = isR))
                .SetNext(new ListCommandHandler(fileCabinetService, DefaultRecordPrint))
                .SetNext(new EditCommandHandler(fileCabinetService))
                .SetNext(new FindCommandHandler(fileCabinetService, DefaultRecordPrint))
                .SetNext(new ExportCommandHandler(fileCabinetService))
                .SetNext(new ImportCommandHandler(fileCabinetService))
                .SetNext(new RemoveCommandHandler(fileCabinetService))
                .SetNext(new PurgeCommandHandler(fileCabinetService));

            return commandHandler;
        }

        private static void DefaultRecordPrint(IEnumerable<FileCabinetRecord> records)
        {
            foreach (FileCabinetRecord record in records)
            {
                Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.CreateSpecificCulture("en-US"))}");
            }
        }

        private static void InitFileCabinetService(string[] args)
        {
            IRecordValidator recordValidator = new ValidatorBuilder().CreateDefault();
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(
                    o =>
                    {
                        if ((o.ValidationRules == null) || o.ValidationRules.Equals("default", StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine("Using default validation rules.");
                        }
                        else if (o.ValidationRules.Equals("custom", StringComparison.OrdinalIgnoreCase))
                        {
                            recordValidator = new ValidatorBuilder().CreateCustom();
                            Console.WriteLine("Using custom validation rules.");
                        }

                        if ((o.StorageRules == null) || o.StorageRules.Equals("memory", StringComparison.OrdinalIgnoreCase))
                        {
                            fileCabinetService = new FileCabinetMemoryService(recordValidator);
                            Console.WriteLine("Using memory storage rules.");
                        }
                        else if (o.StorageRules.Equals("file", StringComparison.OrdinalIgnoreCase))
                        {
                            fileCabinetService = !File.Exists("cabinet-records.db")
                                                     ? new FileCabinetFilesystemService(new FileStream("cabinet-records.db", FileMode.OpenOrCreate), recordValidator)
                                                     : new FileCabinetFilesystemService(new FileStream("cabinet-records.db", FileMode.Open), recordValidator);
                            Console.WriteLine("Using file storage rules.");
                        }
                    });
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