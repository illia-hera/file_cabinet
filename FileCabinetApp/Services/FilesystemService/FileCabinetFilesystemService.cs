using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FileCabinetApp.Entities;
using FileCabinetApp.Services.SnapshotServices;
using FileCabinetApp.Utils;
using FileCabinetApp.Validators;

namespace FileCabinetApp.Services.FileService
{
    /// <summary>
    /// Class <c>FileCabinetFilesystemService</c>.
    /// </summary>
    /// <seealso cref="FileCabinetApp.Services.IFileCabinetService" />
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private const int NameSizes = 120;

        private const int StringsInFile = 2;

        private const int RecordSize = sizeof(int) // id
                                       + (NameSizes * 2) // First name + Last name
                                       + (sizeof(int) * 3)
                                       + sizeof(short) // balance
                                       + sizeof(decimal) // money
                                       + sizeof(char) // account type
                                       + StringsInFile; // String's count because the number in front of each string tell how many bytes are necessary to store the string in binary file

        private readonly FileStream fileStream;

        private readonly IRecordValidator validator = new DefaultValidator();

        private int recordsCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// </summary>
        /// <param name="fileStream">The file stream.</param>
        public FileCabinetFilesystemService(FileStream fileStream)
        {
            this.fileStream = fileStream;

            this.fileStream.Seek(0, SeekOrigin.Begin);
            this.BinaryReader = new BinaryReader(fileStream);
            this.recordsCount = this.fileStream.Length > 0 ? this.BinaryReader.ReadInt16() : 0;
        }

        private BinaryReader BinaryReader { get; set; }

        /// <summary>
        /// Creates the record.
        /// </summary>
        /// <param name="container">The record of parameters.</param>
        /// <returns>
        /// Return Id of created record.
        /// </returns>
        public int CreateRecord(ParametersContainer container)
        {
            this.validator.ValidateParameters(container);
            var offset = this.fileStream.Length;
            this.recordsCount += 1;
            var id = this.recordsCount;

            if (container != null)
            {
                this.WriteRecord(offset, new FileCabinetRecord(container, id));
            }

            return id;
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
            var resultArray = this.recordsCount == 0 ? Array.Empty<FileCabinetRecord>() : new FileCabinetRecord[this.recordsCount];

            for (int i = 0; i < this.recordsCount; i++)
            {
                byte[] buffer = new byte[RecordSize];
                this.fileStream.Seek(RecordSize * i, SeekOrigin.Begin);
                this.fileStream.Read(buffer);
                resultArray[i] = new FileCabinetRecord
                {
                    Id = BitConverter.ToInt32(buffer.AsSpan()[2..6]),
                    FirstName = ByteConverter.ToString(buffer[6..126]),
                    LastName = ByteConverter.ToString(buffer[126..246]),
                    DateOfBirth = ByteConverter.ToDateTime(buffer[246..258]),
                    WorkingHoursPerWeek = BitConverter.ToInt16(buffer.AsSpan()[258..260]),
                    AnnualIncome = ByteConverter.ToDecimal(buffer[260..276]),
                    DriverLicenseCategory = BitConverter.ToChar(buffer.AsSpan()[276..278]),
                };
            }

            return resultArray;
        }

        /// <summary>
        /// Edits the record.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="container">The record of parameters.</param>
        /// <exception cref="System.NotImplementedException">Not implemented.</exception>
        public void EditRecord(int id, ParametersContainer container)
        {
            this.validator.ValidateParameters(container);

            long offset = RecordSize * (id - 1);
            byte[] buffer = new byte[RecordSize];

            this.fileStream.Seek(offset, SeekOrigin.Begin);
            this.fileStream.Write(buffer);

            if (container != null)
            {
                this.WriteRecord(offset, new FileCabinetRecord(container, id));
            }
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
            return this.recordsCount;
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
            var records = this.GetRecords();
            var filteredRecords = records.Where(record =>
                firstName.Equals(record.FirstName, StringComparison.OrdinalIgnoreCase)).ToArray();

            return filteredRecords;
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
            var records = this.GetRecords();
            var filteredRecords = records.Where(record =>
                lastName.Equals(record.LastName, StringComparison.OrdinalIgnoreCase)).ToArray();

            return filteredRecords;
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
            var records = this.GetRecords();
            var filteredRecords = records.Where(record => dateOfBirth == record.DateOfBirth).ToArray();

            return filteredRecords;
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
            var snapshot = new FileCabinetServiceSnapshot(this.GetRecords().ToArray(), this.validator);

            return snapshot;
        }

        /// <summary>
        /// Restores this instance.
        /// </summary>
        /// <param name="snapshot">IFileCabinetServiceSnapshot.</param>
        /// <exception cref="System.NotImplementedException">Not implemented.</exception>
        public void Restore(IFileCabinetServiceSnapshot snapshot)
        {
            if (snapshot == null)
            {
                throw new ArgumentException("Snapshot can't be null");
            }

            if (this.fileStream != null)
            {
                List<FileCabinetRecord> snapShotRecords = snapshot.Records.ToList();
                this.recordsCount = snapShotRecords.Count;
                byte[] buffer = new byte[this.recordsCount * RecordSize];

                this.fileStream.Seek(0, SeekOrigin.Begin);
                this.fileStream.Write(buffer);
                foreach (var record in snapShotRecords)
                {
                    this.WriteRecord((record.Id - 1) * RecordSize, record);
                }
            }
        }

        /// <summary>
        /// Removes the record from FileCabinetApp.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void RemoveRecord(int id)
        {
            throw new NotImplementedException();
        }

        private void WriteRecord(long offset,  FileCabinetRecord record)
        {
            this.fileStream.Seek(offset, SeekOrigin.Begin);
            this.fileStream.Write(BitConverter.GetBytes(this.recordsCount)); // status
            this.fileStream.Seek(offset + 2, SeekOrigin.Begin);
            this.fileStream.Write(BitConverter.GetBytes(record.Id)); // Id
            this.fileStream.Seek(offset + 6, SeekOrigin.Begin);
            this.fileStream.Write(Encoding.GetEncoding("UTF-8").GetBytes(record.FirstName.ToCharArray())); // first name
            this.fileStream.Seek(offset + 126, SeekOrigin.Begin);
            this.fileStream.Write(Encoding.GetEncoding("UTF-8").GetBytes(record.LastName.ToCharArray())); // last name
            this.fileStream.Seek(offset + 246, SeekOrigin.Begin);
            this.fileStream.Write(BitConverter.GetBytes(record.DateOfBirth.Year)); // year
            this.fileStream.Seek(offset + 250, SeekOrigin.Begin);
            this.fileStream.Write(BitConverter.GetBytes(record.DateOfBirth.Month)); // month
            this.fileStream.Seek(offset + 254, SeekOrigin.Begin);
            this.fileStream.Write(BitConverter.GetBytes(record.DateOfBirth.Day)); // day
            this.fileStream.Seek(offset + 258, SeekOrigin.Begin);
            this.fileStream.Write(BitConverter.GetBytes(record.WorkingHoursPerWeek)); // working hours
            this.fileStream.Seek(offset + 260, SeekOrigin.Begin);
            this.fileStream.Write(ByteConverter.GetBytes(record.AnnualIncome)); // annual income
            this.fileStream.Seek(offset + 276, SeekOrigin.Begin);
            this.fileStream.Write(BitConverter.GetBytes(record.DriverLicenseCategory)); // Driver license category
            this.fileStream.Seek(offset + 278, SeekOrigin.Begin);
        }
    }
}
