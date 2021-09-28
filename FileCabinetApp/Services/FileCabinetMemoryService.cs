using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FileCabinetApp.Entities;
using FileCabinetApp.Services.SnapshotServices;
using FileCabinetApp.Utility.Iterator;
using FileCabinetApp.Validators.RecordValidator;

namespace FileCabinetApp.Services
{
    /// <summary>
    /// Class <c>FileCabinetMemoryService</c> with File Cabinet.
    /// </summary>
    public class FileCabinetMemoryService : IFileCabinetService
    {
        private readonly IRecordValidator validator;
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetMemoryService"/> class.
        /// </summary>
        /// <param name="validator">The validator.</param>
        public FileCabinetMemoryService(IRecordValidator validator)
        {
            this.validator = validator;
        }

        /// <summary>
        /// Creates the record.
        /// </summary>
        /// <param name="container">The container of parameters.</param>
        /// <returns>Return Id of created record.</returns>
        public int CreateRecord(ParametersContainer container)
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            this.validator.ValidateParameters(container);
            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = container.FirstName,
                LastName = container.LastName,
                DateOfBirth = container.DateOfBirthday,
                WorkingHoursPerWeek = container.WorkingHoursPerWeek,
                AnnualIncome = container.AnnualIncome,
                DriverLicenseCategory = char.ToUpper(container.DriverLicenseCategory, CultureInfo.InvariantCulture),
            };

            this.list.Add(record);
            AddRecordToDict(container.FirstName, record, this.firstNameDictionary);
            AddRecordToDict(container.LastName, record, this.lastNameDictionary);
            AddRecordToDict(container.DateOfBirthday, record, this.dateOfBirthDictionary);

