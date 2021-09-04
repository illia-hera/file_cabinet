using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace FileCabinetApp
{
    public static class Program
    {
        private const string DeveloperName = "Ilya Gerasimchik";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static bool isRunning = true;

        private static FileCabinetService fileCabinetService = new FileCabinetService();

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "prints counts of records.", "The 'stat' command prints counts of records." },
            new string[] { "create", "create new user.", "The 'create' command create new user." },
            new string[] { "list", "returned list of created users.", "The 'list' command returned list of created users." },
            new string[] { "edit", "edit user parameters.", "The 'edit' command edit user parameters." },
            new string[] { "find", "find user by parameter.", "The 'find' command find user by parameter." },
        };

        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();

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
            GetPersonParameters(out string firstName, out string lastName, out DateTime dateOfBd, out short workingHoursPerWeek, out decimal annualIncome, out char driverLicenseCategory);

            int recordId = fileCabinetService.CreateRecord(firstName, lastName, dateOfBd, workingHoursPerWeek, annualIncome, driverLicenseCategory);
            Console.WriteLine($"Record #{recordId} is created.");
        }

        private static void List(string parameters)
        {
            FileCabinetRecord[] records = fileCabinetService.GetRecords();
            if (records.Length == 0)
            {
                Console.WriteLine("No records yet.");
                return;
            }

            foreach (FileCabinetRecord record in records)
            {
                Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.CreateSpecificCulture("en-US"))}");
            }
        }

        private static void Edit(string parameters)
        {
            bool isParsedId = int.TryParse(parameters, out int id);

            FileCabinetRecord[] records = fileCabinetService.GetRecords();
            var record = records.FirstOrDefault(r => r.Id == id);

            if (!isParsedId || record is null)
            {
                Console.WriteLine($"#{id} record is not found.");
                return;
            }

            GetPersonParameters(out string firstName, out string lastName, out DateTime dateOfBd, out short workingHoursPerWeek, out decimal annualIncome, out char driverLicenseCategory);
            fileCabinetService.EditRecord(id, firstName, lastName, dateOfBd, workingHoursPerWeek, annualIncome, driverLicenseCategory);
            Console.WriteLine($"Record #{id} is updated.");
        }

        private static void GetPersonParameters(out string firstName, out string lastName, out DateTime dateOfBd, out short workingHoursPerWeek, out decimal annualIncome, out char driverLicenseCategory)
        {
            string[] parameterNames = { "First name", "Last name", "Date of birth", "Working Hours Per Week", "Annual Income", "Driver License Category" };
            firstName = null;
            lastName = null;
            dateOfBd = default;
            annualIncome = default;
            driverLicenseCategory = default;
            workingHoursPerWeek = default;

            for (int i = 0; i < parameterNames.Length;)
            {
                Console.Write($"{parameterNames[i]}: ");
                string personParameter = Console.ReadLine();
                bool isParsed = parameterNames[i] switch
                {
                    var name when name == "First name" => Validator.TryGetValidFirstName(personParameter, out firstName),
                    var name when name == "Last name" => Validator.TryGetValidLastName(personParameter, out lastName),
                    var name when name == "Working Hours Per Week" => Validator.TryGetValidWorkingHoursPerWeek(personParameter, out workingHoursPerWeek),
                    var name when name == "Driver License Category" => Validator.TryGetValidDriverLicenseCategory(personParameter, out driverLicenseCategory),
                    var name when name == "Annual Income" => Validator.TryGetValidAnnualIncome(personParameter, out annualIncome),
                    var name when name == "Date of birth" => Validator.TryGetValidDateTimeOfBd(personParameter, out dateOfBd),
                    _ => false
                };

                i = isParsed ? i + 1 : i;
            }
        }

        private static void Find(string parameters)
        {
            string[] inputs = parameters.Split(' ', 2);
            const int parameterIndex = 0;
            const int valueIndex = 1;
            string parameter = inputs[parameterIndex];
            string value = inputs.Length > 1 ? inputs[valueIndex] : string.Empty;

            FileCabinetRecord[] records = parameter switch
            {
                var p when p.Equals("firstName", StringComparison.OrdinalIgnoreCase) => fileCabinetService.FindByFirstName(value.Trim('\"')),
                _ => Array.Empty<FileCabinetRecord>()
            };

            foreach (FileCabinetRecord record in records)
            {
                Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.CreateSpecificCulture("en-US"))}");
            }
        }
    }
}