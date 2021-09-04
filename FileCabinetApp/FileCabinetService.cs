using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, short workingHoursPerWeek, decimal annualIncome, char driverLicenseCategory)
        {
            ValidateParameters(firstName, lastName, dateOfBirth, workingHoursPerWeek, annualIncome, driverLicenseCategory);
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

        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, short workingHoursPerWeek, decimal annualIncome, char driverLicenseCategory)
        {
            var record = this.list.FirstOrDefault(r => r.Id == id);
            if (record == null)
            {
                throw new ArgumentException($"#{id} record is not found.");
            }

            ValidateParameters(firstName, lastName, dateOfBirth, workingHoursPerWeek, annualIncome, driverLicenseCategory);

            record.FirstName = firstName;
            record.LastName = lastName;
            record.AnnualIncome = annualIncome;
            record.DateOfBirth = dateOfBirth;
            record.DriverLicenseCategory = driverLicenseCategory;
            record.WorkingHoursPerWeek = workingHoursPerWeek;
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

        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            FileCabinetRecord[] resultArray = this.list.Where(r =>
                firstName.Equals(r.FirstName, StringComparison.OrdinalIgnoreCase)).ToArray();

            return resultArray;
        }

        private static void ValidateParameters(string firstName, string lastName, DateTime dateOfBirth, short workingHoursPerWeek, decimal annualIncome, char driverLicenseCategory)
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

            var driverLicenseCategoryUpper = char.ToUpper(driverLicenseCategory, CultureInfo.CurrentCulture);

            if (!(driverLicenseCategoryUpper == 'A' || driverLicenseCategoryUpper == 'B' || driverLicenseCategoryUpper == 'C' || driverLicenseCategoryUpper == 'D'))
            {
                throw new ArgumentException("The Driver License Category can be only - A, B, C, D.");
            }
        }
    }
}