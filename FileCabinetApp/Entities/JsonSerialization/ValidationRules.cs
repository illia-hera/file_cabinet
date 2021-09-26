namespace FileCabinetApp.Entities.JsonSerialization
{
    /// <summary>
    /// Validation rules.
    /// </summary>
    public class ValidationRules
    {
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public FirstName FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public LastName LastName { get; set; }

        /// <summary>
        /// Gets or sets the date of birth.
        /// </summary>
        /// <value>
        /// The date of birth.
        /// </value>
        public DateOfBirth DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the working hours per week.
        /// </summary>
        /// <value>
        /// The working hours per week.
        /// </value>
        public WorkingHoursPerWeek WorkingHoursPerWeek { get; set; }

        /// <summary>
        /// Gets or sets the annual income.
        /// </summary>
        /// <value>
        /// The annual income.
        /// </value>
        public AnnualIncome AnnualIncome { get; set; }

        /// <summary>
        /// Gets or sets the driver categories.
        /// </summary>
        /// <value>
        /// The driver categories.
        /// </value>
        public DriverCategories DriverCategories { get; set; }
    }
}
