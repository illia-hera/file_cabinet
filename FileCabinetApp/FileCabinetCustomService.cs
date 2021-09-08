using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Class <c>FileCabinetCustomService</c> validate parameters by specified rules.
    /// </summary>
    /// <seealso cref="FileCabinetApp.FileCabinetService" />
    public class FileCabinetCustomService : FileCabinetService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetCustomService"/> class.
        /// </summary>
        public FileCabinetCustomService()
            : base(new CustomValidator())
        {
        }

        /// <summary>
        /// Creates the validator.
        /// </summary>
        /// <returns>
        /// Return validator.
        /// </returns>
        protected override IRecordValidator CreateValidator()
        {
            return new CustomValidator();
        }
    }
}
