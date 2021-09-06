using System;
using System.Globalization;
using System.Reflection.Metadata;

namespace FileCabinetApp
{
    /// <summary>
    /// Class <c>ParametersContainer</c> parsed and validate string values.
    /// </summary>
    public class ParametersContainer
    {
        private const int MinAnnualIncome = 1000;
        private const int MaxLengthLastName = 60;
        private const int MinLengthLastName = 2;
        private const int MaxLengthFirstName = 60;
        private const int MinLengthFirstName = 2;
        private const int MinWorkingHoursPerWeek = 0;
        private const int MaxWorkingHoursPerWeek = 40;
        private static readonly DateTime MinDateOfBirth = DateTime.Parse("01-Jan-1950", CultureInfo.CurrentCulture);
        private static readonly DateTime MaxDateOfBirth = DateTime.Now;

        /// <summary>
        /// Gets the date of birthday.
        /// </summary>
        /// <value>
        /// The date of birthday.
        /// </value>
        public DateTime DateOfBirthday { get; private set; }

        /// <summary>
        /// Gets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName { get; private set; }

        /// <summary>
        /// Gets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; private set; }

        /// <summary>
        /// Gets the working hours per week.
        /// </summary>
        /// <value>
        /// The working hours per week.
        /// </value>
        public short WorkingHoursPerWeek { get; private set; }

        /// <summary>
        /// Gets the annual income.
        /// </summary>
        /// <value>
        /// The annual income.
        /// </value>
        public decimal AnnualIncome { get; private set; }

        /// <summary>
        /// Gets the driver license category.
        /// </summary>
        /// <value>
        /// The driver license category.
        /// </value>
        public char DriverLicenseCategory { get; private set; }

        /// <summary>
        /// Tries the get valid firstName date of birth day.
        /// </summary>
        /// <param name="value">The string value date of birthday.</param>
        /// <returns>Return validation result.</returns>
        public bool TrySetDateTimeOfBd(string value)
        {
            string format = "MM/dd/yyyy";
            CultureInfo formatProvider = CultureInfo.CreateSpecificCulture("en-US");
            DateTimeStyles style = DateTimeStyles.None;

            if (DateTime.TryParseExact(value, format, formatProvider, style, out DateTime dateOfBd)
                && (dateOfBd > MinDateOfBirth && dateOfBd < MaxDateOfBirth))
            {
                this.DateOfBirthday = dateOfBd;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Tries the first name of the get valid.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <returns>Return validation result.</returns>
        public bool TrySetFirstName(string firstName)
        {
            if (!string.IsNullOrWhiteSpace(firstName) && (firstName.Length >= MinLengthFirstName && firstName.Length <= MaxLengthFirstName))
            {
                this.FirstName = firstName;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Tries the last name of the get valid.
        /// </summary>
        /// <param name="lastName">The last name.</param>
        /// <returns>Return validation result.</returns>
        public bool TrySetLastName(string lastName)
        {
            if (!string.IsNullOrWhiteSpace(lastName) && (lastName.Length >= MinLengthLastName && lastName.Length <= MaxLengthLastName))
            {
                this.LastName = lastName;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Tries the get valid working hours per week.
        /// </summary>
        /// <param name="value">The string working hours.</param>
        /// <returns>Return validation result.</returns>
        public bool TrySetWorkingHoursPerWeek(string value)
        {
            if (short.TryParse(value, out short shortValueHours)
                && (shortValueHours >= MinWorkingHoursPerWeek && shortValueHours <= MaxWorkingHoursPerWeek))
            {
                this.WorkingHoursPerWeek = shortValueHours;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Tries the get valid annual income.
        /// </summary>
        /// <param name="value">The string value annual income.</param>
        /// <returns>Return validation result.</returns>
        public bool TrySetAnnualIncome(string value)
        {
            if (decimal.TryParse(value, out decimal annualIncome)
                && annualIncome >= MinAnnualIncome)
            {
                this.AnnualIncome = annualIncome;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Tries the get valid driver license category.
        /// </summary>
        /// <param name="value">The firstName.</param>
        /// <returns>Return validation result.</returns>
        public bool TrySetDriverLicenseCategory(string value)
        {
            if (char.TryParse(value, out char driverLicenseCategory))
            {
                var driverLicenseCategoryUpper = char.ToUpper(driverLicenseCategory, CultureInfo.CurrentCulture);
                if (driverLicenseCategoryUpper == 'A' || driverLicenseCategoryUpper == 'B' || driverLicenseCategoryUpper == 'C' || driverLicenseCategoryUpper == 'D')
                {
                    this.DriverLicenseCategory = driverLicenseCategory;
                    return true;
                }

                return false;
            }

            return false;
        }
    }
}