using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCabinetApp.Entities;

namespace FileCabinetApp.Printers
{
    /// <summary>
    /// Interface <c>IRecordPrinter</c>.
    /// </summary>
    internal interface IRecordPrinter
    {
        /// <summary>
        /// Prints the specified records.
        /// </summary>
        /// <param name="records">The records.</param>
        void Print(IEnumerable<FileCabinetRecord> records);
    }
}
