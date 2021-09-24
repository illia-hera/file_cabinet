using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using FileCabinetApp.Entities;
using FileCabinetApp.Validators;
using FileCabinetApp.Validators.RecordValidator;

namespace FileCabinetApp.Readers
{
    /// <summary>
    /// Class FileCabinetRecordCsvReader.
    /// </summary>
    public class FileCabinetRecordCsvReader
    {
        private readonly StreamReader reader;

        private readonly IRecordValidator inputValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvReader"/> class.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public FileCabinetRecordCsvReader(StreamReader reader)
        {
            this.reader = reader;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvReader"/> class.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="inputValidator">The input validator.</param>
        public FileCabinetRecordCsvReader(StreamReader reader, IRecordValidator inputValidator)
            : this(reader)
        {
            this.inputValidator = inputValidator;
        }

        /// <summary>
        /// Reads all.
        /// </summary>
        /// <returns>Return List of FileCabinetRecords.</returns>
        public IList<FileCabinetRecord> ReadAll()
        {
            List<FileCabinetRecord> records = new List<FileCabinetRecord>();

            using (this.reader)
            {
                while (!this.reader.EndOfStream)
                {
                    var line = this.reader.ReadLine();
                    if (line != null && !line.Equals("Id,Firstname,Last name,Date of birthday,Working hours per week,Annual income,Driver license category", StringComparison.Ordinal))
                    {
                        var values = line.Split(',', StringSplitOptions.RemoveEmptyEntries);

                        ParametersContainer container = null;

                        try
                        {
                            container = new ParametersContainer(
                                values[1],
                                values[2],
                                Convert.ToDateTime(values[3], DateTimeFormatInfo.InvariantInfo),
                                Convert.ToInt16(values[4], CultureInfo.InvariantCulture),
                                Convert.ToDecimal(values[5], NumberFormatInfo.InvariantInfo),
                                Convert.ToChar(values[6], CultureInfo.InvariantCulture));

                            this.inputValidator.ValidateParameters(container);
                        }
                        catch (Exception e) when (e is ArgumentException)
                        {
                            Console.WriteLine($"id #{Convert.ToInt32(values[0], CultureInfo.InvariantCulture)}, {e.Message}");
                            continue;
                        }

                        var record = new FileCabinetRecord
                        {
                            Id = Convert.ToInt32(values[0], CultureInfo.InvariantCulture),
                            FirstName = container.FirstName,
                            LastName = container.LastName,
                            DateOfBirth = container.DateOfBirthday,
                            WorkingHoursPerWeek = container.WorkingHoursPerWeek,
                            AnnualIncome = container.AnnualIncome,
                            DriverLicenseCategory = container.DriverLicenseCategory,
                        };
                        records.Add(record);
                    }
                }
            }

            return records;
        }
    }
}