using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCabinetApp.Entities;
using FileCabinetApp.Services;

namespace FileCabinetApp.Utility.Iterator
{
    /// <summary>
    /// Class <c>MemoryIterator</c>.
    /// </summary>
    /// <seealso cref="FileCabinetApp.Utility.Iterator.IRecordIterator" />
    public class MemoryIterator : IRecordIterator
    {
        private readonly List<FileCabinetRecord> records;

        private int currentPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryIterator" /> class.
        /// </summary>
        /// <param name="records">The records.</param>
        public MemoryIterator(List<FileCabinetRecord> records)
        {
            this.records = records ?? throw new ArgumentNullException(nameof(records));
        }

        /// <summary>
        /// Gets the next.
        /// </summary>
        /// <returns>
        /// Return next FileCabinetRecord.
        /// </returns>
        public FileCabinetRecord GetNext()
        {
            if (!this.HasMore())
            {
                return null;
            }

            return this.records[this.currentPosition++];
        }

        /// <summary>
        /// Determines whether this instance has more.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance has more; otherwise, <c>false</c>.
        /// </returns>
        public bool HasMore()
        {
            return this.currentPosition < this.records.Count;
        }
    }
}
