using FileCabinetApp.Entities;

namespace FileCabinetApp.RecordWriters
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
