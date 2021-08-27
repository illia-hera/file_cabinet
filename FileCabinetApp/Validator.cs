using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public static class Validator
    {
        public static bool IsDateOfBdValid(string birthDay, out DateTime dateOfBd)
        {
            string format = "MM/dd/yyyy";
            CultureInfo formatProvider = CultureInfo.CreateSpecificCulture("en-US");
            DateTimeStyles style = DateTimeStyles.None;
            if (DateTime.TryParseExact(birthDay, format, formatProvider, style, out dateOfBd))
            {
                if (dateOfBd < DateTime.Now)
                {
                    return true;
                }

                Console.WriteLine($"Date of birthday can not be bidder than now date.");
            }
            else
            {
                Console.WriteLine("Incorrect format of date, try to write like that - mm/dd/yyyy");
            }

            return false;
        }
    }
}
