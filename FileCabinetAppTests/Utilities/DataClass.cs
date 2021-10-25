using System;
using System.Collections;
using System.Collections.Generic;

using FileCabinetApp;
using FileCabinetApp.Entities;

using NUnit.Framework;

//// ReSharper disable InconsistentNaming
namespace FileCabinetAppTests.Utilities
{
    public partial class FileCabinetServiceTests
    {
        public class DataClass
        {
            public static IEnumerable ArgumentExceptionTestCases
            {
                get
                {
                    yield return new TestCaseData("Petr", "p", new DateTime(1994, 10, 10), (short)45, 100.300m, 'A');
                    yield return new TestCaseData("p", "Semenov", new DateTime(1994, 10, 10), (short)45, 100.300m, 'A');
                    yield return new TestCaseData("p", "s", new DateTime(1994, 10, 10), (short)45, 100.300m, 'A');
                    yield return new TestCaseData("Petsr", "Semenov", new DateTime(1400, 1, 1), (short)45, 100.300m, 'A');
                    yield return new TestCaseData("Petr3", "Semenov", new DateTime(2100, 1, 1), (short)45, 10000.300m, 'A');
                    yield return new TestCaseData("Petssr", "Semenov", new DateTime(1990, 1, 1), (short)20000, 1100.300m, 'f');
                    yield return new TestCaseData("Petrs", "Semenov", new DateTime(1990, 1, 1), (short)1000, 1100.300m, '1');
                    yield return new TestCaseData("Petrs", "Semenov", new DateTime(1990, 1, 1), (short)1000, -1100.300m, 'A');
                }
            }

            public static IEnumerable ArgumentNullExceptionTestCases
            {
                get
                {
                    yield return new TestCaseData(null, null, new DateTime(1994, 10, 10), (short)45, 100.300m, 'A');
                    yield return new TestCaseData(null, "Semenov", new DateTime(1994, 10, 10), (short)45, 100.300m, 'A');
                    yield return new TestCaseData("Petr", null, new DateTime(1994, 10, 10), (short)45, 100.300m, 'A');

                    yield return new TestCaseData("    ", "    ", new DateTime(1994, 10, 10), (short)45, 100.300m, 'A');
                    yield return new TestCaseData("    ", "Semenov", new DateTime(1994, 10, 10), (short)45, 100.300m, 'A');
                    yield return new TestCaseData("Petr", "    ", new DateTime(1994, 10, 10), (short)45, 100.300m, 'A');
                }
            }

            public class ProductComparer : IEqualityComparer<FileCabinetRecord>
            {
                public bool Equals(FileCabinetRecord x, FileCabinetRecord y)
                {
                    if (ReferenceEquals(x, null))
                    {
                        if (ReferenceEquals(y, null))
                        {
                            return true;
                        }

                        return false;
                    }

                    if (x.DriverLicenseCategory.Equals(y.DriverLicenseCategory)
                        && x.WorkingHoursPerWeek.Equals(y.WorkingHoursPerWeek)
                        && x.DateOfBirth.Equals(y.DateOfBirth)
                        && x.FirstName.Equals(y.FirstName)
                        && x.LastName.Equals(y.LastName)
                        && x.AnnualIncome.Equals(y.AnnualIncome))
                    {
                        return true;
                    }

                    return false;
                }

                public int GetHashCode(FileCabinetRecord obj)
                {
                    return obj.GetHashCode();
                }
            }
        }
    }
}