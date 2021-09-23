using System;
using System.Globalization;
using FileCabinetApp.Entities;
using FileCabinetApp.Validators.ValidationRule;

namespace FileCabinetApp.Validators.CustomValidators
{
    /// <summary>
    /// Validation Class.
    /// </summary>
    /// <seealso cref="FileCabinetApp.Validators.IRecordValidator" />
    public class CustomDriverCategoryValidator : IRecordValidator
    {
        private ValidationRules ValidationRules { get; } = new ValidationCustomRules();

        /// <summary>
        /// Validates the parameters.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <exception cref="System.ArgumentException">The Driver License Category can be only - {this.ValidationRules.ActualCategories}.</exception>
        public void ValidateParameters(ParametersContainer container)
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            if (!this.ValidationRules.ActualCategories.Contains(char.ToUpper(container.DriverLicenseCategory, CultureInfo.CurrentCulture)))
            {
                throw new ArgumentException($"The Driver License Category can be only - {this.ValidationRules.ActualCategories}");
            }
        }
    }
}
