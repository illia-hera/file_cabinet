using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCabinetApp.Entities;
using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Class <c>RemoveCommandHandler</c> implement remove command.
    /// </summary>
    /// <seealso cref="CabinetServiceCommandHandlerBase" />
    internal class RemoveCommandHandler : CabinetServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">The file cabinet service.</param>
        public RemoveCommandHandler(IFileCabinetService fileCabinetService)
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

            if (appCommandRequest.Command.Equals("remove", StringComparison.OrdinalIgnoreCase))
            {
                if (!int.TryParse(appCommandRequest.Parameters, out var id))
                {
                    Console.WriteLine("Please, enter the id value in format \"edit n\", where n is an integer value greater than zero. Please try again.");
                    return;
                }

                try
                {
                    List<FileCabinetRecord> records = this.FileCabinetService.GetRecords().ToList();
                    if (!records.Exists(x => x.Id == id))
                    {
                        throw new ArgumentException($"{id} id record is not found.");
                    }

                    this.FileCabinetService.RemoveRecord(id);
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }

                Console.WriteLine($"Record #{id} is removed.");
                return;
            }

            base.Handle(appCommandRequest);
        }
    }
}
