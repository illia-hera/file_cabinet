using System;
using System.Globalization;

namespace FileCabinetApp
{
    public static class Parser
    {
        public static bool TryParseDateTimeBd(string birthDay, out DateTime dateOfBd)
        {
            string format = "MM/dd/yyyy";
            CultureInfo formatProvider = CultureInfo.CreateSpecificCulture("en-US");
            DateTimeStyles style = DateTimeStyles.None;
            if (DateTime.TryParseExact(birthDay, format, formatProvider, style, out dateOfBd))
            {
                return true;
            }

            return false;
        }
    }
}