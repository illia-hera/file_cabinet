using System;
using System.Collections.Generic;
using System.IO;
using FileCabinetApp.Entities;
using FileCabinetApp.Services.SnapshotServices;

namespace FileCabinetApp.Services
{
    /// <summary>
    /// Interface <c>IFileCabinetService</c>.
    /// </summary>
    public interface IFileCabinetService
    {
        /// <summary>
        /// Creates the record.
        /// </summary>
        /// <param name="container">The container of parameters.</param>
        /// <returns>Return Id of created record.</returns>
        int CreateRecord(ParametersContainer container);

        /// <summary>
        /// Edits the record.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="container">The container of parameters.</param>
        void EditRecord(int id, ParametersContainer container);

        /// <summary>
        /// Inserts the record.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>
        /// Return is record inserted.
        /// </returns>
        bool InsertRecord(ParametersContainer container);

        /// <summary>
        /// Gets all records in File Cabinet.
        /// </summary>
        /// <returns>Return array of <c>FileCabinetRecord</c>.</returns>
        IReadOnlyCollection<FileCabinetRecord> GetRecords();

        /// <summary>
        /// Gets the stat of users.
        /// </summary>
        /// <returns>Return count of records and deleted records in File Cabinet.</returns>
        Tuple<int, int> GetStat();

        /// <summary>
        /// Finds the by.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>Return IEnumerable of FileCabinetRecords.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// key - can not be null
        /// or
        /// value - can not be null.
        /// </exception>
        IEnumerable<FileCabinetRecord> FindBy(string key, object value)
        {
            if (key is null)
            {
                throw new ArgumentNullException(nameof(key), "can not be null");
            }

            if (value is null)
            {
                throw new ArgumentNullException(nameof(value), "can not be null");
            }

            return key.ToUpperInvariant() switch
            {
                "ID" => this.FindById((int)value),
                "FIRSTNAME" => this.FindByFirstName((string)value),
                "LASTNAME" => this.FindByLastName((string)value),
                "DATEOFBIRTH" => this.FindByDateOfBirthday((DateTime)value),
                "WORKINGHOURS" => this.FindByWorkingHours((short)value),
                "ANNUALINCOME" => this.FindByAnnualIncome((decimal)value),
                "DRIVERCATEGORY" => this.FindByDriverCategory((char)value),
                _ => throw new ArgumentException("Error in field name. Possible field naming options: id, firstname, lastname, accountType, bonuses, dateofbirth, money.")
            };
        }

        /// <summary>
        /// Finds the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Return the FileCabinetRecord.</returns>
        IEnumerable<FileCabinetRecord> FindById(int id);

        /// <summary>
        /// Finds the records by the first name.
        /// </summary>
        /// <param name="value">The first name.</param>
        /// <returns>Return array of records.</returns>
        IEnumerable<FileCabinetRecord> FindByFirstName(string value);

        /// <summary>
        /// Finds the records by the last name.
        /// </summary>
        /// <param name="value">The last name.</param>
        /// <returns>Return array of records.</returns>
        IEnumerable<FileCabinetRecord> FindByLastName(string value);

        /// <summary>
        /// Finds the records by date of birthday.
        /// </summary>
        /// <param name="value">The date of birthday.</param>
        /// <returns>Return array of records.</returns>
        IEnumerable<FileCabinetRecord> FindByDateOfBirthday(DateTime value);

        /// <summary>
        /// Finds the records by the last name.
        /// </summary>
        /// <param name="workingHours">The working hours.</param>
        /// <returns>
        /// Return array of records.
        /// </returns>
        IEnumerable<FileCabinetRecord> FindByWorkingHours(short workingHours);

        /// <summary>
        /// Finds the records by the last name.
        /// </summary>
        /// <param name="annualIncome">The annual income.</param>
        /// <returns>
        /// Return array of records.
        /// </returns>
        IEnumerable<FileCabinetRecord> FindByAnnualIncome(decimal annualIncome);

        /// <summary>
        /// Finds the records by the last name.
        /// </summary>
        /// <param name="driverCategory">The driver category.</param>
        /// <returns>
        /// Return array of records.
        /// </returns>
        IEnumerable<FileCabinetRecord> FindByDriverCategory(char driverCategory);

        /// <summary>
        /// Makes the snapshot.
        /// </summary>
        /// <returns>Return <c>FileCabinetServiceSnapshot</c>.</returns>
        IFileCabinetServiceSnapshot MakeSnapshot();

        /// <summary>
        /// Restores the specified snapshot.
        /// </summary>
        /// <param name="snapshot">The snapshot.</param>
        void Restore(IFileCabinetServiceSnapshot snapshot);

        /// <summary>
        /// Removes the record from FileCabinetApp.
        /// </summary>
        /// <param name="id">The identifier.</param>
        void RemoveRecord(int id);

        /// <summary>
        /// Removing voids in the data file formed by deleted records.
        /// </summary>
        /// <returns>Return purged Count.</returns>
        int Purge();
    }
}