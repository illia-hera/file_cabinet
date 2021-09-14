using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Utils
{
    /// <summary>
    /// Class <c>ByteConverter</c>.
    /// </summary>
    public static class ByteConverter
    {
        /// <summary>
        /// Convert <c>decimal</c> to <c>byte[]</c>.
        /// </summary>
        /// <param name="dec">The decimal.</param>
        /// <returns>Byte array.</returns>
        public static byte[] GetBytes(decimal dec)
        {
            int[] bits = decimal.GetBits(dec);
            List<byte> bytes = new List<byte>();
            foreach (var i in bits)
            {
                bytes.AddRange(BitConverter.GetBytes(i));
            }

            return bytes.ToArray();
        }

        /// <summary>
        /// Converts byte[] to decimal.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>Return decimal value.</returns>
        /// <exception cref="System.ArgumentException">A decimal must be created from exactly 16 bytes.</exception>
        public static decimal ToDecimal(byte[] bytes)
        {
            if (bytes?.Length != 16)
            {
                throw new ArgumentException("A decimal must be created from exactly 16 bytes");
            }

            int[] bits = new int[4];
            for (int i = 0; i <= 15; i += 4)
            {
                bits[i / 4] = BitConverter.ToInt32(bytes, i);
            }

            return new decimal(bits);
        }

        /// <summary>
        /// Converts to datetime. Split byte array on 3 int numbers and parsed there.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>Return datetime.</returns>
        public static DateTime ToDateTime(byte[] bytes)
        {
            int year = BitConverter.ToInt32(bytes.AsSpan()[0 .. 4]);
            int month = BitConverter.ToInt32(bytes.AsSpan()[4 .. 8]);
            int day = BitConverter.ToInt32(bytes.AsSpan()[8 .. 12]);
            string stringDate = $"{year}{month}{day}";
            DateTime.TryParseExact(stringDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateOfBirth);

            return dateOfBirth;
        }
    }
}
