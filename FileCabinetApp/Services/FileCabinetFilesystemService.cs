using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using FileCabinetApp.Entities;
using FileCabinetApp.Services.SnapshotServices;
using FileCabinetApp.Utility;
using FileCabinetApp.Validators.RecordValidator;

namespace FileCabinetApp.Services
{
    /// <summary>
    /// Class <c>FileCabinetFilesystemService</c>.
    /// </summary>
    /// <seealso cref="FileCabinetApp.Services.IFileCabinetService" />
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        /// <summary>
        /// The record size.
        /// </summary>
        public const int RecordSize = sizeof(int) // id
                                      + (NameSizes * 2) // First name + Last name
                                      + (sizeof(int) * 3)
                                      + sizeof(short) // balance
                                      + sizeof(decimal) // money
                                      + sizeof(char) // account type
                                      + StringsInFile; // String's count because the number in front of each string tell how many bytes are necessary to store the string in binary file

        private const int NameSizes = 120;

        private const int StringsInFile = 2;

        private readonly FileStream fileStream;

        private readonly IRecordValidator validator;

        private int recordsCount;

        private int deletedRecordsCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// </summary>
        /// <param name="fileStream">The file stream.</param>
        public FileCabinetFilesystemService(FileStream fileStream)
        {
            this.fileStream = fileStream;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFilesystemService" /> class.
        /// </summary>
        /// <param name="fileStream">The file stream.</param>
        /// <param name="validator">The validator.</param>
        public FileCabinetFilesystemService(FileStream fileStream, IRecordValidator validator)
            : this(fileStream)
        {
            this.validator = validator;
        }

        private enum RecordParametersOffsets : long
        {
            Status = 0,
            Id = Status + sizeof(short),
            FirstName = Id + sizeof(int),
            LastName = FirstName + 120,
            DateOfBirth = LastName + 120,
            WorkingHours = DateOfBirth + (sizeof(int) * 3),
            AnnualIncome = WorkingHours + sizeof(short),
            DriverCategory = AnnualIncome + sizeof(decimal),
        }

        private Dictionary<string, List<int>> FieldOffsetDictionary => this.InitializeIndexes();

        /// <summary>
        /// Creates the record.
        /// </summary>
        /// <param name="container">The record of parameters.</param>
        /// <returns>
        /// Return Id of created record.
        /// </returns>
        /// <exception cref="System.ArgumentException">Container can not be null.</exception>
        public int CreateRecord(ParametersContainer container)
        {
            this.validator.ValidateParameters(container);
            var offset = this.fileStream.Length;
            this.recordsCount += 1;
            var id = this.recordsCount;

            if (container == null)
            {
                throw new ArgumentException($"Container can not be null");
            }

            this.WriteRecord(offset, new FileCabinetRecord(container, id));
            return id;
        }

        /// <summary>
        /// Inserts the record.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>
        /// Return is record inserted.
        /// </returns>
        /// <exception cref="System.ArgumentException">Container can not be null.</exception>
        public bool InsertRecord(ParametersContainer container)
        {
            if (container == null)
            {
                throw new ArgumentException($"Container can not be null");
            }

            this.validator.ValidateParameters(container);
            (_, FileCabinetRecord record) = this.FindRecordById(container.Id);

            if (record != null && record.Status == 1)
            {
                this.EditRecord(container.Id, container);
                return true;
            }

            if (record != null)
            {
                return false;
            }

            this.WriteRecord(this.recordsCount * RecordSize, new FileCabinetRecord(container, container.Id));
            this.recordsCount++;
            return true;
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

            for (int i = 0, count = 0; count < this.recordsCount; i++, count++)
            {
                long position = RecordSize * i;
                var record = this.GetRecord(position);
                if (record.Status == 1)
                {
                    count--;
                    continue;
                }

                resultArray[count] = record;
            }

            return resultArray;
        }

        /// <summary>
        /// Gets the record.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>Return record.</returns>
        /// <exception cref="System.ArgumentException">Incorrect value {nameof(position)}, it is must be multiples to RecordSize - {RecordSize}.</exception>
        public FileCabinetRecord GetRecord(long position)
        {
            if (position % RecordSize != 0)
            {
                throw new ArgumentException($"Incorrect value {nameof(position)}, it is must be multiples to RecordSize - {RecordSize}");
            }

            byte[] buffer = new byte[RecordSize];
            this.fileStream.Seek(position, SeekOrigin.Begin);
            this.fileStream.Read(buffer);

            var record = new FileCabinetRecord
            {
                Status = BitConverter.ToInt16(buffer.AsSpan()[..2]),
                Id = BitConverter.ToInt32(buffer.AsSpan()[2..6]),
                FirstName = ByteConverter.ToString(buffer[6..126]),
                LastName = ByteConverter.ToString(buffer[126..246]),
                DateOfBirth = ByteConverter.ToDateTime(buffer[246..258]),
                WorkingHoursPerWeek = BitConverter.ToInt16(buffer.AsSpan()[258..260]),
                AnnualIncome = ByteConverter.ToDecimal(buffer[260..276]),
                DriverLicenseCategory = BitConverter.ToChar(buffer.AsSpan()[276..278]),
            };

            return record;
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

            var tuple = this.FindRecordById(id);
            if (tuple.Item2 is null)
            {
                throw new ArgumentException($"Record {id} not found.");
            }

            long offset = tuple.Item1;
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
        public Tuple<int, int> GetStat()
        {
            return new Tuple<int, int>(this.recordsCount, this.deletedRecordsCount);
        }

        /// <summary>
        /// Finds the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Return the FileCabinetRecord.
        /// </returns>
        public IEnumerable<FileCabinetRecord> FindById(int id)
        {
            var tuple = this.FindRecordById(id);
            yield return tuple.Item2;
        }

        /// <summary>
        /// Finds the records by the first name.
        /// </summary>
        /// <param name="value">The first name.</param>
        /// <returns>
        /// Return array of records.
        /// </returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            foreach (int index in this.FieldOffsetDictionary["value"])
            {
                byte[] buffer = new byte[120];
                this.fileStream.Seek(index, SeekOrigin.Begin);
                this.fileStream.Read(buffer);

                string name = ByteConverter.ToString(buffer);

                if (name.Equals(value, StringComparison.OrdinalIgnoreCase) && !this.IsDeleted((long)(index - RecordParametersOffsets.FirstName)))
                {
                    var record = this.GetRecord((int)(index - RecordParametersOffsets.FirstName));

                    yield return record;
                }
            }
        }

        /// <summary>
        /// Finds the records by the last name.
        /// </summary>
        /// <param name="value">The last name.</param>
        /// <returns>
        /// Return array of records.
        /// </returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            foreach (int index in this.FieldOffsetDictionary["value"])
            {
                byte[] buffer = new byte[120];
                this.fileStream.Seek(index, SeekOrigin.Begin);
                this.fileStream.Read(buffer);

                string name = ByteConverter.ToString(buffer);

                if (name.Equals(value, StringComparison.OrdinalIgnoreCase) && !this.IsDeleted((long)(index - RecordParametersOffsets.LastName)))
                {
                    var record = this.GetRecord((int)(index - RecordParametersOffsets.LastName));

                    yield return record;
                }
            }
        }

