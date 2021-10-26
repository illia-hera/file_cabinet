using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using CommandLine;
using FileCabinetApp.CommandHandlers;
using FileCabinetApp.Entities;
using FileCabinetApp.Readers;
using FileCabinetApp.Services;
using FileCabinetApp.Utility;
using FileCabinetApp.Validators.InputValidators;
using FileCabinetApp.Validators.RecordValidator;

[assembly: CLSCompliant(false)]

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

        private static InputValidator inputValidator;

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
                    var input = Console.ReadLine() ?? throw new ArgumentException("Input can not be null");
                    var inputs = input.Split(' ', 2);
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
            container.FirstName = ParameterReaders.ReadInput(Converter.StringConverter, inputValidator.FirstNameValidator);

            Console.Write("Last name: ");
            container.LastName = ParameterReaders.ReadInput(Converter.StringConverter, inputValidator.LastNameValidator);

            Console.Write("Date of birth: ");
            container.DateOfBirthday = ParameterReaders.ReadInput(Converter.DateConverter, inputValidator.DateOfBirthValidator);

            Console.Write("Working Hours Per Week: ");
            container.WorkingHoursPerWeek = ParameterReaders.ReadInput(Converter.ShortConverter, inputValidator.WorkingHoursValidator);

            Console.Write("Annual Income: ");
            container.AnnualIncome = ParameterReaders.ReadInput(Converter.DecimalConverter, inputValidator.AnnualIncomeValidator);

            Console.Write("Driver License Category: ");
            container.DriverLicenseCategory = ParameterReaders.ReadInput(Converter.CharConverter, inputValidator.DriverLicenseCategoryValidator);

            return container;
        }

        private static ICommandHandler CreateCommandHandler()
        {
            var commandHandler = new CreateCommandHandler(fileCabinetService);

            commandHandler.SetNext(new HelpCommandHandler())
                .SetNext(new StatCommandHandler(fileCabinetService))
                .SetNext(new ExitCommandHandler(isR => isRunning = isR))
                .SetNext(new ExportCommandHandler(fileCabinetService))
                .SetNext(new ImportCommandHandler(fileCabinetService))
                .SetNext(new PurgeCommandHandler(fileCabinetService))
                .SetNext(new InsertCommandHandler(fileCabinetService, inputValidator))
                .SetNext(new DeleteCommandHandler(fileCabinetService, inputValidator))
                .SetNext(new UpdateCommandHandler(fileCabinetService, inputValidator))
                .SetNext(new SelectCommandHandler(fileCabinetService, inputValidator));

            return commandHandler;
        }

        private static void DefaultRecordPrint(IEnumerable<FileCabinetRecord> records)
        {
            foreach (FileCabinetRecord record in records)
            {
                Console.WriteLine($"#{record.Id}," +
                                  $" {record.FirstName}," +
                                  $" {record.LastName}," +
                                  $" {record.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.CreateSpecificCulture("en-US"))}," +
                                  $" working hours: {record.WorkingHoursPerWeek}," +
                                  $" annual income: {record.AnnualIncome}," +
                                  $" driver category: {record.DriverLicenseCategory}.");
            }
        }

        private static void InitFileCabinetService(string[] args)
        {
            IRecordValidator recordValidator;
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(
                    o =>
                    {
                        if (o.ValidationRules.Equals("custom", StringComparison.OrdinalIgnoreCase))
                        {
                            recordValidator = new ValidatorBuilder().CreateCustomValidator();
                            inputValidator = new InputValidator("custom");
                            Console.WriteLine("Using custom validation rules.");
                        }
                        else
                        {
                            recordValidator = new ValidatorBuilder().CreateDefaultValidator();
                            inputValidator = new InputValidator("default");
                            Console.WriteLine("Using default validation rules.");
                        }

                        if (o.StorageRules.Equals("file", StringComparison.OrdinalIgnoreCase))
                        {
                            fileCabinetService = new FileCabinetFilesystemService(new FileStream("cabinet-records.db", FileMode.Create), recordValidator);
                            Console.WriteLine("Using file storage rules.");
                        }
                        else
                        {
                            fileCabinetService = new FileCabinetMemoryService(recordValidator);
                            Console.WriteLine("Using memory storage rules.");
                        }

                        if (o.StopWatchUse)
                        {
                            fileCabinetService = new ServiceMeter(fileCabinetService);
                            Console.WriteLine("Using stopWatch.");
                        }

                        if (o.UseLogger)
                        {
                            fileCabinetService = new ServiceLogger(fileCabinetService);
                            Console.WriteLine("Using logger.");
                        }
                    });
        }
    }
}