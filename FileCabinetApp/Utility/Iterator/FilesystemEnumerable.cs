using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCabinetApp.Entities;
using FileCabinetApp.Services;

namespace FileCabinetApp.Utility.Iterator
{
    /// <summary>
    /// Filesystem enumerable.
    /// </summary>
    /// <seealso cref="FileCabinetRecord" />
    public class FilesystemEnumerable : IEnumerable<FileCabinetRecord>
    {
        private readonly FileCabinetFilesystemService fileCabinetFilesystemService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilesystemEnumerable"/> class.
        /// </summary>
        /// <param name="fileCabinetFilesystemService">The file cabinet filesystem service.</param>
        public FilesystemEnumerable(FileCabinetFilesystemService fileCabinetFilesystemService)
        {
            this.fileCabinetFilesystemService = fileCabinetFilesystemService;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<FileCabinetRecord> GetEnumerator()
        {
            return new FilesystemEnumerator(this.fileCabinetFilesystemService);
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
