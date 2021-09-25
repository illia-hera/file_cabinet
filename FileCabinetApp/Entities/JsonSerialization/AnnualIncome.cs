using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Entities.JsonSerialization
{
    /// <summary>
    /// Annual income.
    /// </summary>
    public class AnnualIncome
    {
        /// <summary>
        /// Gets or sets the minimum.
        /// </summary>
        /// <value>
        /// The minimum.
        /// </value>
        public int Min { get; set; }

        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        /// <value>
        /// The minimum.
        /// </value>
        public int Max { get; set; }
    }
}
