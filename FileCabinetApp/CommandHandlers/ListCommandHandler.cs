using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCabinetApp.Entities;
using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Class <c>ListCommandHandler</c> implement List command.
    /// </summary>
    /// <seealso cref="CabinetServiceCommandHandlerBase" />
    public class ListCommandHandler : CabinetServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">The file cabinet service.</param>
        public ListCommandHandler(IFileCabinetService fileCabinetService)
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

            if (appCommandRequest.Command.Equals("list", StringComparison.OrdinalIgnoreCase))
            {
                IReadOnlyCollection<FileCabinetRecord> recordsCollection = this.FileCabinetService.GetRecords();
                if (recordsCollection.Count == 0)
                {
                    Console.WriteLine("No records yet.");
                    return;
                }

                foreach (FileCabinetRecord record in recordsCollection)
                {
                    Console.WriteLine($"#{record.Id}," +
                                      $" {record.FirstName}," +
                                      $" {record.LastName}," +
                                      $" {record.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.CreateSpecificCulture("en-US"))}");
                }

                return;
            }

            base.Handle(appCommandRequest);
        }
    }
}
