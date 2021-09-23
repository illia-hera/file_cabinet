using System;
using System.Collections.Generic;
using System.Globalization;
using FileCabinetApp.Entities;
using FileCabinetApp.Validators.ParametersValidators;

namespace FileCabinetApp.Validators
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

            new AnnualIncomeValidator(1500, 500).ValidateParameters(container);
            new WorkingHoursValidator(30, 20).ValidateParameters(container);
            new LastNameValidator(10, 5).ValidateParameters(container);
            new FirstNameValidator(10, 5).ValidateParameters(container);
            new DateOfBirthdayValidator(DateTime.Now, DateTime.Parse("10-Dec-1970", CultureInfo.CreateSpecificCulture("en-US"))).ValidateParameters(container);
            new DriverCategoryValidator(new List<char>() { 'A', 'B' }).ValidateParameters(container);
        }
    }
}
