using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using FileCabinetApp.Entities;
using FileCabinetApp.Services.SnapshotServices;

namespace FileCabinetGenerator
{
    /// <summary>
    /// Class <c>RecordsWrite</c>.
    /// </summary>
    public class RecordsWriter
    {
        private readonly List<FileCabinetRecord> records;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordsWriter"/> class.
        /// </summary>
        /// <param name="records">The records.</param>
        public RecordsWriter(List<FileCabinetRecord> records)
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
            var s = new FileCabinetServiceSnapshot(this.records.ToArray());

            if (fileFormat != null && fileFormat.Equals("csv"))
            {
                using StreamWriter streamWriter = new StreamWriter(filePath);
                s.SaveToCsv(streamWriter);
                streamWriter.Close();
            }
            else if (fileFormat != null && fileFormat.Equals("xml"))
            {
                using XmlWriter xmlWriter = XmlWriter.Create(filePath);
                s.SaveToXml(xmlWriter);
                xmlWriter.Close();
            }
        }
    }
}