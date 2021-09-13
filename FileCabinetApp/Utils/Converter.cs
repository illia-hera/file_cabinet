using System;
using System.Globalization;

namespace FileCabinetApp.Utils
{
    /// <summary>
    /// Class to convert.
    /// </summary>
    public static class Converter
    {
        /// <summary>
        /// Strings the converter.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Return result of boolean conversion result and string input value and string value.</returns>
        public static Tuple<bool, string, string> StringConverter(string input)
        {
            bool isConverted = !string.IsNullOrWhiteSpace(input);

            return new Tuple<bool, string, string>(isConverted, input, input);
        }

        /// <summary>
        /// Dates the converter.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Return result of boolean conversion result and string input value and <c>DeteTime</c> value.</returns>
        public static Tuple<bool, string, DateTime> DateConverter(string input)
        {
            string format = "MM/dd/yyyy";
            CultureInfo formatProvider = CultureInfo.CreateSpecificCulture("en-US");
            DateTimeStyles style = DateTimeStyles.None;
            bool isConverted = DateTime.TryParseExact(input, format, formatProvider, style, out DateTime dateTimeValue);

            return new Tuple<bool, string, DateTime>(isConverted, input, dateTimeValue);
        }

        /// <summary>
        /// Shorts the converter.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Return result of boolean conversion result and string input value and <c>short</c> value.</returns>
        public static Tuple<bool, string, short> ShortConverter(string input)
        {
            bool isConverted = short.TryParse(input, out short shortValue);

            return new Tuple<bool, string, short>(isConverted, input, shortValue);
        }

        /// <summary>
        /// Characters the converter.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Return result of boolean conversion result and string input value and char value.</returns>
        public static Tuple<bool, string, char> CharConverter(string input)
        {
            bool isConverted = char.TryParse(input, out char charValue);

            return new Tuple<bool, string, char>(isConverted, input, charValue);
        }

        /// <summary>
        /// Decimals the converter.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Return result of boolean conversion result and string input value and decimal value.</returns>
        public static Tuple<bool, string, decimal> DecimalConverter(string input)
        {
            bool isConverted = decimal.TryParse(input, out decimal decimalValue);

            return new Tuple<bool, string, decimal>(isConverted, input, decimalValue);
        }
    }
}
