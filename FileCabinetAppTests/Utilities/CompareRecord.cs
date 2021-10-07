using System;
using System.Collections;

using FileCabinetApp;
using FileCabinetApp.Entities;

//// ReSharper disable InconsistentNaming
namespace FileCabinetAppTests.Utilities
{
    public partial class FileCabinetServiceTests
    {
        public class CompareRecord : IComparer
        {
            public int Compare(object x, object y)
            {
                if (ReferenceEquals(x, null))
                {
                    if (ReferenceEquals(y, null))
                    {
                        return 0;
                    }

                    return -1;
                }

                if (x is FileCabinetRecord lhs && y is FileCabinetRecord rhs)
                {
                    int firstName = string.Compare(lhs.FirstName, rhs.FirstName, StringComparison.Ordinal);
                    int lastName = string.Compare(lhs.LastName, rhs.LastName, StringComparison.Ordinal);
                    int bankAccountType = lhs.DriverLicenseCategory.CompareTo(rhs.DriverLicenseCategory);
                    int bonuses = lhs.WorkingHoursPerWeek.CompareTo(rhs.WorkingHoursPerWeek);
                    int money = lhs.AnnualIncome.CompareTo(rhs.AnnualIncome);
                    int dateOfBirth = lhs.DateOfBirth.CompareTo(rhs.DateOfBirth);

                    if (firstName == 0 && lastName == 0 && bankAccountType == 0 && bonuses == 0 && money == 0 && dateOfBirth == 0)
                    {
                        return 0;
                    }
                }
                else
                {
                    throw new ArgumentException("Can't compare two objects");
                }

                return -1;
            }
        }
    }
}