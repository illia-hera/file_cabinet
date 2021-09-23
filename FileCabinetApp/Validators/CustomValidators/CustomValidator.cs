using System;
using FileCabinetApp.Entities;

namespace FileCabinetApp.Validators.CustomValidators
{
    /// <summary>
    /// Custom validator for person parameters.
    /// </summary>
    /// <seealso cref="IRecordValidator" />
    public class CustomValidator : IRecordValidator
    {
        /// <summary>
        /// Validates the parameters.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <exception cref="System.ArgumentNullException">container.</exception>
        public void ValidateParameters(ParametersContainer container)
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            new CustomAnnualIncomeValidator().ValidateParameters(container);
            new CustomWorkingHoursValidator().ValidateParameters(container);
            new CustomLastNameValidator().ValidateParameters(container);
            new CustomFirstNameValidator().ValidateParameters(container);
            new CustomDateOfBirthdayValidator().ValidateParameters(container);
            new CustomDriverCategoryValidator().ValidateParameters(container);
        }
    }
}
