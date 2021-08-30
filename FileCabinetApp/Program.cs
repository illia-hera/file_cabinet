﻿using System;
using System.Collections.Generic;
using System.Globalization;

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
            new Tuple<string, Action<string>>("list", Program.List),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "prints counts of records.", "The 'stat' command prints counts of records." },
            new string[] { "create", "create new user.", "The 'create' command create new user." },
            new string[] { "list", "returned list of created users.", "The 'list' command returned list of created users." },
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
            string[] parameterName = { "First name", "Last name", "Date of birth", "property1", "property1", "property1" };
            Type[] types = {typeof(string), typeof(string), typeof(DateTime), typeof(short), typeof(decimal), typeof(char) };
            string[] personParams = new string[parameterName.Length];
            DateTime dateOfBd = default;
            decimal decimalValue = default;
            char charValue = default;
            short shortValue = default;

            for (int i = 0; i < parameterName.Length;)
            {
                Console.Write($"{parameterName[i]}: ");
                personParams[i] = Console.ReadLine();
                bool isParsed = Type.GetTypeCode(types[i]) switch
                {
                    TypeCode.String => !string.IsNullOrWhiteSpace(personParams[i]),
                    TypeCode.Int16 => short.TryParse(personParams[i], out shortValue),
                    TypeCode.Char => char.TryParse(personParams[i], out charValue),
                    TypeCode.Decimal => decimal.TryParse(personParams[i], out decimalValue),
                    TypeCode.DateTime => Parser.TryParseDateTimeBd(personParams[i], out dateOfBd),
                    _ => false
                };

                i = isParsed ? i + 1 : i;
            }

            fileCabinetService.CreateRecord(personParams[0], personParams[1], dateOfBd, shortValue, decimalValue, charValue);
        }

        private static void List(string parameters)
        {
            var records = fileCabinetService.GetRecords();
            string dateTimeFormat = "yyyy-MMM-dd";
            var culture = CultureInfo.CreateSpecificCulture("en-US");
            if (records.Length == 0)
            {
                Console.WriteLine("No records yet.");
            }

            foreach (FileCabinetRecord record in records)
            {
                Console.WriteLine("#{0}, {1}, {2}, {3}", record.Id, record.FirstName, record.LastName, record.DateOfBirth.ToString(dateTimeFormat, culture));
            }
        }
    }
}