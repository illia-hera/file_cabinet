using System;
using FileCabinetApp.Entities;

namespace FileCabinetApp.Validators.RecordValidator.ParametersValidators
{
    /// <summary>
    /// Validation Class.
    /// </summary>
    /// <seealso cref="IRecordValidator" />
    public class LastNameValidator : IRecordValidator
    {
        private readonly int min;

        private readonly int max;

        /// <summary>
        /// Initializes a new instance of the <see cref="LastNameValidator"/> class.
        /// </summary>
        /// <param name="max">The maximum.</param>
        /// <param name="min">The minimum.</param>
        public LastNameValidator(int max, int min)
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

            if (container.LastName is null)
            {
                throw new ArgumentNullException(nameof(container), $"{container.LastName}can not be null");
            }

            if (container.LastName.Length < this.min ||
                container.LastName.Length > this.max)
            {
                throw new ArgumentException(
                    $"Last name must to have from {this.min} to {this.max} characters.");
            }
        }
    }
}
