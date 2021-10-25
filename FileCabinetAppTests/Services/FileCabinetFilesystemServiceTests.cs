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
    public class FileCabinetFilesystemServiceTests
    {

        private static readonly FileCabinetFilesystemService FileCabinetService = new FileCabinetFilesystemService(
            new FileStream("cabinet-records.db", FileMode.Create),
            new ValidatorBuilder().CreateCustomValidator());

        [SetUp]
        public void Init()
        {
            FileCabinetService.CreateRecord(new ParametersContainer("Petro", "Semenov", new DateTime(1990, 1, 1), (short)21, 1100.30m, 'A'));
            FileCabinetService.CreateRecord(new ParametersContainer("Andrew", "Semenov", new DateTime(1995, 1, 1), (short)27, 1122.3m, 'A'));
            FileCabinetService.CreateRecord(new ParametersContainer("Petroz", "Pavlov", new DateTime(1994, 10, 1), (short)26, 999m, 'A'));
            FileCabinetService.CreateRecord(new ParametersContainer("Alexey", "Goodboy", new DateTime(1994, 12, 10), (short)23, 1111m, 'B'));
            FileCabinetService.CreateRecord(new ParametersContainer("Alexey", "Badboy", new DateTime(1994, 12, 10), (short)29, 1311m, 'B'));
        }

        [Test]
        public void CreateRecordTest()
        {
            var records = FileCabinetService.GetRecords().ToList();

            FileCabinetRecord expected = new FileCabinetRecord
            {
                FirstName = "Petro",
                LastName = "Semenov",
                DateOfBirth = new DateTime(1990, 1, 1),
                WorkingHoursPerWeek = (short)21,
                AnnualIncome = 1100.300m,
                DriverLicenseCategory = 'A',
                Id = 1
            };

            Assert.That(records[0], Is.EqualTo(expected).Using(new FileCabinetServiceTests.DataClass.ProductComparer()));
        }

        [Test]
        public void GetStatTest()
        {
            var records = FileCabinetService.GetStat();

            Assert.AreEqual(25, records.Item1);
        }

        [Test]
        public void EditRecordTest()
        {
            FileCabinetRecord expected = new FileCabinetRecord
            {
                FirstName = "Semenchik",
                LastName = "Petrovich",
                DriverLicenseCategory = 'B',
                DateOfBirth = new DateTime(1993, 4, 1),
                AnnualIncome = 999m,
                WorkingHoursPerWeek = 24,
                Id = 1
            };

            FileCabinetService.EditRecord(1, new ParametersContainer("Semenchik", "Petrovich", new DateTime(1993, 4, 1), (short)24, 999m, 'B'));

            List<FileCabinetRecord> records = FileCabinetService.GetRecords().ToList();

            Assert.That(records[0], Is.EqualTo(expected).Using(new FileCabinetServiceTests.DataClass.ProductComparer()));
        }

        [Test]
        public void EditRecordTest_UpdateDict_DifferentKey()
        {
            FileCabinetRecord expected = new FileCabinetRecord
            {
                FirstName = "Semenchik",
                LastName = "Petrovich",
                DriverLicenseCategory = 'B',
                DateOfBirth = new DateTime(1993, 4, 1),
                AnnualIncome = 1341m,
                WorkingHoursPerWeek = 22,
                Id = 1
            };

            FileCabinetService.EditRecord(5, new ParametersContainer("Semenchik", "Petrovich", new DateTime(1993, 4, 1), (short)22, 1341m, 'B'));

            var records = FileCabinetService.GetRecords().ToList();

            Assert.That(records[4], Is.EqualTo(expected).Using(new FileCabinetServiceTests.DataClass.ProductComparer()));
        }

        [Test]
        public void EditRecordTest_UpdateDict_SameKey()
        {
            FileCabinetRecord expected = new FileCabinetRecord
            {
                FirstName = "Alexis",
                LastName = "Petrovich",
                DriverLicenseCategory = 'B',
                DateOfBirth = new DateTime(1993, 4, 1),
                AnnualIncome = 999m,
                WorkingHoursPerWeek = 23,
                Id = 1
            };

            FileCabinetService.EditRecord(5, new ParametersContainer("Alexis", "Petrovich", new DateTime(1993, 4, 1), (short)23, 999m, 'B'));

            var records = FileCabinetService.GetRecords().ToList();

            Assert.That(records[4], Is.EqualTo(expected).Using(new FileCabinetServiceTests.DataClass.ProductComparer()));
        }
    }
}