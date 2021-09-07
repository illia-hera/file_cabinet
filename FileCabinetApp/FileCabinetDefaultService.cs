using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Class <c>FileCabinetDefaultService</c> validate parameters by default method.
    /// </summary>
    /// <seealso cref="FileCabinetApp.FileCabinetService" />
    public class FileCabinetDefaultService : FileCabinetService
    {
        /// <summary>
        /// Creates the validator.
        /// </summary>
        /// <returns>
        /// Return validator.
        /// </returns>
        protected override IRecordValidator CreateValidator()
        {
            return base.CreateValidator();
        }
    }
}
