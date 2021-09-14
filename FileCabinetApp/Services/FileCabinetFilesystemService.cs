using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCabinetApp.Enteties;
using FileCabinetApp.SnapshotServices;
using FileCabinetApp.Utils;

namespace FileCabinetApp.Services
{
    /// <summary>
    /// Class <c>FileCabinetFilesystemService</c>.
    /// </summary>
    /// <seealso cref="FileCabinetApp.Services.IFileCabinetService" />
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        /// <summary>
        /// The file stream.
        /// </summary>
        private readonly FileStream fileStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// </summary>
        /// <param name="fileStream">The file stream.</param>
        public FileCabinetFilesystemService(FileStream fileStream)
        {
            this.fileStream = fileStream;
        }

        /// <summary>
        /// Creates the record.
        /// </summary>
        /// <param name="container">The container of parameters.</param>
        /// <returns>
        /// Return Id of created record.
        /// </returns>
        public int CreateRecord(ParametersContainer container)
        {
            var offset = this.fileStream.Length;
            var id = (int)(offset / 278) + 1;
            if (container != null)
            {
                this.fileStream.Seek(offset, SeekOrigin.Begin);
                this.fileStream.Write(BitConverter.GetBytes(short.MinValue)); // status
                this.fileStream.Seek(offset + 2, SeekOrigin.Begin);
                this.fileStream.Write(BitConverter.GetBytes(id)); // Id
                this.fileStream.Seek(offset + 6, SeekOrigin.Begin);
                this.fileStream.Write(Encoding.GetEncoding("UTF-8").GetBytes(container.FirstName.ToCharArray())); // first name
                this.fileStream.Seek(offset + 126, SeekOrigin.Begin);
                this.fileStream.Write(Encoding.GetEncoding("UTF-8").GetBytes(container.LastName.ToCharArray())); // last name
                this.fileStream.Seek(offset + 246, SeekOrigin.Begin);
                this.fileStream.Write(BitConverter.GetBytes(container.DateOfBirthday.Year)); // year
                this.fileStream.Seek(offset + 250, SeekOrigin.Begin);
                this.fileStream.Write(BitConverter.GetBytes(container.DateOfBirthday.Month)); // month
                this.fileStream.Seek(offset + 254, SeekOrigin.Begin);
                this.fileStream.Write(BitConverter.GetBytes(container.DateOfBirthday.Day)); // day
                this.fileStream.Seek(offset + 258, SeekOrigin.Begin);
                this.fileStream.Write(BitConverter.GetBytes(container.WorkingHoursPerWeek)); // working hours
                this.fileStream.Seek(offset + 260, SeekOrigin.Begin);
                this.fileStream.Write(ByteConverter.GetBytes(container.AnnualIncome)); // annual income
                this.fileStream.Seek(offset + 276, SeekOrigin.Begin);
                this.fileStream.Write(BitConverter.GetBytes(container.DriverLicenseCategory)); // Driver license category
                this.fileStream.Seek(offset + 278, SeekOrigin.Begin);
            }

            return id;
        }

        /// <summary>
        /// Edits the record.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="container">The container of parameters.</param>
        /// <exception cref="System.NotImplementedException">Not implemented.</exception>
        public void EditRecord(int id, ParametersContainer container)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets all records in File Cabinet.
        /// </summary>
        /// <returns>
        /// Return array of <c>FileCabinetRecord</c>.
        /// </returns>
        /// <exception cref="System.NotImplementedException">Not implemented.</exception>
        public IReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            var offset = this.fileStream.Length;
            var count = (int)(offset / 278);
            var resultArray = count == 0 ? Array.Empty<FileCabinetRecord>() : new FileCabinetRecord[count];

            for (int i = 0; i < count; i++)
            {
                byte[] buffer = new byte[278];
                this.fileStream.Seek(278 * i, SeekOrigin.Begin);
                this.fileStream.Read(buffer);
                resultArray[i] = new FileCabinetRecord
                {
                    Id = BitConverter.ToInt32(buffer.AsSpan()[2..6]),
                    FirstName = Encoding.Default.GetString(buffer[6..126]),
                    LastName = Encoding.Default.GetString(buffer[126..246]),
                    DateOfBirth = ByteConverter.ToDateTime(buffer[246..258]),
                    WorkingHoursPerWeek = BitConverter.ToInt16(buffer.AsSpan()[258..260]),
                    AnnualIncome = ByteConverter.ToDecimal(buffer[260..276]),
                    DriverLicenseCategory = BitConverter.ToChar(buffer.AsSpan()[276..278]),
                };
            }

            return resultArray;
        }

        /// <summary>
        /// Gets the stat of users.
        /// </summary>
        /// <returns>
        /// Return count of records in File Cabinet.
        /// </returns>
        /// <exception cref="System.NotImplementedException">Not implemented.</exception>
        public int GetStat()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds the records by the first name.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <returns>
        /// Return array of records.
        /// </returns>
        /// <exception cref="System.NotImplementedException">Not implemented.</exception>
        public IReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds the records by the last name.
        /// </summary>
        /// <param name="lastName">The last name.</param>
        /// <returns>
        /// Return array of records.
        /// </returns>
        /// <exception cref="System.NotImplementedException">Not implemented.</exception>
        public IReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds the records by date of birthday.
        /// </summary>
        /// <param name="dateOfBirth">The date of birthday.</param>
        /// <returns>
        /// Return array of records.
        /// </returns>
        /// <exception cref="System.NotImplementedException">Not implemented.</exception>
        public IReadOnlyCollection<FileCabinetRecord> FindByDateOfBirthName(DateTime dateOfBirth)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Makes the snapshot.
        /// </summary>
        /// <returns>
        /// Return <c>FileCabinetServiceSnapshot</c>.
        /// </returns>
        /// <exception cref="System.NotImplementedException">Not implemented.</exception>
        public IFileCabinetServiceSnapshot MakeSnapshot()
        {
            throw new NotImplementedException();
        }
    }
}
