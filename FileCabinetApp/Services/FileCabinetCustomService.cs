using FileCabinetApp.Validators;

namespace FileCabinetApp.Services
{
    /// <summary>
    /// Class <c>FileCabinetCustomService</c> validate parameters by specified rules.
    /// </summary>
    /// <seealso cref="FileCabinetService" />
    public class FileCabinetCustomService : FileCabinetService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetCustomService"/> class.
        /// </summary>
        public FileCabinetCustomService()
            : base(new CustomValidator())
        {
        }
    }
}
