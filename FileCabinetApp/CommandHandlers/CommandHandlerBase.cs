using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Class <c>CommandHandlerBase</c> base of command handling.
    /// </summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.ICommandHandler" />
    public class CommandHandlerBase : ICommandHandler
    {
        private readonly string[] commands = new[]
        {
            "help",
            "exit",
            "stat",
            "create",
            "export",
            "import",
            "delete",
            "purge",
            "update",
            "select",
        };

        private ICommandHandler nextHandler;

        /// <summary>
        /// Handles the specified application command request.
        /// </summary>
        /// <param name="appCommandRequest">The application command request.</param>
        /// <exception cref="System.ArgumentNullException">appCommandRequest.</exception>
        public virtual void Handle(AppCommandRequest appCommandRequest)
        {
            if (appCommandRequest == null)
            {
                throw new ArgumentNullException(nameof(appCommandRequest));
            }

            if (this.nextHandler == null)
            {
                Console.WriteLine($"There is no '{appCommandRequest.Command}' command.");

                var similar = this.CompareCommand(appCommandRequest.Command);

                if (!string.IsNullOrWhiteSpace(similar))
                {
                    Console.WriteLine(similar);
                }
            }
            else
            {
                this.nextHandler.Handle(appCommandRequest);
            }
        }

        /// <summary>
        /// Sets the next.
        /// </summary>
        /// <param name="commandHandler">The command handler.</param>
        /// <returns>Return new CommandHandle.</returns>
        public ICommandHandler SetNext(ICommandHandler commandHandler)
        {
            this.nextHandler = commandHandler;
            return commandHandler;
        }

        private static double Minimum(double a, double b) => a < b ? a : b;

        private static double Minimum(double a, double b, double c) => (a = a < b ? a : b) < c ? a : c;

        private static double DamerauLevenshteinDistance(string firstText, string secondText)
        {
            var n = firstText.Length + 1;
            var m = secondText.Length + 1;
            double[,] arrayD = new double[n, m];

            for (var i = 0; i < n; i++)
            {
                arrayD[i, 0] = i;
            }

            for (var j = 0; j < m; j++)
            {
                arrayD[0, j] = j;
            }

            for (var i = 1; i < n; i++)
            {
                for (var j = 1; j < m; j++)
                {
                    var cost = firstText[i - 1] == secondText[j - 1] ? 0 : 1;

                    arrayD[i, j] = Minimum(
                        arrayD[i - 1, j] + 1,          // удаление
                        arrayD[i, j - 1] + 1,         // вставка
                        arrayD[i - 1, j - 1] + cost); // замена

                    if (i > 1 && j > 1
                              && firstText[i - 1] == secondText[j - 2]
                              && firstText[i - 2] == secondText[j - 1])
                    {
                        arrayD[i, j] = Minimum(
                            arrayD[i, j],
                            arrayD[i - 2, j - 2] + cost); // перестановка
                    }
                }
            }

            return arrayD[n - 1, m - 1];
        }

        private static void ReplaceArrays(string currentStr, double comparingValue, string[] resultsStrings, double[] distanceValues)
        {
            if (distanceValues[0] > comparingValue)
            {
                distanceValues[2] = distanceValues[1];
                resultsStrings[2] = resultsStrings[1];
                distanceValues[1] = distanceValues[0];
                resultsStrings[1] = resultsStrings[0];
                distanceValues[0] = comparingValue;
                resultsStrings[0] = currentStr;
            }
            else if (distanceValues[1] > comparingValue)
            {
                distanceValues[2] = distanceValues[1];
                resultsStrings[2] = resultsStrings[1];
                distanceValues[1] = comparingValue;
                resultsStrings[1] = currentStr;
            }
            else if (distanceValues[2] > comparingValue)
            {
                distanceValues[2] = comparingValue;
                resultsStrings[2] = currentStr;
            }
        }

        private string CompareCommand(string command)
        {
            string[] resultsStrings = new string[3];
            double[] distanceValues = { double.MaxValue,  double.MaxValue, double.MaxValue };
            foreach (var s in this.commands)
            {
                var comparingValue = DamerauLevenshteinDistance(command, s);
                ReplaceArrays(s, comparingValue, resultsStrings, distanceValues);
            }

            var sb = new StringBuilder();
            sb.Append("The most similar commands are:\n");
            foreach (var resultsString in resultsStrings)
            {
                sb.AppendLine($"{new string(' ', 10)}-{resultsString}");
            }

            return sb.ToString();
        }
    }
}
