using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCabinetApp.Entities;
using FileCabinetApp.Services;
using FileCabinetApp.Services.MemoryService;
using FileCabinetApp.Validators;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Class <c>CreateCommandHandler</c> create records.
    /// </summary>
    /// <seealso cref="CabinetServiceCommandHandlerBase" />
    public class CreateCommandHandler : CabinetServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">The file cabinet service.</param>
        public CreateCommandHandler(IFileCabinetService fileCabinetService)
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

            if (appCommandRequest.Command.Equals("create", StringComparison.OrdinalIgnoreCase))
            {
                ParametersContainer container = Program.GetValidInputParameters();
                int recordId = 0;

                try
                {
                    recordId = this.FileCabinetService.CreateRecord(container);
                }
                catch (Exception e) when (e is ArgumentException)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Please, try again.");
                }

                Console.WriteLine($"Record #{recordId} is created.");
                return;
            }

            base.Handle(appCommandRequest);
        }
    }
}
