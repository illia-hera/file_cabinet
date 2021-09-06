using System;
using System.Globalization;

namespace FileCabinetApp
{
    public static class Validator
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

        public static bool TryGetValidDateTimeOfBd(string value, out DateTime dateOfBd)
        {
            string format = "MM/dd/yyyy";
            CultureInfo formatProvider = CultureInfo.CreateSpecificCulture("en-US");
            DateTimeStyles style = DateTimeStyles.None;

            if (DateTime.TryParseExact(value, format, formatProvider, style, out dateOfBd)
                && (dateOfBd > MinDateOfBirth && dateOfBd < MaxDateOfBirth))
            {
                return true;
            }

            return false;
        }

        public static bool TryGetValidFirstName(string value, out string firstName)
        {
            firstName = value;
            if (!string.IsNullOrWhiteSpace(firstName) && (firstName.Length >= MinLengthFirstName && firstName.Length <= MaxLengthFirstName))
            {
                return true;
            }

            return false;
        }

        public static bool TryGetValidLastName(string value, out string lastName)
        {
            lastName = value;
            if (!string.IsNullOrWhiteSpace(lastName) && (lastName.Length >= MinLengthLastName && lastName.Length <= MaxLengthLastName))
            {
                return true;
            }

            return false;
        }

        public static bool TryGetValidWorkingHoursPerWeek(string value, out short shortValueHours)
        {
            if (short.TryParse(value, out shortValueHours)
                && (shortValueHours >= MinWorkingHoursPerWeek && shortValueHours <= MaxWorkingHoursPerWeek))
            {
                return true;
            }

            return false;
        }

        public static bool TryGetValidAnnualIncome(string value, out decimal annualIncome)
        {
            if (decimal.TryParse(value, out annualIncome)
                && annualIncome >= MinAnnualIncome)
            {
                return true;
            }

            return false;
        }

        public static bool TryGetValidDriverLicenseCategory(string value, out char driverLicenseCategory)
        {
            if (char.TryParse(value, out driverLicenseCategory))
            {
                var driverLicenseCategoryUpper = char.ToUpper(driverLicenseCategory, CultureInfo.CurrentCulture);
                if (driverLicenseCategoryUpper == 'A' || driverLicenseCategoryUpper == 'B' || driverLicenseCategoryUpper == 'C' || driverLicenseCategoryUpper == 'D')
                {
                    return true;
                }

                return false;
            }

            return false;
        }
    }
}