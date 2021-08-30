using System;
using System.Globalization;

namespace FileCabinetApp
{
    public static class Parser
    {
    //    public static bool Parse<T>(Type type, string stringParameter, out T outParameter) => Type.GetTypeCode(type) switch
    //    {
    //        TypeCode.Int16 => short.TryParse(stringParameter, out outParameter),
    //        TypeCode.Char => char.TryParse(stringParameter, out outParameter),
    //        TypeCode.Decimal => decimal.TryParse(stringParameter, out outParameter),
    //        TypeCode.DateTime => Parser.TryParseDateTimeBd(stringParameter, out outParameter),
    //        _ => FalseValue(out outParameter)
    //    };

    //    private static bool FalseValue<T>(out T defaultValue)
    //    {
    //        defaultValue = default;
    //        return false;
    //    }
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