using System;

namespace FileCabinetApp.e
{
    /// <summary>
    /// Class <c>FileCabinetRecord</c> describe the user of File Cabinet.
    /// </summary>
    public class FileCabinetRecord
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the date of birth.
        /// </summary>
        /// <value>
        /// The date of birthday.
        /// </value>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the working hours per week.
        /// </summary>
        /// <value>
        /// The working hours per week.
        /// </value>
        public short WorkingHoursPerWeek { get; set; }

        /// <summary>
        /// Gets or sets the annual income.
        /// </summary>
        /// <value>
        /// The annual income.
        /// </value>
        public decimal AnnualIncome { get; set; }

        /// <summary>
        /// Gets or sets the driver license category.
        /// </summary>
        /// <value>
        /// The driver license category.
        /// </value>
        public char DriverLicenseCategory { get; set; }
    }
}
