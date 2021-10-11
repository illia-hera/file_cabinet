using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Readers
{
    /// <summary>
    /// Parameter reader.
    /// </summary>
    public static class ParameterReaders
    {
        /// <summary>
        /// Reads the input.
        /// </summary>
        /// <typeparam name="T">Parameter type.</typeparam>
        /// <param name="input">The input.</param>
        /// <param name="converter">The converter.</param>
        /// <param name="validator">The validator.</param>
        /// <returns>
        /// Return value.
        /// </returns>
        /// <exception cref="System.ArgumentException">Conversion failed: {conversionResult.Item2}. Please, correct your input.
        /// or
        /// Conversion failed: {validationResult.Item2}. Please, correct your input.</exception>
        public static T ReadInput<T>(string input, Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        {
            T value;

            var conversionResult = converter != null ? converter(input) : new Tuple<bool, string, T>(false, null, default);

            if (!conversionResult.Item1)
            {
                throw new ArgumentException($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
            }

            value = conversionResult.Item3;

            var validationResult = validator != null ? validator(value) : new Tuple<bool, string>(false, null);
            if (!validationResult.Item1)
            {
                throw new ArgumentException($"Validation failed: {validationResult.Item2}. Please, correct your input.");
            }

            return value;
        }

        /// <summary>
        /// Reads the input.
        /// </summary>
        /// <typeparam name="T">Parameter types.</typeparam>
        /// <param name="converter">The converter.</param>
        /// <param name="validator">The validator.</param>
        /// <returns>Return value.</returns>
        public static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        {
            do
            {
                T value;

                var input = Console.ReadLine();
                var conversionResult = converter != null ? converter(input) : new Tuple<bool, string, T>(false, null, default);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                    continue;
                }

                value = conversionResult.Item3;

                var validationResult = validator != null ? validator(value) : new Tuple<bool, string>(false, null);
                if (!validationResult.Item1)
                {
                    Console.WriteLine($"Validation failed: {validationResult.Item2}. Please, correct your input.");
                    continue;
                }

                return value;
            }
            while (true);
        }
    }
}
