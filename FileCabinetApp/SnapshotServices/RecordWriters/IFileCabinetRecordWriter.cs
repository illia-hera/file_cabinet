using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCabinetApp.Enteties;

namespace FileCabinetApp.SnapshotServices.RecordWriters
{
    /// <summary>
    /// Interface <c>IFileCabinetRecordWriter</c> write record in to the file.
    /// </summary>
    public interface IFileCabinetRecordWriter
    {
        /// <summary>
        /// Writes record in to the file.
        /// </summary>
        /// <param name="record">The record.</param>
        public void Write(FileCabinetRecord record);
    }
}
