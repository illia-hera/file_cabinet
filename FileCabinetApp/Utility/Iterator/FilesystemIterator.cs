﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCabinetApp.Entities;
using FileCabinetApp.Services;

namespace FileCabinetApp.Utility.Iterator
{
    /// <summary>
    /// Class <c>FilesystemIterator</c>.
    /// </summary>
    /// <seealso cref="FileCabinetApp.Utility.Iterator.IRecordIterator" />
    public class FilesystemIterator : IRecordIterator
    {
        private readonly FileCabinetFilesystemService fileCabinetFilesystemService;
        private int currentPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilesystemIterator"/> class.
        /// </summary>
        /// <param name="fileCabinetFilesystemService">The file cabinet filesystem service.</param>
        public FilesystemIterator(FileCabinetFilesystemService fileCabinetFilesystemService)
        {
            this.fileCabinetFilesystemService = fileCabinetFilesystemService;
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

            FileCabinetRecord record = null;
            try
            {
                record = this.fileCabinetFilesystemService.GetRecord(this.currentPosition * FileCabinetFilesystemService.RecordSize);
                this.currentPosition++;
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return record;
        }

        /// <summary>
        /// Determines whether this instance has more.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance has more; otherwise, <c>false</c>.
        /// </returns>
        public bool HasMore()
        {
            return this.currentPosition < this.fileCabinetFilesystemService.GetStat().Item1;
        }
    }
}