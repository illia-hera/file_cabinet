using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using FileCabinetApp.Entities;
using FileCabinetApp.Services;

namespace FileCabinetApp.Utility.Iterator
{
    /// <summary>
    /// FileCabinetFilesystem Enumerator.
    /// </summary>
    /// <seealso cref="FileCabinetRecord" />
    public class FilesystemEnumerator : IEnumerator<FileCabinetRecord>
    {
        private readonly FileCabinetFilesystemService filesystemService;
        private int currentPosition = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilesystemEnumerator"/> class.
        /// </summary>
        /// <param name="filesystemService">The filesystem service.</param>
        public FilesystemEnumerator(FileCabinetFilesystemService filesystemService)
        {
            this.filesystemService = filesystemService;
        }

        /// <summary>
        /// Gets the element in the collection at the current position of the enumerator.
        /// </summary>
        /// <value></value>
        public FileCabinetRecord Current
        {
            get
            {
                try
                {
                    return this.filesystemService.GetRecord(FileCabinetFilesystemService.RecordSize * this.currentPosition);
                }
                catch (IOException e)
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
        /// Advances the enumerator to the next element of the collection.
        /// </summary>
        /// <returns>
        ///   <see langword="true" /> if the enumerator was successfully advanced to the next element; <see langword="false" /> if the enumerator has passed the end of the collection.
        /// </returns>
        public bool MoveNext()
        {
            this.currentPosition++;

            return this.currentPosition < this.filesystemService.GetStat().Item1;
        }

        /// <summary>
        /// Sets the enumerator to its initial position, which is before the first element in the collection.
        /// </summary>
        public void Reset()
        {
            this.currentPosition = -1;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
