using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// CLass <c>StatCommandHandler</c> generate File Cabinet Stat.
    /// </summary>
    /// <seealso cref="CabinetServiceCommandHandlerBase" />
    public class StatCommandHandler : CabinetServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">The file cabinet service.</param>
        public StatCommandHandler(IFileCabinetService fileCabinetService)
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

            if (appCommandRequest.Command.Equals("stat", StringComparison.OrdinalIgnoreCase))
            {
                Tuple<int, int> recordsCount = this.FileCabinetService.GetStat();
                Console.WriteLine($"{recordsCount.Item1} record(s), {recordsCount.Item2} record(s) deleted.");
                return;
            }

            base.Handle(appCommandRequest);
        }
    }
}