        /// <summary>
        /// Finds the records by date of birthday.
        /// </summary>
        /// <param name="value">The date of birthday.</param>
        /// <returns>
        /// Return array of records.
        /// </returns>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirthday(DateTime value)
        {
            foreach (int index in this.FieldOffsetDictionary["birthDay"])
            {
                byte[] buffer = new byte[sizeof(int) * 3];
                this.fileStream.Seek(index, SeekOrigin.Begin);
                this.fileStream.Read(buffer);

                var dateOfBd = ByteConverter.ToDateTime(buffer);

                if (dateOfBd == value && !this.IsDeleted((long)(index - RecordParametersOffsets.DateOfBirth)))
                {
                    var record = this.GetRecord((int)(index - RecordParametersOffsets.DateOfBirth));

                    yield return record;
                }
            }
        }

        /// <summary>
        /// Finds the records by the last name.
        /// </summary>
        /// <param name="workingHours">The working hours.</param>
        /// <returns>
        /// Return array of records.
        /// </returns>
        public IEnumerable<FileCabinetRecord> FindByWorkingHours(short workingHours)
        {
            foreach (int index in this.FieldOffsetDictionary["workingHours"])
            {
                byte[] buffer = new byte[sizeof(short)];
                this.fileStream.Seek(index, SeekOrigin.Begin);
                this.fileStream.Read(buffer);

                var hours = BitConverter.ToInt16(buffer.AsSpan());

                if (hours == workingHours && !this.IsDeleted((long)(index - RecordParametersOffsets.WorkingHours)))
                {
                    var record = this.GetRecord((int)(index - RecordParametersOffsets.WorkingHours));

                    yield return record;
                }
            }
        }

