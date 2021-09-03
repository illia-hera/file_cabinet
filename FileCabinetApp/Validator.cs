using System;
using System.Globalization;

namespace FileCabinetApp
{
    public static class Validator
    {
        public static bool TryGetValidDateTimeOfBd(string value, out DateTime dateOfBd)
        {
            string format = "MM/dd/yyyy";
            CultureInfo formatProvider = CultureInfo.CreateSpecificCulture("en-US");
            DateTimeStyles style = DateTimeStyles.None;
            DateTime minimalDate = DateTime.Parse("01-Jan-1950", CultureInfo.CurrentCulture);

            if (DateTime.TryParseExact(value, format, formatProvider, style, out dateOfBd)
                && (dateOfBd > minimalDate && dateOfBd < DateTime.Now))
            {
                return true;
            }

            return false;
        }

        public static bool TryGetValidFirstName(string value, out string firstName)
        {
            firstName = value;
            if (string.IsNullOrWhiteSpace(firstName) || (firstName.Length > 2 && firstName.Length < 60))
            {
                return true;
            }

            return false;
        }

        public static bool TryGetValidLastName(string value, out string lastName)
        {
            lastName = value;
            if (string.IsNullOrWhiteSpace(lastName) || (lastName.Length > 2 && lastName.Length < 60))
            {
                return true;
            }

            return false;
        }

        public static bool TryGetValidWorkingHoursPerWeek(string value, out short shortValueHours)
        {
            if (short.TryParse(value, out shortValueHours)
                && (shortValueHours > 0 && shortValueHours < 40))
            {
                return true;
            }

            return false;
        }

        public static bool TryGetValidAnnualIncome(string value, out decimal annualIncome)
        {
            if (decimal.TryParse(value, out annualIncome)
                && annualIncome > 0)
            {
                return true;
            }

            return false;
        }

        public static bool TryGetValidDriverLicenseCategory(string value, out char driverLicenseCategory)
        {
            if (char.TryParse(value, out driverLicenseCategory))
            {
                var driverLicenseCategoryUpper = char.ToUpper(driverLicenseCategory, CultureInfo.CreateSpecificCulture("en-US"));
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