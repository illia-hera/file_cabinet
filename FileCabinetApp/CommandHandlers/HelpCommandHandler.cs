using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Class <c>HelpCommandHandler</c> help command.
    /// </summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.CommandHandlerBase" />
    public class HelpCommandHandler : CommandHandlerBase
    {
        private const int CommandHelpIndex = 0;

        private const int DescriptionHelpIndex = 1;

        private const int ExplanationHelpIndex = 2;

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

            if (appCommandRequest.Command.Equals("help", StringComparison.OrdinalIgnoreCase))
            {
                if (!string.IsNullOrEmpty(appCommandRequest.Parameters))
                {
                    var index = Array.FindIndex(HelpMessages, 0, HelpMessages.Length, i => string.Equals(i[CommandHelpIndex], appCommandRequest.Parameters, StringComparison.OrdinalIgnoreCase));
                    if (index >= 0)
                    {
                        Console.WriteLine(HelpMessages[index][ExplanationHelpIndex]);
                    }
                    else
                    {
                        Console.WriteLine($"There is no explanation for '{appCommandRequest.Parameters}' command.");
                    }
                }
                else
                {
                    Console.WriteLine("Available commands:");

                    foreach (var helpMessage in HelpMessages)
                    {
                        Console.WriteLine("\t{0}\t- {1}", helpMessage[CommandHelpIndex], helpMessage[DescriptionHelpIndex]);
                    }
                }

                Console.WriteLine();
                return;
            }

            base.Handle(appCommandRequest);
        }
    }
}
