using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using FileCabinetApp.Entities;
using FileCabinetApp.Services.SnapshotServices;

namespace FileCabinetGenerator.Writer
{
    /// <summary>
    /// Class <c>RecordsWrite</c>.
    /// </summary>
    public class RecordsWriter
    {
        private readonly IReadOnlyCollection<FileCabinetRecord> records;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordsWriter"/> class.
        /// </summary>
        /// <param name="records">The records.</param>
        public RecordsWriter(IReadOnlyCollection<FileCabinetRecord> records)
        {
            this.records = records;
        }

        /// <summary>
        /// Writes the records.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="fileFormat">The file format.</param>
        public void WriteRecords(string filePath, string fileFormat)
        {
            var s = new FileCabinetServiceSnapshot(this.records);

            if (fileFormat != null && fileFormat.Equals("csv", StringComparison.Ordinal))
            {
                using StreamWriter streamWriter = new StreamWriter(filePath);
                s.SaveToCsv(streamWriter);
                streamWriter.Close();
            }
            else if (fileFormat != null && fileFormat.Equals("xml", StringComparison.Ordinal))
            {
                using XmlWriter xmlWriter = XmlWriter.Create(filePath);
                s.SaveToXml(xmlWriter);
                xmlWriter.Close();
            }
        }
    }
}