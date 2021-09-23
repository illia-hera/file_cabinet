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
    public class DefaultFirstNameValidator : IRecordValidator
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
