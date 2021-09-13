using System.IO;

namespace FileCabinetApp.SnapshotServices
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
    }
}
