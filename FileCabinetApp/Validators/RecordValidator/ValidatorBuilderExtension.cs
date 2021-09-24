using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validators.RecordValidator
{
    /// <summary>
    /// Class <c>ValidatorBuilderExtension</c>.
    /// </summary>
    public static class ValidatorBuilderExtension
    {
        /// <summary>Creates the custom.</summary>
        /// <param name="builder">The builder.</param>
        /// <returns><see cref="ValidatorBuilder"/>.</returns>
        /// <exception cref="ArgumentNullException">builder.</exception>
        public static IRecordValidator CreateCustom(this ValidatorBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.ValidateFirstName(10, 5)
                .ValidateLastName(10, 5)
                .ValidateDateOfBirth(new DateTime(1970, 12, 10).Date, DateTime.Now.Date)
                .ValidateWorkingHours(30, 20)
                .ValidateAnnualIncome(1500, 500)
                .ValidateDriverCategory(new List<char>() { 'A', 'B' })
                .Create();
        }

        /// <summary>Creates the default.</summary>
        /// <param name="builder">The builder.</param>
        /// <returns><see cref="ValidatorBuilder"/>.</returns>
        /// <exception cref="ArgumentNullException">builder.</exception>
        public static IRecordValidator CreateDefault(this ValidatorBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.ValidateFirstName(60, 2)
                .ValidateLastName(60, 2)
                .ValidateDateOfBirth(DateTime.Now.Date, new DateTime(1950, 1, 1).Date)
                .ValidateWorkingHours(40, 1)
                .ValidateAnnualIncome(1000_000, 1000)
                .ValidateDriverCategory(new List<char>() { 'A', 'B', 'C', 'D' })
                .Create();
        }
    }
}
