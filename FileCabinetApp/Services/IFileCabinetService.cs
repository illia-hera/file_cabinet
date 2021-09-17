using System;
using System.Collections.Generic;
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
        public int CreateRecord(ParametersContainer container);

        /// <summary>
        /// Edits the record.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="container">The container of parameters.</param>
        public void EditRecord(int id, ParametersContainer container);

        /// <summary>
        /// Gets all records in File Cabinet.
        /// </summary>
        /// <returns>Return array of <c>FileCabinetRecord</c>.</returns>
        public IReadOnlyCollection<FileCabinetRecord> GetRecords();

        /// <summary>
        /// Gets the stat of users.
        /// </summary>
        /// <returns>Return count of records in File Cabinet.</returns>
        public int GetStat();

        /// <summary>
        /// Finds the records by the first name.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <returns>Return array of records.</returns>
        public IReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName);

        /// <summary>
        /// Finds the records by the last name.
        /// </summary>
        /// <param name="lastName">The last name.</param>
        /// <returns>Return array of records.</returns>
        public IReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName);

        /// <summary>
        /// Finds the records by date of birthday.
        /// </summary>
        /// <param name="dateOfBirth">The date of birthday.</param>
        /// <returns>Return array of records.</returns>
        public IReadOnlyCollection<FileCabinetRecord> FindByDateOfBirthName(DateTime dateOfBirth);

        /// <summary>
        /// Makes the snapshot.
        /// </summary>
        /// <returns>Return <c>FileCabinetServiceSnapshot</c>.</returns>
        public IFileCabinetServiceSnapshot MakeSnapshot();

        /// <summary>
        /// Restores the specified snapshot.
        /// </summary>
        /// <param name="snapshot">The snapshot.</param>
        public void Restore(IFileCabinetServiceSnapshot snapshot);
    }
}
