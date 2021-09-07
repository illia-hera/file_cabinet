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
        /// Validates the parameters.
        /// </summary>
        /// <param name="container">The container of parameters.</param>
        protected override void ValidateParameters(ParametersContainer container)
        {
            base.ValidateParameters(container);
        }
    }
}
