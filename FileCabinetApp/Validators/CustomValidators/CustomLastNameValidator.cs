using System;
using FileCabinetApp.Entities;
using FileCabinetApp.Validators.ValidationRule;

namespace FileCabinetApp.Validators.CustomValidators
{
    /// <summary>
    /// Validation Class.
    /// </summary>
    /// <seealso cref="FileCabinetApp.Validators.IRecordValidator" />
    public class CustomLastNameValidator : IRecordValidator
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

            if (container.LastName is null)
            {
                throw new ArgumentNullException(nameof(container), $"{container.LastName}can not be null");
            }

            if (container.LastName.Length < this.ValidationRules.MinLengthLastName ||
                container.LastName.Length > this.ValidationRules.MaxLengthLastName)
            {
                throw new ArgumentException(
                    $"Last name must to have from {this.ValidationRules.MinLengthLastName} to {this.ValidationRules.MaxLengthLastName} characters.");
            }
        }
    }
}
