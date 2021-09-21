using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using FileCabinetApp.Entities;
using FileCabinetApp.Services;
using FileCabinetApp.Services.FileService;
using FileCabinetApp.Services.MemoryService;
using FileCabinetApp.Services.SnapshotServices;
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
            new Tuple<string, Action<string>>("import", Import),
            new Tuple<string, Action<string>>("remove", Remove),
            new Tuple<string, Action<string>>("purge", Purge),
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
            new string[] { "export csv/xml", "export records to csv/xml file.", "The 'export csv/xml' command export records to csv/xml file." },
            new string[] { "import csv/xml", "import records from csv file.", "The 'import csv/xml' command import records from csv file." },
            new string[] { "remove", "remove record from FileCabinet.", "The 'remove' command remove records from FileCabinet." },
            new string[] { "purge", "purge bites from file.", "The 'purge' command purge bites from file." },
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

            fileCabinetService = InitFileCabinetService(args);

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

        private static IFileCabinetService InitFileCabinetService(string[] args)
        {
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

                if (parameter.Equals("-v") || parameter.Equals("--validation-rules"))
                {
                    return parameterValue switch
                    {
                        var p when p.Equals("custom", StringComparison.OrdinalIgnoreCase) => new FileCabinetMemoryCustomService(),
                        _ => new FileCabinetMemoryDefaultService()
                    };
                }

                if (parameter.Equals("-s") || parameter.Equals("--storage"))
                {
                    return parameterValue switch
                    {
                        var p when p.Equals("file", StringComparison.OrdinalIgnoreCase) => new FileCabinetFilesystemService(File.Open("cabinet-records.db", FileMode.Create)),
                        var p when p.Equals("memory", StringComparison.OrdinalIgnoreCase) => new FileCabinetMemoryDefaultService(),
                        _ => new FileCabinetMemoryDefaultService()
                    };
                }
            }

            return new FileCabinetMemoryDefaultService();
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
            Console.WriteLine($"{recordsCount.Item1} record(s), {recordsCount.Item2} record(s) deleted.");
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
                var p when p.Equals("firstname", StringComparison.OrdinalIgnoreCase) => fileCabinetService.FindByFirstName(value.Trim('\"')),
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
                case FileCabinetMemoryCustomService car:
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
            var parametersTuple = ValidateImportExportParameters(parameters);
            if (File.Exists(parametersTuple.Item2))
            {
                Console.Write($"File is exist - rewrite {parametersTuple.Item2}? [Y/n] ");
                string answer = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(answer) || !answer.Equals("y", StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }
            }

            IFileCabinetServiceSnapshot snapshot = fileCabinetService.MakeSnapshot();
            if (parametersTuple.Item1 && parametersTuple.Item3.Equals("csv", StringComparison.OrdinalIgnoreCase))
            {
                using StreamWriter streamWriter = new StreamWriter(parametersTuple.Item2);
                snapshot.SaveToCsv(streamWriter);
                streamWriter.Close();
            }
            else if (parametersTuple.Item1 && parametersTuple.Item3.Equals("xml", StringComparison.OrdinalIgnoreCase))
            {
                using XmlWriter xmlWriter = XmlWriter.Create(parametersTuple.Item2);
                snapshot.SaveToXml(xmlWriter);
                xmlWriter.Close();
            }
            else
            {
                return;
            }

            Console.WriteLine($"All records are exported to file {parametersTuple.Item2!.Split('\\')[^1]}");
        }

        private static void Import(string parameters)
        {
            var parametersTuple = ValidateImportExportParameters(parameters);

            using var fs = new FileStream(parametersTuple.Item2, FileMode.Open, FileAccess.Read);
            IFileCabinetServiceSnapshot snapshot = fileCabinetService.MakeSnapshot();
            if (parametersTuple.Item1 && parametersTuple.Item3.Equals("csv", StringComparison.OrdinalIgnoreCase))
            {
                using StreamReader streamReader = new StreamReader(fs);
                snapshot.LoadFromCsv(streamReader);
                fileCabinetService.Restore(snapshot);
            }
            else if (parametersTuple.Item1 && parametersTuple.Item3.Equals("xml", StringComparison.OrdinalIgnoreCase))
            {
                using StreamReader streamReader = new StreamReader(fs);
                snapshot.LoadFromXml(streamReader);
                fileCabinetService.Restore(snapshot);
            }
            else
            {
                return;
            }

            Console.WriteLine($"{snapshot.Records.Count} records were imported from file {parametersTuple.Item2!.Split('\\')[^1]}");
        }

        private static Tuple<bool, string, string> ValidateImportExportParameters(string parameters)
        {
            string[] inputs = parameters.Split(' ', 2);
            const int parameterIndex = 0;
            const int valueIndex = 1;
            string fileFormat = inputs[parameterIndex];
            string filePath = inputs.Length > 1 ? inputs[valueIndex] : string.Empty;
            int lastIndexOfBackSlash = filePath.LastIndexOf("\\", StringComparison.OrdinalIgnoreCase);
            string fileDirection = lastIndexOfBackSlash > 0 ? filePath[..lastIndexOfBackSlash] : string.Empty;

            if (string.IsNullOrWhiteSpace(filePath) || (!string.IsNullOrEmpty(fileDirection) && (!Directory.Exists(fileDirection))))
            {
                Console.WriteLine($"Export failed: can't open file {filePath}.");
                return new Tuple<bool, string, string>(false, null, null);
            }

            return new Tuple<bool, string, string>(true, filePath, fileFormat);
        }

        private static void Remove(string parameters)
        {
            if (!int.TryParse(parameters, out var id))
            {
                Console.WriteLine("Please, enter the id value in format \"edit n\", where n is an integer value greater than zero. Please try again.");
                return;
            }

            try
            {
                List<FileCabinetRecord> records = fileCabinetService.GetRecords().ToList();
                if (!records.Exists(x => x.Id == id))
                {
                    throw new ArgumentException($"{id} id record is not found.");
                }

                fileCabinetService.RemoveRecord(id);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            Console.WriteLine($"Record #{id} is removed.");
        }

        private static void Purge(string parameters)
        {
            if (fileCabinetService is FileCabinetFilesystemService filesystemService)
            {
                var items = fileCabinetService.GetStat();
                var deleted = filesystemService.Purge();
                Console.WriteLine($"Data file processing is completed: {deleted} of {items.Item1} records were purged.");
            }
        }
    }
}