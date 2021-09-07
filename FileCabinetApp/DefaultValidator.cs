using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Default validator for person parameters.
    /// </summary>
    /// <seealso cref="FileCabinetApp.IRecordValidator" />
    public class DefaultValidator : IRecordValidator
    {
        /// <summary>
        /// Validates the parameters.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <exception cref="System.ArgumentNullException">
        /// container
        /// or
        /// container
        /// or
        /// container.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// First name must to have from {minLengthFirstName} to {maxLengthFirstName} characters.
        /// or
        /// Last name must to have from {minLengthLastName} to {maxLengthLastName} characters.
        /// or
        /// The minimum date is {minDateOfBirth}, the maximum date is the current one.
        /// or
        /// The minimum hours is {minWorkingHoursPerWeek} hour, the maximum hours is the {maxWorkingHoursPerWeek}.
        /// or
        /// The Annual income must be bigger than {minAnnualIncome}.
        /// or
        /// The Driver License Category can be only - A, B, C, D.
        /// </exception>
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

            const int maxLengthFirstName = 60;
            const int minLengthFirstName = 2;
            if (container.FirstName.Length < minLengthFirstName || container.FirstName.Length > maxLengthFirstName)
            {
                throw new ArgumentException($"First name must to have from {minLengthFirstName} to {maxLengthFirstName} characters.");
            }

            if (container.LastName is null)
            {
                throw new ArgumentNullException(nameof(container), $"{container.LastName}can not be null");
            }

            const int maxLengthLastName = 60;
            const int minLengthLastName = 2;
            if (container.LastName.Length < minLengthLastName || container.LastName.Length > maxLengthLastName)
            {
                throw new ArgumentException($"Last name must to have from {minLengthLastName} to {maxLengthLastName} characters.");
            }

            DateTime minDateOfBirth = DateTime.Parse("01-Jan-1950", CultureInfo.CurrentCulture);
            DateTime maxDateOfBirth = DateTime.Now;
            if (container.DateOfBirthday < minDateOfBirth || container.DateOfBirthday > maxDateOfBirth)
            {
                throw new ArgumentException($"The minimum date is {minDateOfBirth}, the maximum date is the current one.");
            }

            const int minWorkingHoursPerWeek = 0;
            const int maxWorkingHoursPerWeek = 40;
            if (container.WorkingHoursPerWeek > maxWorkingHoursPerWeek || container.WorkingHoursPerWeek < minWorkingHoursPerWeek)
            {
                throw new ArgumentException("The minimum hours is {minWorkingHoursPerWeek} hour, the maximum hours is the {maxWorkingHoursPerWeek}.");
            }

            const int minAnnualIncome = 1000;
            if (container.AnnualIncome < minAnnualIncome)
            {
                throw new ArgumentException($"The Annual income must be bigger than {minAnnualIncome}.");
            }

            var driverLicenseCategoryUpper = char.ToUpper(container.DriverLicenseCategory, CultureInfo.CurrentCulture);

            if (!(driverLicenseCategoryUpper == 'A' || driverLicenseCategoryUpper == 'B' || driverLicenseCategoryUpper == 'C' || driverLicenseCategoryUpper == 'D'))
            {
                throw new ArgumentException("The Driver License Category can be only - A, B, C, D.");
            }
        }
    }
}
