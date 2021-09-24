using System;
using FileCabinetApp.Entities;

namespace FileCabinetApp.Validators.RecordValidator.ParametersValidators
{
    /// <summary>
    /// Validation Class.
    /// </summary>
    /// <seealso cref="IRecordValidator" />
    public class DateOfBirthdayValidator : IRecordValidator
    {
        private readonly DateTime min;

        private readonly DateTime max;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateOfBirthdayValidator"/> class.
        /// </summary>
        /// <param name="max">The maximum.</param>
        /// <param name="min">The minimum.</param>
        public DateOfBirthdayValidator(DateTime max, DateTime min)
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

            if (container.DateOfBirthday < this.min || container.DateOfBirthday > this.max)
            {
                throw new ArgumentException($"The minimum date is {this.min}, the maximum date is {this.max}.");
            }
        }
    }
}
