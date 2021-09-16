using System;

namespace FileCabinetApp.Entities
{
    /// <summary>
    /// Class <c>ParametersContainer</c> parsed and validate string values.
    /// </summary>
    public class ParametersContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParametersContainer"/> class.
        /// </summary>
        public ParametersContainer()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParametersContainer"/> class.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="birthDay">The birth day.</param>
        /// <param name="workingHours">The working hours.</param>
        /// <param name="annualIncome">The annual income.</param>
        /// <param name="driverLicense">The driver license.</param>
        public ParametersContainer(string firstName, string lastName, DateTime birthDay, short workingHours, decimal annualIncome, char driverLicense)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.DateOfBirthday = birthDay;
            this.WorkingHoursPerWeek = workingHours;
            this.AnnualIncome = annualIncome;
            this.DriverLicenseCategory = driverLicense;
        }

        /// <summary>
        /// Gets or sets the date of birthday.
        /// </summary>
        /// <value>
        /// The date of birthday.
        /// </value>
        public DateTime DateOfBirthday { get; set; }

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