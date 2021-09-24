using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCabinetApp.Entities;
using FileCabinetApp.Services;
using FileCabinetApp.Validators;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Class <c>EditCommandHandler</c> implement edit command.
    /// </summary>
    /// <seealso cref="CabinetServiceCommandHandlerBase" />
    public class EditCommandHandler : CabinetServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">The file cabinet service.</param>
        public EditCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
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

            if (appCommandRequest.Command.Equals("edit", StringComparison.OrdinalIgnoreCase))
            {
                bool isParsedId = int.TryParse(appCommandRequest.Parameters, out int id);

                IReadOnlyCollection<FileCabinetRecord> recordsCollection = this.FileCabinetService.GetRecords();
                var record = recordsCollection.FirstOrDefault(r => r.Id == id);

                if (!isParsedId || record is null)
                {
                    Console.WriteLine($"#{id} record is not found.");
                    return;
                }

                ParametersContainer container = Program.GetValidInputParameters();

                this.FileCabinetService.EditRecord(id, container);
                Console.WriteLine($"Record #{id} is updated.");

                return;
            }

            base.Handle(appCommandRequest);
        }
    }
}
