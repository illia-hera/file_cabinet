﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();

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
            this.AddRecordToFirstNameDict(firstName, record);
            this.AddRecordToLastNameDict(lastName, record);
            this.AddRecordToDateOfBirthDict(dateOfBirth, record);

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

            this.EditFirstNameDictionary(firstName, record);
            this.EditLastNameDictionary(lastName, record);
            this.EditDateOfBirthDictionary(dateOfBirth, record);

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
            FileCabinetRecord[] resultArray = this.firstNameDictionary.ContainsKey(firstName) ? this.firstNameDictionary[firstName].ToArray() : Array.Empty<FileCabinetRecord>();

            return resultArray;
        }

        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            FileCabinetRecord[] resultArray = this.lastNameDictionary.ContainsKey(lastName) ? this.lastNameDictionary[lastName].ToArray() : Array.Empty<FileCabinetRecord>();

            return resultArray;
        }

        public FileCabinetRecord[] FindByDateOfBirthName(DateTime dateOfBirth)
        {
            FileCabinetRecord[] resultArray = this.dateOfBirthDictionary.ContainsKey(dateOfBirth) ? this.dateOfBirthDictionary[dateOfBirth].ToArray() : Array.Empty<FileCabinetRecord>();

            return resultArray;
        }

        private static void ValidateParameters(string firstName, string lastName, DateTime dateOfBirth, short workingHoursPerWeek, decimal annualIncome, char driverLicenseCategory)
        {
            if (firstName is null)
            {
                throw new ArgumentNullException(nameof(firstName), "can not be null");
            }

            const int maxLengthFirstName = 60;
            const int minLengthFirstName = 2;
            if (firstName.Length < minLengthFirstName || firstName.Length > maxLengthFirstName)
            {
                throw new ArgumentException("First name must to have from 2 to 60 characters.");
            }

            if (lastName is null)
            {
                throw new ArgumentNullException(nameof(lastName), "can not be null");
            }

            const int maxLengthLastName = 60;
            const int minLengthLastName = 2;
            if (lastName.Length < minLengthLastName || lastName.Length > maxLengthLastName)
            {
                throw new ArgumentException("Last name must to have from 2 to 60 characters.");
            }

            if (dateOfBirth < DateTime.Parse("01-Jan-1950", CultureInfo.CurrentCulture) || dateOfBirth > DateTime.Now)
            {
                throw new ArgumentException("The minimum date is January 01, 1950, the maximum date is the current one.");
            }

            const int minWorkingHoursPerWeek = 0;
            const int maxWorkingHoursPerWeek = 40;
            if (workingHoursPerWeek > maxWorkingHoursPerWeek || workingHoursPerWeek < minWorkingHoursPerWeek)
            {
                throw new ArgumentException("The minimum hours is 1 hour, the maximum hours is the 40 according to the Labor Code of the RB.");
            }

            const int minAnnualIncome = 1000;
            if (annualIncome < minAnnualIncome)
            {
                throw new ArgumentException("The Annual income must be bigger than 0.");
            }

            var driverLicenseCategoryUpper = char.ToUpper(driverLicenseCategory, CultureInfo.CurrentCulture);

            if (!(driverLicenseCategoryUpper == 'A' || driverLicenseCategoryUpper == 'B' || driverLicenseCategoryUpper == 'C' || driverLicenseCategoryUpper == 'D'))
            {
                throw new ArgumentException("The Driver License Category can be only - A, B, C, D.");
            }
        }

        private void AddRecordToFirstNameDict(string firstName, FileCabinetRecord record)
        {
            if (this.firstNameDictionary.ContainsKey(firstName))
            {
                this.firstNameDictionary[firstName].Add(record);
            }
            else
            {
                this.firstNameDictionary.Add(firstName, new List<FileCabinetRecord>() { record });
            }
        }

        private void EditFirstNameDictionary(string firstName, FileCabinetRecord record)
        {
            if (!firstName.Equals(record.FirstName, StringComparison.OrdinalIgnoreCase))
            {
                this.firstNameDictionary[record.FirstName].Remove(record);
                if (this.firstNameDictionary[record.FirstName].Count == 0)
                {
                    this.firstNameDictionary.Remove(record.FirstName);
                }

                this.AddRecordToFirstNameDict(firstName, record);
            }
        }

        private void AddRecordToLastNameDict(string lastName, FileCabinetRecord record)
        {
            if (this.lastNameDictionary.ContainsKey(lastName))
            {
                this.lastNameDictionary[lastName].Add(record);
            }
            else
            {
                this.lastNameDictionary.Add(lastName, new List<FileCabinetRecord>() { record });
            }
        }

        private void EditLastNameDictionary(string lastName, FileCabinetRecord record)
        {
            if (!lastName.Equals(record.FirstName, StringComparison.OrdinalIgnoreCase))
            {
                this.lastNameDictionary[record.FirstName].Remove(record);
                if (this.lastNameDictionary[record.FirstName].Count == 0)
                {
                    this.lastNameDictionary.Remove(record.FirstName);
                }

                this.AddRecordToLastNameDict(lastName, record);
            }
        }

        private void AddRecordToDateOfBirthDict(DateTime dateOfBirth, FileCabinetRecord record)
        {
            if (this.dateOfBirthDictionary.ContainsKey(dateOfBirth))
            {
                this.dateOfBirthDictionary[dateOfBirth].Add(record);
            }
            else
            {
                this.dateOfBirthDictionary.Add(dateOfBirth, new List<FileCabinetRecord>() { record });
            }
        }

        private void EditDateOfBirthDictionary(DateTime dateOfBirth, FileCabinetRecord record)
        {
            if (dateOfBirth != record.DateOfBirth)
            {
                this.dateOfBirthDictionary[dateOfBirth].Remove(record);
                if (this.dateOfBirthDictionary[dateOfBirth].Count == 0)
                {
                    this.dateOfBirthDictionary.Remove(dateOfBirth);
                }

                this.AddRecordToDateOfBirthDict(dateOfBirth, record);
            }
        }
    }
}