using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Class <c>CabinetServiceCommandHandlerBase</c>.
    /// </summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.CommandHandlerBase" />
    public class CabinetServiceCommandHandlerBase : CommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CabinetServiceCommandHandlerBase" /> class.
        /// </summary>
        /// <param name="fileCabinetService">The file cabinet service.</param>
        public CabinetServiceCommandHandlerBase(IFileCabinetService fileCabinetService)
        {
            this.FileCabinetService = fileCabinetService;
        }

        /// <summary>
        /// Gets the file cabinet service.
        /// </summary>
        /// <value>
        /// The file cabinet service.
        /// </value>
        protected IFileCabinetService FileCabinetService { get; }
    }
}
