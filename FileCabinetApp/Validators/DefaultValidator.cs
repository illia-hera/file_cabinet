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
    /// <seealso cref="FileCabinetApp.Validators.CompositeValidator" />
    /// <seealso cref="IRecordValidator" />
    public class DefaultValidator : CompositeValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultValidator"/> class.
        /// </summary>
        public DefaultValidator()
            : base(new IRecordValidator[]
            {
                new AnnualIncomeValidator(1000000, 1000),
                new WorkingHoursValidator(40, 1),
                new LastNameValidator(60, 2),
                new FirstNameValidator(60, 2),
                new DateOfBirthdayValidator(DateTime.Now, DateTime.Parse("01-Jun-1950", CultureInfo.CreateSpecificCulture("en-US"))),
                new DriverCategoryValidator(new List<char>() { 'A', 'B', 'C', 'D' }),
            })
        {
        }
    }
}
