using System.Globalization;
using System.IO;
using FileCabinetApp.Entities;

namespace FileCabinetApp.RecordWriters
{
    /// <summary>
    /// Class <c>FileCabinetRecordCsvWriter</c> write in to the file.
    /// </summary>
    public class FileCabinetRecordCsvWriter : IFileCabinetRecordWriter
    {
        private readonly TextWriter textWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvWriter"/> class.
        /// </summary>
        /// <param name="textWriter">The text text writer.</param>
        public FileCabinetRecordCsvWriter(TextWriter textWriter)
        {
            this.textWriter = textWriter;
        }

        /// <summary>
        /// Writes the specified record.
        /// </summary>
        /// <param name="record">The record.</param>
        public void Write(FileCabinetRecord record)
        {
            if (record != null)
            {
                this.textWriter.WriteLine($"{record.Id}," +
                                          $"{record.FirstName}," +
                                          $"{record.LastName}," +
                                          $"{record.DateOfBirth.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)}," +
                                          $"{record.WorkingHoursPerWeek}," +
                                          $"{record.AnnualIncome}," +
                                          $"{record.DriverLicenseCategory}");
            }
        }

        /// <summary>
        /// Writes the header.
        /// </summary>
        public void WriteHeader()
        {
            this.textWriter.WriteLine("Id," +
                                      "Firstname," +
                                      "Last name," +
                                      "Date of birthday," +
                                      "Working hours per week," +
                                      "Annual income," +
                                      "Driver license category");
        }
    }
}
