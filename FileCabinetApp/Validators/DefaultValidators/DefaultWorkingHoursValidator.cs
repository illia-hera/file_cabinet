using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCabinetApp.Entities;
using FileCabinetApp.Validators.ValidationRule;

namespace FileCabinetApp.Validators.DefaultValidators
{
    /// <summary>
    /// Validation Class.
    /// </summary>
    /// <seealso cref="FileCabinetApp.Validators.IRecordValidator" />
    public class DefaultWorkingHoursValidator : IRecordValidator
    {
        private ValidationRules ValidationRules { get; } = new ValidationDefaultRules();

        /// <summary>
        /// Validates the parameters.
        /// </summary>
        /// <param name="container">The container.</param>
        public void ValidateParameters(ParametersContainer container)
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            if (container.WorkingHoursPerWeek > this.ValidationRules.MaxWorkingHoursPerWeek || container.WorkingHoursPerWeek < this.ValidationRules.MinWorkingHoursPerWeek)
            {
                throw new ArgumentException($"The minimum hours is {this.ValidationRules.MinWorkingHoursPerWeek} hour, the maximum hours is the {this.ValidationRules.MaxWorkingHoursPerWeek}.");
            }
        }
    }
}
