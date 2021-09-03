using System;
using System.Collections.Generic;
using System.Globalization;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, short workingHoursPerWeek, decimal annualIncome, char driverLicenseCategory)
        {
            if (firstName is null)
            {
                throw new ArgumentNullException(nameof(firstName), "can not be null");
            }

            if (firstName.Length < 2 || firstName.Length > 60)
            {
                throw new ArgumentException("First name must to have from 2 to 60 characters.");
            }

            if (lastName is null)
            {
                throw new ArgumentNullException(nameof(lastName), "can not be null");
            }

            if (lastName.Length < 2 || lastName.Length > 60)
            {
                throw new ArgumentException("Last name must to have from 2 to 60 characters.");
            }

            if (dateOfBirth < DateTime.Parse("01-Jan-1950", CultureInfo.CurrentCulture) || dateOfBirth > DateTime.Now)
            {
                throw new ArgumentException("The minimum date is January 01, 1950, the maximum date is the current one.");
            }

            if (workingHoursPerWeek > 40 || workingHoursPerWeek < 0)
            {
                throw new ArgumentException("The minimum hours is 1 hour, the maximum hours is the 40 according to the Labor Code of the RB.");
            }

            if (annualIncome < 0)
            {
                throw new ArgumentException("The Annual income must be bigger than 0.");
            }

            var driverLicenseCategoryUpper = char.ToUpper(driverLicenseCategory, CultureInfo.CreateSpecificCulture("en-US"));

            if (!(driverLicenseCategoryUpper == 'A' || driverLicenseCategoryUpper == 'B' || driverLicenseCategoryUpper == 'C' || driverLicenseCategoryUpper == 'D'))
            {
                throw new ArgumentException("The Driver License Category can be only - A, B, C, D.");
            }

            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                WorkingHoursPerWeek = workingHoursPerWeek,
                AnnualIncome = annualIncome,
                DriverLicenseCategory = driverLicenseCategory,
            };

            this.list.Add(record);

            return record.Id;
        }

        public FileCabinetRecord[] GetRecords()
        {
            FileCabinetRecord[] resultArray = this.list.ToArray();
            return resultArray;
        }

        public int GetStat()
        {
            return this.list.Count;
        }
    }
}