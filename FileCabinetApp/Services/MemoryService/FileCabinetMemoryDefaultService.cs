using FileCabinetApp.Validators;
using FileCabinetApp.Validators.RecordValidator;

namespace FileCabinetApp.Services.MemoryService
{
    /// <summary>
    /// Class <c>FileCabinetMemoryDefaultService</c> validate parameters by default method.
    /// </summary>
    /// <seealso cref="FileCabinetMemoryService" />
    public class FileCabinetMemoryDefaultService : FileCabinetMemoryService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetMemoryDefaultService"/> class.
        /// </summary>
        public FileCabinetMemoryDefaultService()
            : base(new DefaultValidator())
        {
        }
    }
}
