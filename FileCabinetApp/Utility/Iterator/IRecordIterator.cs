using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCabinetApp.Entities;

namespace FileCabinetApp.Utility.Iterator
{
    /// <summary>
    /// IRecordIterator.
    /// </summary>
    public interface IRecordIterator
    {
        /// <summary>
        /// Gets the next.
        /// </summary>
        /// <returns>Return next FileCabinetRecord.</returns>
        FileCabinetRecord GetNext();

        /// <summary>
        /// Determines whether this instance has more.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance has more; otherwise, <c>false</c>.
        /// </returns>
        bool HasMore();
    }
}
