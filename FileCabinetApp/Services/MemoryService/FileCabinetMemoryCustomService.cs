using FileCabinetApp.Validators;

namespace FileCabinetApp.Services.MemoryService
{
    /// <summary>
    /// Class <c>FileCabinetMemoryCustomService</c> validate parameters by specified rules.
    /// </summary>
    /// <seealso cref="FileCabinetMemoryService" />
    public class FileCabinetMemoryCustomService : FileCabinetMemoryService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetMemoryCustomService"/> class.
        /// </summary>
        public FileCabinetMemoryCustomService()
            : base(new CustomValidator())
        {
        }
    }
}
