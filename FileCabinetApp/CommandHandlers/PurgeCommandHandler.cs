using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Class <c>PurgeCommandHandler</c> implement purge command.
    /// </summary>
    /// <seealso cref="CabinetServiceCommandHandlerBase" />
    internal class PurgeCommandHandler : CabinetServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PurgeCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">The file cabinet service.</param>
        public PurgeCommandHandler(IFileCabinetService fileCabinetService)
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

            if (appCommandRequest.Command.Equals("purge", StringComparison.OrdinalIgnoreCase))
            {
                var items = this.FileCabinetService.GetStat();
                var deleted = this.FileCabinetService.Purge();
                Console.WriteLine($"Data file processing is completed: {deleted} of {items.Item1} records were purged.");

                return;
            }

            base.Handle(appCommandRequest);
        }
    }
}
