using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FileCabinetApp.Entities.XmlSerialization
{
    /// <summary>XML export model.</summary>
    [XmlRoot(ElementName = "records", IsNullable = false)]
    [Serializable]
    public class RecordsGroup
    {
        /// <summary>
        /// Gets or sets the record.
        /// </summary>
        /// <value>
        /// The record.
        /// </value>
        [XmlElement("record")]
        public Collection<Record> Record { get; set; } // if change Record to be read-only it will lead to Errors in Deserialization xml file.
    }
}
