using System;
using FileCabinetApp.Entities;

namespace FileCabinetApp.Validators.ParametersValidators
{
    /// <summary>
    /// Validation Class.
    /// </summary>
    /// <seealso cref="FileCabinetApp.Validators.IRecordValidator" />
    public class FirstNameValidator : IRecordValidator
    {
        private readonly int min;

        private readonly int max;

        /// <summary>
        /// Initializes a new instance of the <see cref="FirstNameValidator"/> class.
        /// </summary>
        /// <param name="max">The maximum.</param>
        /// <param name="min">The minimum.</param>
        public FirstNameValidator(int max, int min)
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

            if (container.FirstName is null)
            {
                throw new ArgumentNullException(nameof(container), $"{container.FirstName}can not be null");
            }

            if (container.FirstName.Length < this.min || container.FirstName.Length > this.max)
            {
                throw new ArgumentException($"First name must to have from {this.min} to {this.max} characters.");
            }
        }
    }
}
