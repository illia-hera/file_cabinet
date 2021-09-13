using FileCabinetApp.Validators;

namespace FileCabinetApp.Services
{
    /// <summary>
    /// Class <c>FileCabinetDefaultService</c> validate parameters by default method.
    /// </summary>
    /// <seealso cref="FileCabinetService" />
    public class FileCabinetDefaultService : FileCabinetService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetDefaultService"/> class.
        /// </summary>
        public FileCabinetDefaultService()
            : base(new DefaultValidator())
        {
        }
    }
}