        /// <summary>
        /// Finds the records by the last name.
        /// </summary>
        /// <param name="annualIncome">The annual income.</param>
        /// <returns>
        /// Return array of records.
        /// </returns>
        public IEnumerable<FileCabinetRecord> FindByAnnualIncome(decimal annualIncome)
        {
            foreach (int index in this.FieldOffsetDictionary["annualIncome"])
            {
                byte[] buffer = new byte[sizeof(decimal)];
                this.fileStream.Seek(index, SeekOrigin.Begin);
                this.fileStream.Read(buffer);

                var income = ByteConverter.ToDecimal(buffer);

                if (income == annualIncome && !this.IsDeleted((long)(index - RecordParametersOffsets.AnnualIncome)))
                {
                    var record = this.GetRecord((long)(index - RecordParametersOffsets.AnnualIncome));

                    yield return record;
                }
            }
        }

        /// <summary>
        /// Finds the records by the last name.
        /// </summary>
        /// <param name="driverCategory">The driver category.</param>
        /// <returns>
        /// Return array of records.
        /// </returns>
        public IEnumerable<FileCabinetRecord> FindByDriverCategory(char driverCategory)
        {
            foreach (int index in this.FieldOffsetDictionary["driverCategory"])
            {
                byte[] buffer = new byte[sizeof(char)];
                this.fileStream.Seek(index, SeekOrigin.Begin);
                this.fileStream.Read(buffer);

                var income = BitConverter.ToChar(buffer.AsSpan());

                if (income == driverCategory && !this.IsDeleted((long)(index - RecordParametersOffsets.DriverCategory)))
                {
                    var record = this.GetRecord((long)(index - RecordParametersOffsets.DriverCategory));

                    yield return record;
                }
            }
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
                var length = this.recordsCount * RecordSize;
                byte[] buffer = new byte[length];

                this.fileStream.SetLength(length);
                this.fileStream.Seek(0, SeekOrigin.Begin);
                this.fileStream.Write(buffer);
                for (var i = 0; i < snapShotRecords.Count; i++)
                {
                    this.WriteRecord(i * RecordSize, snapShotRecords[i]);
                }
            }
        }

        /// <summary>
        /// Removes the record from FileCabinetApp.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void RemoveRecord(int id)
        {
            if (id < 1)
            {
                throw new ArgumentException("The value should be greater than zero.", nameof(id));
            }

            if (this.recordsCount < 1)
            {
                throw new ArgumentException("There are no records.");
            }

            if (this.fileStream == null)
            {
                return;
            }

            try
            {
                var tuple = this.FindRecordById(id);
                if (tuple.Item2 != null)
                {
                    this.fileStream.Seek(tuple.Item1, SeekOrigin.Begin);
                    this.fileStream.Write(BitConverter.GetBytes((short)1)); // 0 - not deleted, 1 - deleted
                    this.recordsCount--;
                    this.deletedRecordsCount++;
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Removing voids in the data file formed by deleted records.
        /// </summary>
        /// <returns>
        /// Return purged Count.
        /// </returns>
        public int Purge()
        {
            var currentSize = this.recordsCount * RecordSize;
            var deletedCount = (int)(this.fileStream.Length - currentSize) / RecordSize;
            if (deletedCount == 0)
            {
                return 0;
            }

            try
            {
                if (this.fileStream != null)
                {
                    List<FileCabinetRecord> currentRecordsList = this.GetRecords().ToList();

                    this.fileStream.SetLength(currentSize);

                    for (int i = 0; i < this.recordsCount; i++)
                    {
                        this.WriteRecord(i * RecordSize, currentRecordsList[i]);
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return deletedCount;
        }

        private Tuple<long, FileCabinetRecord> FindRecordById(int id)
        {
            foreach (int index in this.FieldOffsetDictionary["id"])
            {
                byte[] buffer = new byte[sizeof(int)];
                this.fileStream.Seek(index, SeekOrigin.Begin);
                this.fileStream.Read(buffer);

                var recordId = BitConverter.ToInt32(buffer.AsSpan());

                if (recordId == id)
                {
                    long position = (long)(index - RecordParametersOffsets.Id);
                    var record = this.GetRecord(position);

                    return new Tuple<long, FileCabinetRecord>(position, record);
                }
            }

            return new Tuple<long, FileCabinetRecord>(this.recordsCount * RecordSize, null);
        }

        private bool IsDeleted(long position)
        {
            byte[] statusBuffer = new byte[sizeof(short)];
            this.fileStream.Seek(position, SeekOrigin.Begin);
            this.fileStream.Read(statusBuffer);
            var status = BitConverter.ToInt16(statusBuffer.AsSpan());

            return status == 1 ? true : false;
        }

        private Dictionary<string, List<int>> InitializeIndexes()
        {
            long count = this.fileStream.Length / RecordSize;

            var idsIndexes = new List<int>((int)count);
            var firstNameIndexes = new List<int>((int)count);
            var lastNameIndexes = new List<int>((int)count);
            var birthDayIndexes = new List<int>((int)count);
            var workingHoursIndexes = new List<int>((int)count);
            var annualIncomeIndexes = new List<int>((int)count);
            var driverCategoryIndexes = new List<int>((int)count);

            int currentPos = 0;
            for (int i = 0; i < count; i++)
            {
                idsIndexes.Add((int)RecordParametersOffsets.Id + currentPos);
                firstNameIndexes.Add((int)RecordParametersOffsets.FirstName + currentPos);
                lastNameIndexes.Add((int)RecordParametersOffsets.LastName + currentPos);
                birthDayIndexes.Add((int)RecordParametersOffsets.DateOfBirth + currentPos);
                workingHoursIndexes.Add((int)RecordParametersOffsets.WorkingHours + currentPos);
                annualIncomeIndexes.Add((int)RecordParametersOffsets.AnnualIncome + currentPos);
                driverCategoryIndexes.Add((int)RecordParametersOffsets.DriverCategory + currentPos);
                currentPos += RecordSize;
            }

            Dictionary<string, List<int>> offsetDictionary = new Dictionary<string, List<int>>
                                                                 {
                                                                     { "id", idsIndexes },
                                                                     { "value", lastNameIndexes },
                                                                     { "value", firstNameIndexes },
                                                                     { "birthDay", birthDayIndexes },
                                                                     { "workingHours", workingHoursIndexes },
                                                                     { "annualIncome", annualIncomeIndexes },
                                                                     { "driverCategory", driverCategoryIndexes },
                                                                 };

            return offsetDictionary;
        }

        private void WriteRecord(long offset,  FileCabinetRecord record)
        {
            this.fileStream.Seek(offset, SeekOrigin.Begin);
            this.fileStream.Write(BitConverter.GetBytes(record.Status)); // status if 1 - record deleted; if 0 - record is exist
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
