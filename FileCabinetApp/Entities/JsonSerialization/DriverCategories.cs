using System.Collections.Generic;
using System.Linq;

namespace FileCabinetApp.Entities.JsonSerialization
{
    /// <summary>
    /// Driver Categories.
    /// </summary>
    public class DriverCategories
    {
        /// <summary>
        /// Gets or sets the actual categories.
        /// </summary>
        /// <value>
        /// The actual categories.
        /// </value>
        public IReadOnlyCollection<char> ActualCategories { get; set; } // it is need to get records from json
    }
}
