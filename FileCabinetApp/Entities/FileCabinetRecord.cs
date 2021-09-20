using System;

namespace FileCabinetApp.Entities
{
    /// <summary>
    /// Class <c>FileCabinetRecord</c> describe the user of File Cabinet.
    /// </summary>
    public class FileCabinetRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecord"/> class.
        /// </summary>
        public FileCabinetRecord()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecord"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public FileCabinetRecord(ParametersContainer container)
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            this.FirstName = container.FirstName;
            this.LastName = container.LastName;
            this.DateOfBirth = container.DateOfBirthday;
            this.WorkingHoursPerWeek = container.WorkingHoursPerWeek;
            this.AnnualIncome = container.AnnualIncome;
            this.DriverLicenseCategory = container.DriverLicenseCategory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecord"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="id">The identifier.</param>
        public FileCabinetRecord(ParametersContainer container, int id)
            : this(container)
        {
            this.Id = id;
        }

        /// <summary>Initializes a new instance of the <see cref="FileCabinetRecord"/> class.</summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <param name="workingHours">The workingHours.</param>
        /// <param name="annualIncome">The annualIncome.</param>
        /// <param name="driverCategory">Type of the account.</param>
        public FileCabinetRecord(string firstName, string lastName, DateTime dateOfBirth, short workingHours, decimal annualIncome, char driverCategory)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.DateOfBirth = dateOfBirth;
            this.WorkingHoursPerWeek = workingHours;
            this.AnnualIncome = annualIncome;
            this.DriverLicenseCategory = driverCategory;
        }

        /// <summary>Initializes a new instance of the <see cref="FileCabinetRecord"/> class.</summary>
        /// <param name="id">The identifier.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <param name="workingHours">The workingHours.</param>
        /// <param name="annualIncome">The annualIncome.</param>
        /// <param name="driverCategory">Type of the account.</param>
        public FileCabinetRecord(int id, string firstName, string lastName, DateTime dateOfBirth, short workingHours, decimal annualIncome, char driverCategory)
            : this(firstName, lastName, dateOfBirth, workingHours, annualIncome, driverCategory)
        {
            this.Id = id;
        }

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