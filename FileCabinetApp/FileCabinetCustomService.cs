using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Class <c>FileCabinetCustomService</c> validate parameters by specified rules.
    /// </summary>
    /// <seealso cref="FileCabinetApp.FileCabinetService" />
    public class FileCabinetCustomService : FileCabinetService
    {
        /// <summary>
        /// Validates the parameters.
        /// </summary>
        /// <param name="container">The container of parameters.</param>
        protected override void ValidateParameters(ParametersContainer container)
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
                throw new ArgumentException("First name must to have from 5 to 10 characters.");
            }

            if (container.LastName is null)
            {
                throw new ArgumentNullException(nameof(container), $"{container.LastName}can not be null");
            }

            if (container.LastName.Length < minLengthLastName || container.LastName.Length > maxLengthLastName)
            {
                throw new ArgumentException("Last name must to have from 5 to 10 characters.");
            }

            if (container.DateOfBirthday < DateTime.Parse(minDateOfBirth, CultureInfo.CurrentCulture) || container.DateOfBirthday > DateTime.Parse(maxDateOfBirth, CultureInfo.CurrentCulture))
            {
                throw new ArgumentException($"The minimum date is {minDateOfBirth}, the maximum date is {maxDateOfBirth}.");
            }

            if (container.WorkingHoursPerWeek > maxWorkingHoursPerWeek || container.WorkingHoursPerWeek < minWorkingHoursPerWeek)
            {
                throw new ArgumentException("The minimum hours is 20 hour, the maximum hours is the 30.");
            }

            if (container.AnnualIncome < minAnnualIncome || container.AnnualIncome > maxAnnualIncome)
            {
                throw new ArgumentException("The Annual income min value - 500, max value - 1500.");
            }

            var driverLicenseCategoryUpper = char.ToUpper(container.DriverLicenseCategory, CultureInfo.CurrentCulture);

            if (!(driverLicenseCategoryUpper == 'A' || driverLicenseCategoryUpper == 'B'))
            {
                throw new ArgumentException("The Driver License Category can be only - A, B.");
            }
        }
    }
}
