using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace FileCabinetApp.Entities
{
    /// <summary>
    /// Class <c>Options</c>.
    /// </summary>
    public class Options
    {
        /// <summary>
        /// Gets or sets the validation rules.
        /// </summary>
        /// <value>
        /// The validation rules.
        /// </value>
        [Option('v', "validation-rules", Required = false)]
        public string ValidationRules { get; set; }

        /// <summary>
        /// Gets or sets the storage rules.
        /// </summary>
        /// <value>
        /// The storage rules.
        /// </value>
        [Option('s', "storage", Required = false)]
        public string StorageRules { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [stop watch use].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [stop watch use]; otherwise, <c>false</c>.
        /// </value>
        [Option("use-stopwatch", Required = false)]
        public bool StopWatchUse { get; set; }
    }
}
