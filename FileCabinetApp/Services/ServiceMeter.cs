using System;
using System.Collections.Generic;
using System.Diagnostics;
using FileCabinetApp.Entities;
using FileCabinetApp.Services.SnapshotServices;

namespace FileCabinetApp.Services
{
    /// <summary>
    /// Class ServiceMeter.
    /// </summary>
    /// <seealso cref="FileCabinetApp.Services.IFileCabinetService" />
    public class ServiceMeter : IFileCabinetService
    {
        private readonly IFileCabinetService fileCabinetServiceImplementation;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceMeter"/> class.
        /// </summary>
        /// <param name="fileCabinetServiceImplementation">The file cabinet service implementation.</param>
        public ServiceMeter(IFileCabinetService fileCabinetServiceImplementation)
        {
            this.fileCabinetServiceImplementation = fileCabinetServiceImplementation;
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
            var sw = new Stopwatch();

            sw.Start();

            int result = this.fileCabinetServiceImplementation.CreateRecord(container);

            sw.Stop();

            Console.WriteLine($"Create method execution duration is {sw.ElapsedTicks} ticks.");

            return result;
        }

        /// <summary>
        /// Inserts the record.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>
        /// Return is record inserted.
        /// </returns>
        public bool InsertRecord(ParametersContainer container)
        {
            var sw = new Stopwatch();

            sw.Start();

            var result = this.fileCabinetServiceImplementation.InsertRecord(container);

            sw.Stop();

            Console.WriteLine($"Create method execution duration is {sw.ElapsedTicks} ticks.");

            return result;
        }

        /// <summary>
        /// Gets all records in File Cabinet.
        /// </summary>
        /// <returns>
        /// Return array of <c>FileCabinetRecord</c>.
        /// </returns>
        public IReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            var sw = new Stopwatch();

            sw.Start();

            var result = this.fileCabinetServiceImplementation.GetRecords();

            sw.Stop();

            Console.WriteLine($"GetRecords method execution duration is {sw.ElapsedTicks} ticks.");

            return result;
        }

        /// <summary>
        /// Edits the record.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="container">The container of parameters.</param>
        public void EditRecord(int id, ParametersContainer container)
        {
            var sw = new Stopwatch();

            sw.Start();

            this.fileCabinetServiceImplementation.EditRecord(id, container);

            sw.Stop();

            Console.WriteLine($"Edit method execution duration is {sw.ElapsedTicks} ticks.");
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
            var sw = new Stopwatch();

            sw.Start();

            var result = this.fileCabinetServiceImplementation.FindById(id);

            sw.Stop();

            Console.WriteLine($"Edit method execution duration is {sw.ElapsedTicks} ticks.");

            return result;
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
            var sw = new Stopwatch();

            sw.Start();

            var result = this.fileCabinetServiceImplementation.FindByFirstName(value);

            sw.Stop();

            Console.WriteLine($"FindByFirstName method execution duration is {sw.ElapsedTicks} ticks.");

            return result;
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
            var sw = new Stopwatch();

            sw.Start();

            var result = this.fileCabinetServiceImplementation.FindByLastName(value);

            sw.Stop();

            Console.WriteLine($"FindByLastName method execution duration is {sw.ElapsedTicks} ticks.");

            return result;
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
            var sw = new Stopwatch();

            sw.Start();

            var result = this.fileCabinetServiceImplementation.FindByDateOfBirthday(value);

            sw.Stop();

            Console.WriteLine($"FindByDateOfBirth method execution duration is {sw.ElapsedTicks} ticks.");

            return result;
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
            var sw = new Stopwatch();

            sw.Start();

            var result = this.fileCabinetServiceImplementation.FindByWorkingHours(workingHours);

            sw.Stop();

            Console.WriteLine($"FindByDateOfBirth method execution duration is {sw.ElapsedTicks} ticks.");

            return result;
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
            var sw = new Stopwatch();

            sw.Start();

            var result = this.fileCabinetServiceImplementation.FindByAnnualIncome(annualIncome);

            sw.Stop();

            Console.WriteLine($"FindByDateOfBirth method execution duration is {sw.ElapsedTicks} ticks.");

            return result;
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
            var sw = new Stopwatch();

            sw.Start();

            var result = this.fileCabinetServiceImplementation.FindByDriverCategory(driverCategory);

            sw.Stop();

            Console.WriteLine($"FindByDateOfBirth method execution duration is {sw.ElapsedTicks} ticks.");

            return result;
        }

        /// <summary>
        /// Gets the stat of users.
        /// </summary>
        /// <returns>
        /// Return count of records and deleted records in File Cabinet.
        /// </returns>
        public Tuple<int, int> GetStat()
        {
            var sw = new Stopwatch();

            sw.Start();

            var result = this.fileCabinetServiceImplementation.GetStat();

            sw.Stop();

            Console.WriteLine($"GetStat method execution duration is {sw.ElapsedTicks} ticks.");

            return result;
        }

        /// <summary>
        /// Makes the snapshot.
        /// </summary>
        /// <returns>
        /// Return <c>FileCabinetServiceSnapshot</c>.
        /// </returns>
        public IFileCabinetServiceSnapshot MakeSnapshot()
        {
            var sw = new Stopwatch();

            sw.Start();

            var result = this.fileCabinetServiceImplementation.MakeSnapshot();

            sw.Stop();

            Console.WriteLine($"MakeSnapshot method execution duration is {sw.ElapsedTicks} ticks.");

            return result;
        }

        /// <summary>
        /// Removes the record from FileCabinetApp.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void RemoveRecord(int id)
        {
            var sw = new Stopwatch();

            sw.Start();

            this.fileCabinetServiceImplementation.RemoveRecord(id);

            sw.Stop();

            Console.WriteLine($"RemoveRecord method execution duration is {sw.ElapsedTicks} ticks.");
        }

        /// <summary>
        /// Restores the specified snapshot.
        /// </summary>
        /// <param name="snapshot">The snapshot.</param>
        public void Restore(IFileCabinetServiceSnapshot snapshot)
        {
            var sw = new Stopwatch();

            sw.Start();

            this.fileCabinetServiceImplementation.Restore(snapshot);

            sw.Stop();

            Console.WriteLine($"Restore method execution duration is {sw.ElapsedTicks} ticks.");
        }

        /// <summary>
        /// Removing voids in the data file formed by deleted records.
        /// </summary>
        /// <returns>
        /// Return purged Count.
        /// </returns>
        public int Purge()
        {
            var sw = new Stopwatch();

            sw.Start();

            var result = this.fileCabinetServiceImplementation.Purge();

            sw.Stop();

            Console.WriteLine($"Restore method execution duration is {sw.ElapsedTicks} ticks.");

            return result;
        }
    }
}
