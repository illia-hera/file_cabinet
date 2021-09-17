using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using FileCabinetApp.Entities;
using FileCabinetApp.Readers;
using FileCabinetApp.RecordWriters;
using FileCabinetApp.Validators;

namespace FileCabinetApp.Services.SnapshotServices
{
    /// <summary>
    /// Class <c>FileCabinetServiceSnapshot</c> make snapshot of FileCabinetMemoryService.
    /// </summary>
    /// <seealso cref="IFileCabinetServiceSnapshot" />
    public class FileCabinetServiceSnapshot : IFileCabinetServiceSnapshot
    {
        private readonly IRecordValidator inputValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        /// <param name="records">The records.</param>
        public FileCabinetServiceSnapshot(IReadOnlyCollection<FileCabinetRecord> records)
        {
            this.Records = records;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        /// <param name="records">The records.</param>
        /// <param name="inputValidator">The input validator.</param>
        public FileCabinetServiceSnapshot(IReadOnlyCollection<FileCabinetRecord> records, IRecordValidator inputValidator)
            : this(records)
        {
            this.inputValidator = inputValidator;
        }

        /// <summary>
        /// Gets the records.
        /// </summary>
        /// <value>
        /// The records.
        /// </value>
        public IReadOnlyCollection<FileCabinetRecord> Records { get; private set; }

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

            foreach (var fileCabinetRecord in this.Records)
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

            fileCabinetRecordXmlWriter.Write(this.Records);
        }

        /// <summary>Loads from CSV.</summary>
        /// <param name="streamReader">The stream reader.</param>
        /// <exception cref="ArgumentNullException">streamReader is null.</exception>
        public void LoadFromCsv(StreamReader streamReader)
        {
            if (streamReader == null)
            {
                throw new ArgumentNullException(nameof(streamReader));
            }

            var csvReader = new FileCabinetRecordCsvReader(streamReader, this.inputValidator);

            this.Records = csvReader.ReadAll().ToList();
        }
    }
}
