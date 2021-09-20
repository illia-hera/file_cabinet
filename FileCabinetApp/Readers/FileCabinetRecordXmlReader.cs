using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Serialization;

using FileCabinetApp.Entities;
using FileCabinetApp.Entities.XmlSerialization;
using FileCabinetApp.Validators;

namespace FileCabinetApp.Readers
{
    /// <summary>
    /// Class <c>FileCabinetRecordXmlReader</c> to deserialize xml records.
    /// </summary>
    public class FileCabinetRecordXmlReader
    {
        private readonly StreamReader reader;

        private readonly IRecordValidator inputValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlReader"/> class.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public FileCabinetRecordXmlReader(StreamReader reader)
        {
            this.reader = reader;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlReader"/> class.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="inputValidator">The input validator.</param>
        public FileCabinetRecordXmlReader(StreamReader reader, IRecordValidator inputValidator)
            : this(reader)
        {
            this.inputValidator = inputValidator;
        }

        /// <summary>
        /// Reads all.
        /// </summary>
        /// <returns>Return list of Records.</returns>
        public IList<FileCabinetRecord> ReadAll()
        {
            RecordsGroup recordsGroup;
            var records = new List<FileCabinetRecord>();
            var serializer = new XmlSerializer(typeof(RecordsGroup));

            using (this.reader)
            {
                recordsGroup = serializer.Deserialize(this.reader) as RecordsGroup; // if create XmlReader from this.reader it always will be None
            }

            if (recordsGroup != null && recordsGroup.Record.Count > 0)
            {
                foreach (Record item in recordsGroup.Record)
                {
                    var record = this.CreateCabinetRecord(item);
                    if (record != null)
                    {
                        records.Add(record);
                    }
                }
            }

            return records;
        }

        private ParametersContainer CreateContainer(Record record)
        {
            ParametersContainer container = null;
            try
            {
                container = new ParametersContainer(
                    record.Name.FirstName,
                    record.Name.LastName,
                    record.DateOfBirth,
                    record.WorkingHoursPerWeek,
                    record.AnnualIncome,
                    record.DriverLicenseCategory);

                this.inputValidator.ValidateParameters(container);
            }
            catch (Exception e) when (e is ArgumentException)
            {
                Console.WriteLine($"id #{record.Id}, {e.Message}");
                return null;
            }

            return container;
        }

        private FileCabinetRecord CreateCabinetRecord(Record item)
        {
            var container = this.CreateContainer(item);
            if (container is null)
            {
                return null;
            }

            var record = new FileCabinetRecord
            {
                Id = item.Id,
                FirstName = container.FirstName,
                LastName = container.LastName,
                DateOfBirth = container.DateOfBirthday,
                WorkingHoursPerWeek = container.WorkingHoursPerWeek,
                AnnualIncome = container.AnnualIncome,
                DriverLicenseCategory = container.DriverLicenseCategory,
            };

            return record;
        }
    }
}