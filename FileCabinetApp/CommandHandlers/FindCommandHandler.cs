using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCabinetApp.Entities;
using FileCabinetApp.Printers;
using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Class <c>FindCommandHandler</c> implement find command.
    /// </summary>
    /// <seealso cref="CabinetServiceCommandHandlerBase" />
    internal class FindCommandHandler : CabinetServiceCommandHandlerBase
    {
        private readonly Action<IEnumerable<FileCabinetRecord>> print;

        /// <summary>
        /// Initializes a new instance of the <see cref="FindCommandHandler" /> class.
        /// </summary>
        /// <param name="fileCabinetService">The file cabinet service.</param>
        /// <param name="print">The print.</param>
        public FindCommandHandler(IFileCabinetService fileCabinetService, Action<IEnumerable<FileCabinetRecord>> print)
            : base(fileCabinetService)
        {
            this.print = print;
        }

        /// <summary>
        /// Handles the specified application command request.
        /// </summary>
        /// <param name="appCommandRequest">The application command request.</param>
        /// <exception cref="ArgumentNullException">nameof(appCommandRequest).</exception>
        public override void Handle(AppCommandRequest appCommandRequest)
        {
            if (appCommandRequest == null)
            {
                throw new ArgumentNullException(nameof(appCommandRequest));
            }

            if (appCommandRequest.Command.Equals("find", StringComparison.OrdinalIgnoreCase))
            {
                string[] inputs = appCommandRequest.Parameters.Split(' ', 2);
                const int parameterIndex = 0;
                const int valueIndex = 1;
                string parameter = inputs[parameterIndex];
                string value = inputs.Length > 1 ? inputs[valueIndex] : string.Empty;

                IEnumerable<FileCabinetRecord> records = parameter switch
                {
                    var p when p.Equals("firstname", StringComparison.OrdinalIgnoreCase) => this.FileCabinetService.FindByFirstName(value.Trim('\"')),
                    var p when p.Equals("lastName", StringComparison.OrdinalIgnoreCase) => this.FileCabinetService.FindByLastName(value.Trim('\"')),
                    var p when p.Equals("dateOfBirth", StringComparison.OrdinalIgnoreCase)
                               && DateTime.TryParse(value.Trim('\"'), out DateTime dateOfBd) => this.FileCabinetService.FindByDateOfBirthday(dateOfBd),
                    _ => null
                };

                if (records is null)
                {
                    Console.WriteLine($"No records with {parameter} - {value}.");
                    return;
                }

                this.print(records);

                return;
            }

            base.Handle(appCommandRequest);
        }
    }
}
