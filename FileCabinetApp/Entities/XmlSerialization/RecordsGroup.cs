using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FileCabinetApp.Entities.XmlSerialization
{
    /// <summary>XML export model.</summary>
    [XmlRoot("records")]
    public class RecordsGroup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecordsGroup" /> class.
        /// </summary>
        /// <param name="records">The records.</param>
        public RecordsGroup(List<Record> records)
        {
            this.Record = records;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordsGroup"/> class.
        /// </summary>
        public RecordsGroup()
        {
        }

        /// <summary>
        /// Gets the record.
        /// </summary>
        /// <value>
        /// The record.
        /// </value>
        [XmlElement("record")]
        public List<Record> Record { get; }
    }
}
