using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ConsoleTables;
using FileCabinetApp.Entities;
using FileCabinetApp.Readers;
using FileCabinetApp.Services;
using FileCabinetApp.Utility;
using FileCabinetApp.Validators.InputValidators;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Implementation selection command.
    /// </summary>
    public class SelectCommandHandler : ServiceFinderCommandHandlerBase
    {
        private const string Example = "\nExample - select id, firstname, lastname where firstname = 'John' and/or lastname = 'Doe'";

        /// <summary>Initializes a new instance of the <see cref="SelectCommandHandler" /> class.</summary>
        /// <param name="fileCabinetService">The file cabinet service.</param>
        /// <param name="validator">Current validator.</param>
        public SelectCommandHandler(IFileCabinetService fileCabinetService, InputValidator validator)
            : base(validator, fileCabinetService)
        {
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
                Tuple<ConsoleTable, FileCabinetRecord[]> tuple;
                try
                {
                     tuple = this.ParseData(appCommandRequest.Parameters);
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Try again.");
                    return;
                }

                WriteRecords(tuple.Item1, tuple.Item2);

                return;
            }

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
            var columns = GetTableParams(tableParams);

            var table = new ConsoleTable(columns);
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
                throw new ArgumentException($"Invalid input. It must contain 'or' or 'and' between multiple searched parameters.{Example}");
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
                tableParams[i] = tableParams[i].ToUpper(CultureInfo.InvariantCulture).Trim() switch
                {
                    "FIRSTNAME" => "FirstName",
                    "LASTNAME" => "LastName",
                    "DATEOFBIRTH" => "DateOfBirth",
                    "ANNUALINCOME" => "AnnualIncome",
                    "WORKINGHOURS" => "WorkingHoursPerWeek",
                    "DRIVERCATEGORY" => "DriverCategory",
                    "ID" => "Id",
                    _ => throw new ArgumentException($"Invalid input - '{tableParams[i]}'. Parameters name can be:\n'FirstName', \n'LastName', \n'AnnualIncome', \n'DateOfBirth', \n'DriverCategory', \n'WorkingHours'{Example}")
                };
            }

            return tableParams;
        }

        private Tuple<ConsoleTable, FileCabinetRecord[]> ParseData(string parameters)
        {
            if (!parameters.Contains(" where ", StringComparison.CurrentCulture))
            {
                throw new ArgumentException($"Invalid input. It must contain ' where ' between table parameters and searched parameters.{Example}");
            }

            string[] parametersArray = parameters.Split(" where ", 2);
            const int searchedParamsIndex = 1;
            const int tableParamsIndex = 0;
            ConsoleTable table = CreateTable(parametersArray[tableParamsIndex]);
            FileCabinetRecord[] records = this.GetRecords(parametersArray[searchedParamsIndex]);

            return new Tuple<ConsoleTable, FileCabinetRecord[]>(table, records);
        }

        private FileCabinetRecord[] GetRecords(string parameters)
        {
            IEnumerable<FileCabinetRecord> result = new List<FileCabinetRecord>();
            Tuple<List<string>, List<string>, string> tuple = GetSearchedParams(parameters);
            (string[] searchKeys, string[] searchValues, string separator) = (tuple.Item1.ToArray(), tuple.Item2.ToArray(), tuple.Item3);

            for (int i = 0; i < searchKeys.Length; i++)
            {
                var current = this.GetRecordsBy(searchKeys[i], searchValues[i]);

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
    }
}
