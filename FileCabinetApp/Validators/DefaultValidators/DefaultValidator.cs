using System;
using FileCabinetApp.Entities;
using FileCabinetApp.Validators.CustomValidators;

namespace FileCabinetApp.Validators.DefaultValidators
{
    /// <summary>
    /// Default validator for person parameters.
    /// </summary>
    /// <seealso cref="IRecordValidator" />
    public class DefaultValidator : IRecordValidator
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
