using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using FileCabinetApp.Enteties;

namespace FileCabinetApp.SnapshotServices.RecordWriters
{
    /// <summary>
    /// Class <c>FileCabinetRecordXmlWriter</c> write record in to the file.
    /// </summary>
    /// <seealso cref="FileCabinetApp.SnapshotServices.RecordWriters.IFileCabinetRecordWriter" />
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
