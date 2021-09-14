using System;
using System.Collections.Generic;
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
        /// Convert <c>byte[]</c> to <c>decimal</c>.
        /// </summary>
        /// <param name="intValue">The int value.</param>
        /// <returns>Return decimal number.</returns>
        public static byte[] GetBytes(int intValue)
        {
            int[] bits = decimal.GetBits(intValue);
            List<byte> bytes = new List<byte>();
            foreach (var i in bits)
            {
                bytes.AddRange(BitConverter.GetBytes(i));
            }

            return bytes.ToArray();
        }
    }
}
