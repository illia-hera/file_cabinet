using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Entities.JsonSerialization
{
    /// <summary>
    /// Date of birthday.
    /// </summary>
    public class DateOfBirth
    {
        /// <summary>
        /// Gets or sets the minimum.
        /// </summary>
        /// <value>
        /// The minimum.
        /// </value>
        public DateTime From { get; set; }

        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        /// <value>
        /// The minimum.
        /// </value>
        public DateTime To { get; set; }
    }
}
