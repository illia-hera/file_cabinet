using System.IO;
using FileCabinetApp.e;

namespace FileCabinetApp.SnapshotServices
{
    /// <summary>
    /// Class <c>FileCabinetServiceSnapshot</c> make snapshot of FileCabinetService.
    /// </summary>
    /// <seealso cref="IFileCabinetServiceSnapshot" />
    public class FileCabinetServiceSnapshot : IFileCabinetServiceSnapshot
    {
        private readonly FileCabinetRecord[] records;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        /// <param name="records">The records.</param>
        public FileCabinetServiceSnapshot(FileCabinetRecord[] records)
        {
            this.records = records;
        }

        /// <summary>
        /// Saves to CSV.
        /// </summary>
        /// <param name="streamWriter">The stream writer.</param>
        public void SaveToCsv(StreamWriter streamWriter)
        {
            var fileCabinetRecordCsvWriter = new FileCabinetRecordCsvWriter(streamWriter);
            fileCabinetRecordCsvWriter.WriteHeader();

            foreach (var fileCabinetRecord in this.records)
            {
                fileCabinetRecordCsvWriter.Write(fileCabinetRecord);
            }
        }
    }
}
