using System;
using System.Collections.Generic;
using System.Globalization;
using FileCabinetApp.Entities;

namespace FileCabinetApp.Validators.RecordValidator.ParametersValidators
{
    /// <summary>
    /// Validation Class.
    /// </summary>
    /// <seealso cref="IRecordValidator" />
    public class DriverCategoryValidator : IRecordValidator
    {
        private readonly IList<char> actualCategories;

        /// <summary>
        /// Initializes a new instance of the <see cref="DriverCategoryValidator"/> class.
        /// </summary>
        /// <param name="actualCategories">The actual categories.</param>
        public DriverCategoryValidator(IList<char> actualCategories)
        {
            this.actualCategories = actualCategories;
        }

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

            if (!this.actualCategories.Contains(char.ToUpper(container.DriverLicenseCategory, CultureInfo.CurrentCulture)))
            {
                throw new ArgumentException($"The Driver License Category can be only - {this.actualCategories}");
            }
        }
    }
}
