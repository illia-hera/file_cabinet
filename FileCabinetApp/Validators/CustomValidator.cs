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
    /// <seealso cref="FileCabinetApp.Validators.CompositeValidator" />
    /// <seealso cref="IRecordValidator" />
    public class CustomValidator : CompositeValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomValidator"/> class.
        /// </summary>
        public CustomValidator()
            : base(new IRecordValidator[]
            {
                new AnnualIncomeValidator(1500, 500),
                new WorkingHoursValidator(30, 20),
                new LastNameValidator(10, 5),
                new FirstNameValidator(10, 5),
                new DateOfBirthdayValidator(
                    DateTime.Now,
                    DateTime.Parse("10-Dec-1970", CultureInfo.CreateSpecificCulture("en-US"))),
                new DriverCategoryValidator(new List<char>() { 'A', 'B' }),
            })
        {
        }
    }
}
