using System;
using FileCabinetApp.Entities;
using FileCabinetApp.Validators.ValidationRule;

namespace FileCabinetApp.Validators.CustomValidators
{
    /// <summary>
    /// Validation Class.
    /// </summary>
    /// <seealso cref="FileCabinetApp.Validators.IRecordValidator" />
    public class CustomFirstNameValidator : IRecordValidator
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

            if (container.FirstName is null)
            {
                throw new ArgumentNullException(nameof(container), $"{container.FirstName}can not be null");
            }

            if (container.FirstName.Length < this.ValidationRules.MinLengthFirstName || container.FirstName.Length > this.ValidationRules.MaxLengthFirstName)
            {
                throw new ArgumentException($"First name must to have from {this.ValidationRules.MinLengthFirstName} to {this.ValidationRules.MaxLengthFirstName} characters.");
            }
        }
    }
}
