using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FileCabinetApp.Entities;

namespace FileCabinetApp.Utility.Iterator
{
    /// <summary>
    /// Memory enumerator.
    /// </summary>
    /// <seealso cref="FileCabinetRecord" />
    public class MemoryEnumerator : IEnumerator<FileCabinetRecord>
    {
        private readonly List<FileCabinetRecord> records;

        private int currentPosition = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryEnumerator"/> class.
        /// </summary>
        /// <param name="records">The records.</param>
        public MemoryEnumerator(IEnumerable<FileCabinetRecord> records)
        {
            this.records = records.ToList();
        }

        /// <summary>
        /// Gets the element in the collection at the current position of the enumerator.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">.</exception>
        /// <value></value>
        public FileCabinetRecord Current
        {
            get
            {
                try
                {
                    return this.records[this.currentPosition];
                }
                catch (IndexOutOfRangeException e)
                {
                    Console.WriteLine(e);
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the element in the collection at the current position of the enumerator.
        /// </summary>
        /// <value></value>
        object IEnumerator.Current => this.Current;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Advances the enumerator to the next element of the collection.
        /// </summary>
        /// <returns>
        ///   <see langword="true" /> if the enumerator was successfully advanced to the next element; <see langword="false" /> if the enumerator has passed the end of the collection.
        /// </returns>
        public bool MoveNext()
        {
            this.currentPosition++;
            return this.currentPosition < this.records.Count;
        }

        /// <summary>
        /// Sets the enumerator to its initial position, which is before the first element in the collection.
        /// </summary>
        public void Reset()
        {
            this.currentPosition = -1;
        }
    }
}
