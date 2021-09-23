using System;
using System.Collections.Generic;
using System.Globalization;
using FileCabinetApp.Entities;
using FileCabinetApp.Validators.ParametersValidators;

namespace FileCabinetApp.Validators
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

            new AnnualIncomeValidator(1000000, 1000).ValidateParameters(container);
            new WorkingHoursValidator(40, 1).ValidateParameters(container);
            new LastNameValidator(60, 2).ValidateParameters(container);
            new FirstNameValidator(60, 2).ValidateParameters(container);
            new DateOfBirthdayValidator(DateTime.Now, DateTime.Parse("01-Jun-1950", CultureInfo.CreateSpecificCulture("en-US"))).ValidateParameters(container);
            new DriverCategoryValidator(new List<char>() { 'A', 'B', 'C', 'D' }).ValidateParameters(container);
        }
    }
}
