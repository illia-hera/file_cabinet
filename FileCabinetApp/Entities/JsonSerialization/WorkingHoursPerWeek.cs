using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Entities.JsonSerialization
{
    /// <summary>
    /// Working hours per week.
    /// </summary>
    public class WorkingHoursPerWeek
    {
        /// <summary>
        /// Gets or sets the minimum.
        /// </summary>
        /// <value>
        /// The minimum.
        /// </value>
        public short Min { get; set; }

        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        /// <value>
        /// The minimum.
        /// </value>
        public short Max { get; set; }
    }
}
