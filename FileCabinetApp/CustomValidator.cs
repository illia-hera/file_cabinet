using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Custom validator for person parameters.
    /// </summary>
    /// <seealso cref="FileCabinetApp.IRecordValidator" />
    public class CustomValidator : IRecordValidator
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
        /// The minimum date is {minDateOfBirth}, the maximum date is {maxDateOfBirth}.
        /// or
        /// The minimum hours is {minWorkingHoursPerWeek} hour, the maximum hours is the {maxWorkingHoursPerWeek}.
        /// or
        /// The Annual income min value - {minAnnualIncome}, max value - {maxAnnualIncome}.
        /// or
        /// The Driver License Category can be only - A, B.
        /// </exception>
        public void ValidateParameters(ParametersContainer container)
        {
            const int maxLengthFirstName = 10;
            const int minLengthFirstName = 5;
            const int maxLengthLastName = 10;
            const int minLengthLastName = 5;
            const int minWorkingHoursPerWeek = 20;
            const int maxWorkingHoursPerWeek = 30;
            const int minAnnualIncome = 500;
            const int maxAnnualIncome = 1500;
            const string minDateOfBirth = "20-Dec-1970";
            const string maxDateOfBirth = "20-Dec-2015";
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            if (container.FirstName is null)
            {
                throw new ArgumentNullException(nameof(container), $"{container.FirstName}can not be null");
            }

            if (container.FirstName.Length < minLengthFirstName || container.FirstName.Length > maxLengthFirstName)
            {
                throw new ArgumentException($"First name must to have from {minLengthFirstName} to {maxLengthFirstName} characters.");
            }

            if (container.LastName is null)
            {
                throw new ArgumentNullException(nameof(container), $"{container.LastName}can not be null");
            }

            if (container.LastName.Length < minLengthLastName || container.LastName.Length > maxLengthLastName)
            {
                throw new ArgumentException($"Last name must to have from {minLengthLastName} to {maxLengthLastName} characters.");
            }

            if (container.DateOfBirthday < DateTime.Parse(minDateOfBirth, CultureInfo.CurrentCulture) || container.DateOfBirthday > DateTime.Parse(maxDateOfBirth, CultureInfo.CurrentCulture))
            {
                throw new ArgumentException($"The minimum date is {minDateOfBirth}, the maximum date is {maxDateOfBirth}.");
            }

            if (container.WorkingHoursPerWeek > maxWorkingHoursPerWeek || container.WorkingHoursPerWeek < minWorkingHoursPerWeek)
            {
                throw new ArgumentException($"The minimum hours is {minWorkingHoursPerWeek} hour, the maximum hours is the {maxWorkingHoursPerWeek}.");
            }

            if (container.AnnualIncome < minAnnualIncome || container.AnnualIncome > maxAnnualIncome)
            {
                throw new ArgumentException($"The Annual income min value - {minAnnualIncome}, max value - {maxAnnualIncome}.");
            }

            var driverLicenseCategoryUpper = char.ToUpper(container.DriverLicenseCategory, CultureInfo.CurrentCulture);

            if (!(driverLicenseCategoryUpper == 'A' || driverLicenseCategoryUpper == 'B'))
            {
                throw new ArgumentException("The Driver License Category can be only - A, B.");
            }
        }
    }
}
