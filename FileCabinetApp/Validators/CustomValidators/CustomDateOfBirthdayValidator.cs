using System;
using FileCabinetApp.Entities;
using FileCabinetApp.Validators.ValidationRule;

namespace FileCabinetApp.Validators.CustomValidators
{
    /// <summary>
    /// Validation Class.
    /// </summary>
    /// <seealso cref="FileCabinetApp.Validators.IRecordValidator" />
    public class CustomDateOfBirthdayValidator : IRecordValidator
    {
        private ValidationRules ValidationRules { get; } = new ValidationCustomRules();

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

            if (container.DateOfBirthday < this.ValidationRules.MinDateOfBirth || container.DateOfBirthday > this.ValidationRules.MaxDateOfBirth)
            {
                throw new ArgumentException($"The minimum date is {this.ValidationRules.MinDateOfBirth}, the maximum date is {this.ValidationRules.MaxDateOfBirth}.");
            }
        }
    }
}
