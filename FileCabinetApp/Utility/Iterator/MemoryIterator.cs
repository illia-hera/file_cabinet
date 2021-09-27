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
        private readonly List<FileCabinetRecord> list;

        private readonly FileCabinetMemoryService fileCabinetMemoryService;

        private int currentPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryIterator"/> class.
        /// </summary>
        /// <param name="fileCabinetMemoryService">The file cabinet memory service.</param>
        public MemoryIterator(FileCabinetMemoryService fileCabinetMemoryService)
        {
            this.fileCabinetMemoryService = fileCabinetMemoryService;
            this.list = this.fileCabinetMemoryService.GetRecords().ToList();
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

            this.currentPosition++;

            return this.list[this.currentPosition];
        }

        /// <summary>
        /// Determines whether this instance has more.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance has more; otherwise, <c>false</c>.
        /// </returns>
        public bool HasMore()
        {
            return this.currentPosition < this.fileCabinetMemoryService.GetStat().Item1;
        }
    }
}
