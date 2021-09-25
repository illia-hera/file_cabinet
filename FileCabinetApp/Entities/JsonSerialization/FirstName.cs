using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Entities.JsonSerialization
{
    /// <summary>
    /// First name.
    /// </summary>
    public class FirstName
    {
        /// <summary>
        /// Gets or sets the minimum of the last name string length.
        /// </summary>
        /// <value>
        /// The minimum.
        /// </value>
        public int Min { get; set; }

        /// <summary>
        /// Gets or sets the maximum of the last name string length.
        /// </summary>
        /// <value>
        /// The minimum.
        /// </value>
        public int Max { get; set; }
    }
}
