using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileCabinetApp.Entities;
using FileCabinetApp.Readers;
using FileCabinetApp.Services;
using FileCabinetApp.Utility;
using FileCabinetApp.Validators.InputValidators;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Delete command implementation.
    /// </summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.CabinetServiceCommandHandlerBase" />
    public class DeleteCommandHandler : ServiceFinderCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCommandHandler" /> class.
        /// </summary>
        /// <param name="fileCabinetService">The file cabinet service.</param>
        /// <param name="validator">The validator.</param>
        public DeleteCommandHandler(IFileCabinetService fileCabinetService, InputValidator validator)
            : base(validator, fileCabinetService)
        {
        }

        /// <summary>
        ///     Handles the specified application command request.
        /// </summary>
        /// <param name="appCommandRequest">The application command request.</param>
        public override void Handle(AppCommandRequest appCommandRequest)
        {
            if (appCommandRequest == null)
            {
                throw new ArgumentNullException(nameof(appCommandRequest));
            }

            if (appCommandRequest.Command.Equals("delete", StringComparison.OrdinalIgnoreCase))
            {
                StringBuilder sb = new StringBuilder();
                IEnumerable<FileCabinetRecord> records;

                try
                {
                    records = this.ParseData(appCommandRequest.Parameters);
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }

                foreach (var fileCabinetRecord in records.ToList())
                {
                    this.FileCabinetService.RemoveRecord(fileCabinetRecord.Id);
                    sb.Append($"#{fileCabinetRecord.Id}, ");
                }

                Console.WriteLine(records.Any() ? "Example: delete where id = '1'" : $"Records {sb}are deleted");

                return;
            }

            base.Handle(appCommandRequest);
        }

        private IEnumerable<FileCabinetRecord> ParseData(string parameters)
        {
            string inputParameterString = parameters.Replace("where ", string.Empty, StringComparison.InvariantCultureIgnoreCase);
            string[] parameterStrings = inputParameterString.Replace("'", string.Empty, StringComparison.InvariantCultureIgnoreCase).Split('=', StringSplitOptions.RemoveEmptyEntries);

            if (parameterStrings.Length % 2 != 0)
            {
                throw new ArgumentException("the input string must be in the following format: delete where field1='value1'");
            }

            IEnumerable<FileCabinetRecord> records = new List<FileCabinetRecord>();

            for (int i = 0, j = 0; i < parameterStrings.Length; i += 2, j++)
            {
                string key = parameterStrings[i];
                string value = parameterStrings[i + 1];
                records = records.Union(this.GetRecordsBy(key, value));
            }

            return records;
        }
    }
}