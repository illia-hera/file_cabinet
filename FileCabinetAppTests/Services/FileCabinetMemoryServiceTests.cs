using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileCabinetApp.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FileCabinetApp.Entities;
using FileCabinetApp.Validators.RecordValidator;
using FileCabinetAppTests.Utilities;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace FileCabinetApp.Services.Tests
{
    [TestFixture]
    public class FileCabinetMemoryServiceTest
    {
        private static readonly FileCabinetMemoryService FileCabinetService = new FileCabinetApp.Services.FileCabinetMemoryService(new ValidatorBuilder().CreateDefaultValidator());

        [SetUp]
        public void Init()
        {
            FileCabinetService.CreateRecord(new ParametersContainer("Petr", "Semenov", new DateTime(1990, 1, 1), (short)22, 1100.30m, 'A'));
            FileCabinetService.CreateRecord(new ParametersContainer("Andrew", "Semenov", new DateTime(1995, 1, 1), (short)22, 1122.3m, 'A'));
            FileCabinetService.CreateRecord(new ParametersContainer("Petr", "Pavlov", new DateTime(1994, 10, 1), (short)22, 9992m, 'A'));
            FileCabinetService.CreateRecord(new ParametersContainer("Alex", "Good", new DateTime(1994, 12, 10), (short)23, 1111m, 'D'));
            FileCabinetService.CreateRecord(new ParametersContainer("Alex", "Bad", new DateTime(1994, 12, 10), (short)12, 1121m, 'B'));
        }


        [TestCaseSource(typeof(FileCabinetServiceTests.DataClass), nameof(FileCabinetServiceTests.DataClass.ArgumentExceptionTestCases))]
        public void CreateRecordTest_ArgumentExceptions(string firstName, string lastName, DateTime dateOfBirth, short WorkingHoursPerWeek, decimal AnnualIncomeValue, char bankDriverLicenseCategory)
        {
            NUnit.Framework.Assert.Throws<ArgumentException>(() => FileCabinetService.CreateRecord(new ParametersContainer(firstName, lastName, dateOfBirth, WorkingHoursPerWeek, AnnualIncomeValue, bankDriverLicenseCategory)));
        }

        [TestCaseSource(typeof(FileCabinetServiceTests.DataClass), nameof(FileCabinetServiceTests.DataClass.ArgumentNullExceptionTestCases))]
        public void CreateRecordTest_ArgumentNullExceptions(string firstName, string lastName, DateTime dateOfBirth, short WorkingHoursPerWeek, decimal AnnualIncomeValue, char bankDriverLicenseCategory)
        {
            Assert.Throws<ArgumentNullException>(() => FileCabinetService.CreateRecord(new ParametersContainer(firstName, lastName, dateOfBirth, WorkingHoursPerWeek, AnnualIncomeValue, bankDriverLicenseCategory)));
        }

        [Test]
        public void EditRecordTest_ArgumentExceptions()
        {
            Assert.Throws<ArgumentException>(() => FileCabinetService.EditRecord(25,
                new ParametersContainer("Petr", "Semenov", new DateTime(1990, 1, 1), (short) 23, -1100.300m, 'A')));
        }

        [Test]
        public void EditRecordTest()
        {
            List<FileCabinetRecord> records = FileCabinetService.GetRecords().ToList();

            FileCabinetRecord expected = new FileCabinetRecord
            {
                FirstName = "Semen",
                LastName = "Petrovich",
                DriverLicenseCategory = 'B',
                DateOfBirth = new DateTime(1993, 4, 1),
                AnnualIncome = 9999m,
                WorkingHoursPerWeek = 32,
                Id = 1
            };

            FileCabinetService.EditRecord(1, new ParametersContainer("Semen", "Petrovich", new DateTime(1993, 4, 1), (short)32, 9999m, 'B'));

            Assert.That(records[0], Is.EqualTo(expected).Using(new FileCabinetServiceTests.DataClass.ProductComparer()));
        }

        [Test]
        public void EditRecordTest_UpdateDict_DifferentKey()
        {
            var records = FileCabinetService.GetRecords().ToList();

            FileCabinetRecord expected = new FileCabinetRecord
            {
                FirstName = "Semen",
                LastName = "Petrovich",
                DriverLicenseCategory = 'B',
                DateOfBirth = new DateTime(1993, 4, 1),
                AnnualIncome = 9999m,
                WorkingHoursPerWeek = 32,
                Id = 1
            };

            FileCabinetService.EditRecord(5, new ParametersContainer("Semen", "Petrovich", new DateTime(1993, 4, 1), (short)32, 9999m, 'B'));

            Assert.That(records[4], Is.EqualTo(expected).Using(new FileCabinetServiceTests.DataClass.ProductComparer()));
        }

        [Test]
        public void EditRecordTest_UpdateDict_SameKey()
        {
            var records = FileCabinetService.GetRecords().ToList();

            FileCabinetRecord expected = new FileCabinetRecord
            {
                FirstName = "Alex",
                LastName = "Petrovich",
                DriverLicenseCategory = 'B',
                DateOfBirth = new DateTime(1993, 4, 1),
                AnnualIncome = 9999m,
                WorkingHoursPerWeek = 32,
                Id = 1
            };

            FileCabinetService.EditRecord(5, new ParametersContainer("Alex", "Petrovich", new DateTime(1993, 4, 1), (short)32, 9999m, 'B'));

            Assert.That(records[4], Is.EqualTo(expected).Using(new FileCabinetServiceTests.DataClass.ProductComparer()));
        }

        [Test]
        public void CreateRecordTest()
        {
            var records = FileCabinetService.GetRecords().ToList();

            FileCabinetRecord expected = new FileCabinetRecord
            {
                FirstName = "Petr",
                LastName = "Semenov",
                DateOfBirth = new DateTime(1990, 1, 1),
                WorkingHoursPerWeek = (short)22,
                AnnualIncome = 1100.300m,
                DriverLicenseCategory = 'A',
                Id = 1
            };

            Assert.That(records[0], Is.EqualTo(expected).Using(new FileCabinetServiceTests.DataClass.ProductComparer()));
        }


        [TestCase("petr")]
        [TestCase("PETR")]
        public void FindByFirstNameRecordTest(string parameter)
        {
            var records = FileCabinetService.FindByFirstName(parameter);

            var actual = FileCabinetService.GetRecords().Where(x => x.FirstName.Equals(parameter, StringComparison.InvariantCultureIgnoreCase));
        }

        [TestCase("semenov")]
        [TestCase("SEMENOV")]
        public void FindByLastNameRecordTest(string parameter)
        {
            var records = FileCabinetService.FindByLastName(parameter);

            var actual = FileCabinetService.GetRecords().Where(x => x.LastName.Equals(parameter, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}