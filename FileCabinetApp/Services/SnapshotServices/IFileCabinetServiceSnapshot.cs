using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml;
using FileCabinetApp.Entities;

namespace FileCabinetApp.Services.SnapshotServices
{
    /// <summary>
    /// Interface service snapshot.
    /// </summary>
    public interface IFileCabinetServiceSnapshot
    {
        /// <summary>
        /// Gets the records.
        /// </summary>
        /// <value>
        /// The records.
        /// </value>
        public IReadOnlyCollection<FileCabinetRecord> Records { get; }

        /// <summary>
        /// Saves to CSV.
        /// </summary>
        /// <param name="streamWriter">The stream writer.</param>
        public void SaveToCsv(StreamWriter streamWriter);

        /// <summary>
        /// Saves to XML.
        /// </summary>
        /// <param name="xmlWriter">The XML writer.</param>
        public void SaveToXml(XmlWriter xmlWriter);

        /// <summary>
        /// Loads from CSV.
        /// </summary>
        /// <param name="streamReader">The stream reader.</param>
        public void LoadFromCsv(StreamReader streamReader);

        /// <summary>
        /// Loads from XML.
        /// </summary>
        /// <param name="streamReader">The stream reader.</param>
        public void LoadFromXml(StreamReader streamReader);
    }
}