            return record.Id;
        }

        /// <summary>
        /// Edits the record.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="container">The container of parameters.</param>
        /// <exception cref="System.ArgumentException">#{id} record is not found.</exception>
        /// <exception cref="System.ArgumentNullException">container.</exception>
        public void EditRecord(int id, ParametersContainer container)
        {
            var record = this.list.FirstOrDefault(r => r.Id == id);
            if (record == null)
            {
                throw new ArgumentException($"#{id} record is not found.");
            }

            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            this.validator.ValidateParameters(container);

            this.EditFirstNameDictionary(container.FirstName, record);
            this.EditLastNameDictionary(container.LastName, record);
            this.EditDateOfBirthDictionary(container.DateOfBirthday, record);

            record.FirstName = container.FirstName;
            record.LastName = container.LastName;
            record.AnnualIncome = container.AnnualIncome;
            record.DateOfBirth = container.DateOfBirthday;
            record.DriverLicenseCategory = container.DriverLicenseCategory;
            record.WorkingHoursPerWeek = container.WorkingHoursPerWeek;
        }

        /// <summary>
        /// Gets all records in File Cabinet.
        /// </summary>
        /// <returns>Return array of <c>FileCabinetRecord</c>.</returns>
        public IReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            IReadOnlyCollection<FileCabinetRecord> recordsCollection = this.list.ToArray();
            return recordsCollection;
        }

        /// <summary>
        /// Gets the stat of users.
        /// </summary>
        /// <returns>Return count of records in File Cabinet.</returns>
        public Tuple<int, int> GetStat()
        {
            return new Tuple<int, int>(this.list.Count, 0);
        }

        /// <summary>
        /// Finds the records by the first name.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <returns>Return array of records.</returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            if (this.firstNameDictionary.ContainsKey(firstName))
            {
                return new MemoryEnumerable(this.firstNameDictionary[firstName]);
            }

            return new MemoryEnumerable(new List<FileCabinetRecord>());
        }

        /// <summary>
        /// Finds the records by the last name.
        /// </summary>
        /// <param name="lastName">The last name.</param>
        /// <returns>Return array of records.</returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            if (this.lastNameDictionary.ContainsKey(lastName))
            {
                return new MemoryEnumerable(this.lastNameDictionary[lastName]);
            }

            return new MemoryEnumerable(new List<FileCabinetRecord>());
        }

        /// <summary>
        /// Finds the records by date of birthday.
        /// </summary>
        /// <param name="dateOfBirth">The date of birthday.</param>
        /// <returns>Return array of records.</returns>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirthday(DateTime dateOfBirth)
        {
            if (this.dateOfBirthDictionary.ContainsKey(dateOfBirth))
            {
                return new MemoryEnumerable(this.dateOfBirthDictionary[dateOfBirth]);
            }

            return new MemoryEnumerable(new List<FileCabinetRecord>());
        }

        /// <summary>
        /// Makes the snapshot.
        /// </summary>
        /// <returns>Return <c>FileCabinetServiceSnapshot</c>.</returns>
        public IFileCabinetServiceSnapshot MakeSnapshot()
        {
            var snapshot = new FileCabinetServiceSnapshot(this.list.ToArray(), this.validator);

            return snapshot;
        }

        /// <summary>
        /// Restores the specified snapshot.
        /// </summary>
        /// <param name="snapshot">The snapshot.</param>
        /// <exception cref="System.ArgumentException">Snapshot can't be null.</exception>
        public void Restore(IFileCabinetServiceSnapshot snapshot)
        {
            if (snapshot == null)
            {
                throw new ArgumentException("Snapshot can't be null");
            }

            this.firstNameDictionary.Clear();
            this.lastNameDictionary.Clear();
            this.dateOfBirthDictionary.Clear();

            foreach (FileCabinetRecord record in snapshot.Records)
            {
                FileCabinetRecord match = this.list.Find(x => x.Id.Equals(record.Id));
                if (match != null)
                {
                    this.list[this.list.IndexOf(match)] = record;
                }

                AddRecordToDict(record.FirstName, record, this.firstNameDictionary);
                AddRecordToDict(record.LastName, record, this.lastNameDictionary);
                AddRecordToDict(record.DateOfBirth, record, this.dateOfBirthDictionary);

                this.list.Add(record);
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

            if (this.list.Count < 1)
            {
                throw new ArgumentException("There are no records.");
            }

            bool noMatch = true;
            foreach (FileCabinetRecord record in this.list)
            {
                if (record.Id == id)
                {
                    this.list.Remove(record);

                    DeleteRecordFromDictionary(record.FirstName, record, this.firstNameDictionary);
                    DeleteRecordFromDictionary(record.LastName, record, this.lastNameDictionary);
                    DeleteRecordFromDictionary(record.DateOfBirth, record, this.dateOfBirthDictionary);

                    noMatch = false;
                    break;
                }
            }

            if (noMatch)
            {
                throw new ArgumentException($"Record #{id} doesn't exist.");
            }
        }

        /// <summary>
        /// Removing voids in the data file formed by deleted records.
        /// </summary>
        /// <returns>
        /// Return deleted Count.
        /// </returns>
        public int Purge()
        {
            return 0;
        }

        private static void DeleteRecordFromDictionary<T>(T key, FileCabinetRecord record, IDictionary<T, List<FileCabinetRecord>> dictionary)
        {
            dictionary[key].Remove(record);
        }

        private static void AddRecordToDict<T>(T parameter, FileCabinetRecord record, Dictionary<T, List<FileCabinetRecord>> dictionary)
        {
            if (dictionary.ContainsKey(parameter))
            {
                dictionary[parameter].Add(record);
            }
            else
            {
                dictionary.Add(parameter, new List<FileCabinetRecord>() { record });
            }
        }

        private void EditFirstNameDictionary(string firstName, FileCabinetRecord record)
        {
            if (!firstName.Equals(record.FirstName, StringComparison.OrdinalIgnoreCase))
            {
                if (this.firstNameDictionary.ContainsKey(record.FirstName))
                {
                    this.firstNameDictionary[record.FirstName].Remove(record);
                    if (this.firstNameDictionary[record.FirstName].Count == 0)
                    {
                        this.firstNameDictionary.Remove(record.FirstName);
                    }
                }

                AddRecordToDict(firstName, record, this.firstNameDictionary);
            }
        }

        private void EditLastNameDictionary(string lastName, FileCabinetRecord record)
        {
            if (!lastName.Equals(record.FirstName, StringComparison.OrdinalIgnoreCase))
            {
                if (this.lastNameDictionary.ContainsKey(record.LastName))
                {
                    this.lastNameDictionary[record.LastName].Remove(record);
                    if (this.lastNameDictionary[record.LastName].Count == 0)
                    {
                        this.lastNameDictionary.Remove(record.LastName);
                    }
                }

                AddRecordToDict(lastName, record, this.lastNameDictionary);
            }
        }

        private void EditDateOfBirthDictionary(DateTime dateOfBirth, FileCabinetRecord record)
        {
            if (dateOfBirth != record.DateOfBirth)
            {
                if (this.dateOfBirthDictionary.ContainsKey(record.DateOfBirth))
                {
                    this.dateOfBirthDictionary[record.DateOfBirth].Remove(record);
                    if (this.dateOfBirthDictionary[record.DateOfBirth].Count == 0)
                    {
                        this.dateOfBirthDictionary.Remove(record.DateOfBirth);
                    }
                }

                AddRecordToDict(dateOfBirth, record, this.dateOfBirthDictionary);
            }
        }
    }
}