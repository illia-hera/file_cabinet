using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FileCabinetApp.Entities;

namespace FileCabinetApp.Utility.Iterator
{
    /// <summary>
    /// Memory enumerable.
    /// </summary>
    /// <seealso cref="FileCabinetRecord" />
    public class MemoryEnumerable : IEnumerable<FileCabinetRecord>
    {
        private List<FileCabinetRecord> records;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryEnumerable"/> class.
        /// </summary>
        /// <param name="records">The records.</param>
        public MemoryEnumerable(IEnumerable<FileCabinetRecord> records)
        {
            this.records = records.ToList();
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>Return Memory enumerator.</returns>
        public IEnumerator<FileCabinetRecord> GetEnumerator()
        {
           return new MemoryEnumerator(this.records);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
