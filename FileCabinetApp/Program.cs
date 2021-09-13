﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using FileCabinetApp.Enteties;
using FileCabinetApp.Services;
using FileCabinetApp.SnapshotServices;
using FileCabinetApp.Utils;
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
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static bool isRunning = true;

        private static IFileCabinetService fileCabinetService;

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
            new Tuple<string, Action<string>>("export", Export),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "prints counts of records.", "The 'stat' command prints counts of records." },
            new string[] { "create", "create new record.", "The 'create' command create new record." },
            new string[] { "list", "returned list of created records.", "The 'list' command returned list of created records." },
            new string[] { "edit", "edit record parameters.", "The 'edit' command edit record parameters." },
            new string[] { "find", "find record by parameter.", "The 'find' command find record by parameter." },
            new string[] { "export csv", "export records to csv file.", "The 'export csv' command export records to csv file." },
        };

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();

            if (args?.Length == 0)
            {
                fileCabinetService = new FileCabinetDefaultService();
            }
            else
            {
                const int initialCommandIndex = 0;
                const int initialCommandValueIndex = 1;
                string parameter = args[initialCommandIndex];
                var parameterValue = args.Length > 1 ? args[initialCommandValueIndex] : string.Empty;
                if (string.IsNullOrEmpty(parameterValue))
                {
                    string[] splitedParameter = parameter.Split('=', 2);
                    parameter = splitedParameter[initialCommandIndex];
                    parameterValue = splitedParameter[initialCommandValueIndex];
                }

                if (parameter.Equals("-v") || parameter.Equals("--validation-rules"))
                {
                    fileCabinetService = parameterValue switch
                    {
                        var p when p.Equals("default", StringComparison.OrdinalIgnoreCase) => new FileCabinetDefaultService(),
                        var p when p.Equals("custom", StringComparison.OrdinalIgnoreCase) => new FileCabinetCustomService(),
                        _ => new FileCabinetDefaultService()
                    };
                }
            }

            do
            {
                Console.Write("> ");
                var inputs = Console.ReadLine().Split(' ', 2);
                const int commandIndex = 0;
                var command = inputs[commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.OrdinalIgnoreCase));
                if (index >= 0)
                {
                    const int parametersIndex = 1;
                    var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                    commands[index].Item2(parameters);
                }
                else
                {
                    PrintMissedCommandInfo(command);
                }
            }
            while (isRunning);
        }

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.OrdinalIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][Program.ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in helpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[Program.CommandHelpIndex], helpMessage[Program.DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }

        private static void Stat(string parameters)
        {
            var recordsCount = Program.fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
        }

        private static void Create(string parameters)
        {
            IRecordValidator validator = GetValidator();

            ParametersContainer container = GetValidInputParameters(validator);

            int recordId = fileCabinetService.CreateRecord(container);
            Console.WriteLine($"Record #{recordId} is created.");
        }

        private static void List(string parameters)
        {
            IReadOnlyCollection<FileCabinetRecord> recordsCollection = fileCabinetService.GetRecords();
            if (recordsCollection.Count == 0)
            {
                Console.WriteLine("No records yet.");
                return;
            }

            foreach (FileCabinetRecord record in recordsCollection)
            {
                Console.WriteLine($"#{record.Id}," +
                                  $" {record.FirstName}," +
                                  $" {record.LastName}," +
                                  $" {record.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.CreateSpecificCulture("en-US"))}");
            }
        }

        private static void Edit(string parameters)
        {
            bool isParsedId = int.TryParse(parameters, out int id);

            IReadOnlyCollection<FileCabinetRecord> recordsCollection = fileCabinetService.GetRecords();
            var record = recordsCollection.FirstOrDefault(r => r.Id == id);

            if (!isParsedId || record is null)
            {
                Console.WriteLine($"#{id} record is not found.");
                return;
            }

            IRecordValidator validator = GetValidator();
            ParametersContainer container = GetValidInputParameters(validator);

            fileCabinetService.EditRecord(id, container);
            Console.WriteLine($"Record #{id} is updated.");
        }

        private static void Find(string parameters)
        {
            string[] inputs = parameters.Split(' ', 2);
            const int parameterIndex = 0;
            const int valueIndex = 1;
            string parameter = inputs[parameterIndex];
            string value = inputs.Length > 1 ? inputs[valueIndex] : string.Empty;

            IReadOnlyCollection<FileCabinetRecord> recordsCollection = parameter switch
            {
                var p when p.Equals("lastName", StringComparison.OrdinalIgnoreCase) => fileCabinetService.FindByFirstName(value.Trim('\"')),
                var p when p.Equals("lastName", StringComparison.OrdinalIgnoreCase) => fileCabinetService.FindByLastName(value.Trim('\"')),
                var p when p.Equals("dateOfBirth", StringComparison.OrdinalIgnoreCase)
                                && DateTime.TryParse(value.Trim('\"'), out DateTime dateOfBd) => fileCabinetService.FindByDateOfBirthName(dateOfBd),
                _ => Array.Empty<FileCabinetRecord>()
            };

            if (recordsCollection.Count == 0)
            {
                Console.WriteLine($"No records with {parameter} - {value}.");
            }

            foreach (FileCabinetRecord record in recordsCollection)
            {
                Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.CreateSpecificCulture("en-US"))}");
            }
        }

        private static ParametersContainer GetValidInputParameters(IRecordValidator validator)
        {
            var container = new ParametersContainer();

            Console.Write("First name: ");
            container.FirstName = ReadInput(Converter.StringConverter, validator.FirstNameValidator);

            Console.Write("Last name: ");
            container.LastName = ReadInput(Converter.StringConverter, validator.LastNameValidator);

            Console.Write("Date of birth: ");
            container.DateOfBirthday = ReadInput(Converter.DateConverter, validator.DateOfBirthValidator);

            Console.Write("Working Hours Per Week: ");
            container.WorkingHoursPerWeek = ReadInput(Converter.ShortConverter, validator.WorkingHoursValidator);

            Console.Write("Annual Income: ");
            container.AnnualIncome = ReadInput(Converter.DecimalConverter, validator.AnnualIncomeValidator);

            Console.Write("Driver License Category: ");
            container.DriverLicenseCategory = ReadInput(Converter.CharConverter, validator.DriverLicenseCategoryValidator);

            return container;
        }

        private static IRecordValidator GetValidator()
        {
            IRecordValidator validator;
            switch (fileCabinetService)
            {
                case FileCabinetCustomService car:
                    validator = new CustomValidator();
                    break;
                default:
                    validator = new DefaultValidator();
                    break;
            }

            return validator;
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

        private static void Export(string parameters)
        {
            string[] inputs = parameters.Split(' ', 2);
            const int parameterIndex = 0;
            const int valueIndex = 1;
            string fileFormat = inputs[parameterIndex];
            string filePath = inputs.Length > 1 ? inputs[valueIndex] : string.Empty;
            int lastIndexOfBackSlash = filePath.LastIndexOf("\\", StringComparison.OrdinalIgnoreCase);
            string fileDirection = lastIndexOfBackSlash > 0 ? filePath[..lastIndexOfBackSlash] : string.Empty;

            if (string.IsNullOrWhiteSpace(filePath) || (!string.IsNullOrEmpty(fileDirection) && (!Directory.Exists(fileDirection) || string.IsNullOrEmpty(filePath))))
            {
                Console.WriteLine($"Export failed: can't open file {filePath}.");
                return;
            }

            if (File.Exists(filePath))
            {
                Console.Write($"File is exist - rewrite {filePath}? [Y/n] ");
                string answer = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(answer) || !answer.Equals("y", StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }
            }

            IFileCabinetServiceSnapshot snapshot = fileCabinetService.MakeSnapshot();
            if (fileFormat.Equals("csv", StringComparison.OrdinalIgnoreCase))
            {
                using StreamWriter streamWriter = new StreamWriter(filePath);
                snapshot.SaveToCsv(streamWriter);
                streamWriter.Close();
            }
            else if (fileFormat.Equals("xml", StringComparison.OrdinalIgnoreCase))
            {
                using XmlWriter xmlWriter = XmlWriter.Create(filePath);
                snapshot.SaveToXml(xmlWriter);
                xmlWriter.Close();
            }
            else
            {
                return;
            }

            Console.WriteLine($"All records are exported to file {filePath!.Split('\\')[^1]}");
        }
    }
}