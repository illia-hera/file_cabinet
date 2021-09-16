using System.IO;
using System.Xml;

namespace FileCabinetApp.Services.SnapshotServices
{
    /// <summary>
    /// Interface service snapshot.
    /// </summary>
    public interface IFileCabinetServiceSnapshot
    {
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
    }
}
