using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using FileCabinetApp.Entities;
using FileCabinetApp.Entities.XmlSerialization;

namespace FileCabinetApp.RecordWriters
{
    /// <summary>
    /// Class <c>FileCabinetRecordXmlWriter</c> write record in to the file.
    /// </summary>
    /// <seealso cref="IFileCabinetRecordWriter" />
    public class FileCabinetRecordXmlWriter : IFileCabinetRecordWriter
    {
        private readonly XmlWriter xmlWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlWriter"/> class.
        /// </summary>
        /// <param name="xmlWriter">The XML writer.</param>
        public FileCabinetRecordXmlWriter(XmlWriter xmlWriter)
        {
            this.xmlWriter = xmlWriter;
        }

        /// <summary>
        /// Writes record in to the file.
        /// </summary>
        /// <param name="record">The record.</param>
        public void Write(FileCabinetRecord record)
        {
            if (record != null)
            {
                this.xmlWriter.WriteStartElement("record");
                this.xmlWriter.WriteAttributeString("id", $"{record.Id}");

                this.xmlWriter.WriteStartElement("name");
                this.xmlWriter.WriteAttributeString("first", $"{record.FirstName}");
                this.xmlWriter.WriteAttributeString("last", $"{record.LastName}");
                this.xmlWriter.WriteEndElement();

                this.xmlWriter.WriteStartElement("dateOfBirth");
                this.xmlWriter.WriteString($"{record.DateOfBirth.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)}");
                this.xmlWriter.WriteEndElement();

                this.xmlWriter.WriteStartElement("workingHoursPerWeek");
                this.xmlWriter.WriteString($"{record.WorkingHoursPerWeek}");
                this.xmlWriter.WriteEndElement();

                this.xmlWriter.WriteStartElement("annualIncome");
                this.xmlWriter.WriteString($"{record.AnnualIncome}");
                this.xmlWriter.WriteEndElement();

                this.xmlWriter.WriteStartElement("driverLicenseCategory");
                this.xmlWriter.WriteString($"{record.DriverLicenseCategory}");
                this.xmlWriter.WriteEndElement();

                this.xmlWriter.WriteEndElement();
            }
        }

        /// <summary>
        /// Writes the specified records.
        /// </summary>
        /// <param name="records">The records.</param>
        public void Write(IReadOnlyCollection<FileCabinetRecord> records)
        {
            if (records == null)
            {
                return;
            }

            var recordsList = new List<Record>(records.Count);

            foreach (var record in records)
            {
                recordsList.Add(
                    new Record()
                    {
                        DriverLicenseCategory = record.DriverLicenseCategory,
                        AnnualIncome = record.AnnualIncome,
                        WorkingHoursPerWeek = record.WorkingHoursPerWeek,
                        DateOfBirth = record.DateOfBirth,
                        Id = record.Id,
                        Name = new Name() { FirstName = record.FirstName, LastName = record.LastName, },
                    });
            }

            RecordsGroup recordsGroup = new RecordsGroup() { Record = new Collection<Record>(recordsList) };
            XmlSerializer serializer = new XmlSerializer(typeof(RecordsGroup));
            using XmlWriter xw = XmlWriter.Create(this.xmlWriter, new XmlWriterSettings { Indent = true, IndentChars = "\t", });
            serializer.Serialize(xw, recordsGroup, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty, }));
        }

        /// <summary>
        /// Writes the start of document.
        /// </summary>
        public void WriteStartOfDoc()
        {
            this.xmlWriter.WriteStartDocument();
            this.xmlWriter.WriteStartElement("records");
        }

        /// <summary>
        /// Writes the end of document.
        /// </summary>
        public void WriteEndOfDoc()
        {
            this.xmlWriter.WriteEndDocument();
        }
    }
}
