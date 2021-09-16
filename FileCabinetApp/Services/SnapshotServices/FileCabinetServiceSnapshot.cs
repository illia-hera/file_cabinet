using System;
using System.IO;
using System.Xml;
using FileCabinetApp.Entities;
using FileCabinetApp.RecordWriters;

namespace FileCabinetApp.Services.SnapshotServices
{
    /// <summary>
    /// Class <c>FileCabinetServiceSnapshot</c> make snapshot of FileCabinetMemoryService.
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
            if (streamWriter is null)
            {
                throw new ArgumentNullException(nameof(streamWriter));
            }

            var fileCabinetRecordCsvWriter = new FileCabinetRecordCsvWriter(streamWriter);
            fileCabinetRecordCsvWriter.WriteHeader();

            foreach (var fileCabinetRecord in this.records)
            {
                fileCabinetRecordCsvWriter.Write(fileCabinetRecord);
            }
        }

        /// <summary>
        /// Saves to XML.
        /// </summary>
        /// <param name="xmlWriter">The XML writer.</param>
        public void SaveToXml(XmlWriter xmlWriter)
        {
            if (xmlWriter is null)
            {
                throw new ArgumentNullException(nameof(xmlWriter));
            }

            var fileCabinetRecordXmlWriter = new FileCabinetRecordXmlWriter(xmlWriter);

            fileCabinetRecordXmlWriter.Write(this.records);
        }
    }
}
