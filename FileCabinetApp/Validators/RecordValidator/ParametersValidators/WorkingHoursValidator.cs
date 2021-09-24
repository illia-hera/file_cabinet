using System;
using FileCabinetApp.Entities;

namespace FileCabinetApp.Validators.RecordValidator.ParametersValidators
{
    /// <summary>
    /// Validation Class.
    /// </summary>
    /// <seealso cref="IRecordValidator" />
    public class WorkingHoursValidator : IRecordValidator
    {
        private readonly short min;

        private readonly short max;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkingHoursValidator"/> class.
        /// </summary>
        /// <param name="max">The maximum.</param>
        /// <param name="min">The minimum.</param>
        public WorkingHoursValidator(short max, short min)
        {
            this.max = max;
            this.min = min;
        }

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

            if (container.WorkingHoursPerWeek > this.max || container.WorkingHoursPerWeek < this.min)
            {
                throw new ArgumentException($"The minimum hours is {this.min} hour, the maximum hours is the {this.max}.");
            }
        }
    }
}
