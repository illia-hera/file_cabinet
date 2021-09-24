using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using FileCabinetApp.Services;
using FileCabinetApp.Services.SnapshotServices;
using FileCabinetApp.Validators;
using FileCabinetApp.Validators.InputValidator;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Class <c>ExportCommandHandler</c> implement export command.
    /// </summary>
    /// <seealso cref="CabinetServiceCommandHandlerBase" />
    internal class ExportCommandHandler : CabinetServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExportCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">The file cabinet service.</param>
        public ExportCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

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

            if (appCommandRequest.Command.Equals("export", StringComparison.OrdinalIgnoreCase))
            {
                Tuple<bool, string, string> parametersTuple = InputValidator.ValidateImportExportParameters(appCommandRequest.Parameters);
                if (File.Exists(parametersTuple.Item2))
                {
                    Console.Write($"File is exist - rewrite {parametersTuple.Item2}? [Y/n] ");
                    string answer = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(answer) || !answer.Equals("y", StringComparison.OrdinalIgnoreCase))
                    {
                        return;
                    }
                }

                if (this.TryExportRecords(parametersTuple))
                {
                    Console.WriteLine($"All records are exported to file {parametersTuple.Item2!.Split('\\')[^1]}");
                }

                return;
            }

            base.Handle(appCommandRequest);
        }

        private bool TryExportRecords(Tuple<bool, string, string> parametersTuple)
        {
            IFileCabinetServiceSnapshot snapshot = this.FileCabinetService.MakeSnapshot();
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
                return false;
            }

            return true;
        }
    }
}
