using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Utility
{
    /// <summary>
    /// String extensions.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Removes the special characters.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>Return new string without ' ', '(', ')' characters.</returns>
        public static string RemoveSpecialCharacters(this string str)
        {
            StringBuilder sb = new StringBuilder();
            if (str != null)
            {
                foreach (var t in str)
                {
                    if ((t == ' ') || (t == '(') || (t == ')'))
                    {
                        continue;
                    }

                    sb.Append(t);
                }
            }

            return sb.ToString();
        }
    }
}
