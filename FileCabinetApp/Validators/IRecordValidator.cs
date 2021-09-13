using System;
using System.Globalization;
using FileCabinetApp.Enteties;
using FileCabinetApp.Validators.ValidationRules;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Interface <c>IRecordValidator</c>.
    /// </summary>
    public interface IRecordValidator
    {
        /// <summary>
        /// Gets the validation rules.
        /// </summary>
        /// <value>
        /// The validation rules.
        /// </value>
        public ValidationRules ValidationRules { get; }

        /// <summary>Validates the parameters.</summary>
        /// <param name="container">The container.</param>
        /// <exception cref="System.ArgumentNullException">container
        /// or
        /// container
        /// or
        /// container.</exception>
        /// <exception cref="System.ArgumentException">
        /// First name must to have from {this.ValidationRules.MinLengthFirstName} to {this.ValidationRules.MaxLengthFirstName} characters.
        /// or
        /// Last name must to have from {this.ValidationRules.MinLengthLastName} to {this.ValidationRules.MaxLengthLastName} characters.
        /// or
        /// The minimum date is {this.ValidationRules.MinDateOfBirth}, the maximum date is {this.ValidationRules.MaxDateOfBirth}.
        /// or
        /// The minimum hours is {this.ValidationRules.MinWorkingHoursPerWeek} hour, the maximum hours is the {this.ValidationRules.MaxWorkingHoursPerWeek}.
        /// or
        /// The Annual income min value - {this.ValidationRules.MinAnnualIncome}, max value - {this.ValidationRules.MaxAnnualIncome}.
        /// or
        /// The Driver License Category can be only - {this.ValidationRules.ActualCategories}.
        /// </exception>
        public virtual void ValidateParameters(ParametersContainer container)
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            if (container.FirstName is null)
            {
                throw new ArgumentNullException(nameof(container), $"{container.FirstName}can not be null");
            }

            if (container.FirstName.Length < this.ValidationRules.MinLengthFirstName || container.FirstName.Length > this.ValidationRules.MaxLengthFirstName)
            {
                throw new ArgumentException($"First name must to have from {this.ValidationRules.MinLengthFirstName} to {this.ValidationRules.MaxLengthFirstName} characters.");
            }

            if (container.LastName is null)
            {
                throw new ArgumentNullException(nameof(container), $"{container.LastName}can not be null");
            }

            if (container.LastName.Length < this.ValidationRules.MinLengthLastName || container.LastName.Length > this.ValidationRules.MaxLengthLastName)
            {
                throw new ArgumentException($"Last name must to have from {this.ValidationRules.MinLengthLastName} to {this.ValidationRules.MaxLengthLastName} characters.");
            }

            if (container.DateOfBirthday < this.ValidationRules.MinDateOfBirth || container.DateOfBirthday > this.ValidationRules.MaxDateOfBirth)
            {
                throw new ArgumentException($"The minimum date is {this.ValidationRules.MinDateOfBirth}, the maximum date is {this.ValidationRules.MaxDateOfBirth}.");
            }

            if (container.WorkingHoursPerWeek > this.ValidationRules.MaxWorkingHoursPerWeek || container.WorkingHoursPerWeek < this.ValidationRules.MinWorkingHoursPerWeek)
            {
                throw new ArgumentException($"The minimum hours is {this.ValidationRules.MinWorkingHoursPerWeek} hour, the maximum hours is the {this.ValidationRules.MaxWorkingHoursPerWeek}.");
            }

            if (container.AnnualIncome < this.ValidationRules.MinAnnualIncome || container.AnnualIncome > this.ValidationRules.MaxAnnualIncome)
            {
                throw new ArgumentException($"The Annual income min value - {this.ValidationRules.MinAnnualIncome}, max value - {this.ValidationRules.MaxAnnualIncome}.");
            }

            if (!this.ValidationRules.ActualCategories.Contains(char.ToUpper(container.DriverLicenseCategory, CultureInfo.CurrentCulture)))
            {
                throw new ArgumentException($"The Driver License Category can be only - {this.ValidationRules.ActualCategories}");
            }
        }

        /// <summary>Firsts name validator.</summary>
        /// <param name="firstName">The first name.</param>
        /// <returns>
        ///   Return the result of validation an value.
        /// </returns>
        public Tuple<bool, string> FirstNameValidator(string firstName)
        {
            bool isValid = firstName?.Length >= this.ValidationRules.MinLengthFirstName && firstName.Length <= this.ValidationRules.MaxLengthFirstName;

            return new Tuple<bool, string>(isValid, firstName);
        }

        /// <summary>
        /// Lasts the name validator.
        /// </summary>
        /// <param name="lastName">The last name.</param>
        /// <returns>Return the result of validation an value.</returns>
        public Tuple<bool, string> LastNameValidator(string lastName)
        {
            bool isValid = lastName?.Length >= this.ValidationRules.MinLengthLastName && lastName.Length <= this.ValidationRules.MaxLengthLastName;

            return new Tuple<bool, string>(isValid, lastName);
        }

        /// <summary>
        /// Annuals the income validator.
        /// </summary>
        /// <param name="annualIncome">The annual income.</param>
        /// <returns>Return the result of validation an value.</returns>
        public Tuple<bool, string> AnnualIncomeValidator(decimal annualIncome)
        {
            bool isValid = annualIncome >= this.ValidationRules.MinAnnualIncome && annualIncome <= this.ValidationRules.MaxAnnualIncome;

            return new Tuple<bool, string>(isValid, annualIncome.ToString(CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Workings the hours validator.
        /// </summary>
        /// <param name="workingHours">The working hours.</param>
        /// <returns>Return the result of validation an value.</returns>
        public Tuple<bool, string> WorkingHoursValidator(short workingHours)
        {
            bool isValid = workingHours >= this.ValidationRules.MinWorkingHoursPerWeek && workingHours <= this.ValidationRules.MaxWorkingHoursPerWeek;

            return new Tuple<bool, string>(isValid, workingHours.ToString(CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Drivers the license category validator.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns>Return the result of validation an value.</returns>
        public Tuple<bool, string> DriverLicenseCategoryValidator(char category)
        {
            bool isValid = this.ValidationRules.ActualCategories.Contains(char.ToUpper(category, CultureInfo.CurrentCulture));

            return new Tuple<bool, string>(isValid, category.ToString(CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Dates the of birth validator.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <returns>Return the result of validation an value.</returns>
        public Tuple<bool, string> DateOfBirthValidator(DateTime dateOfBirth)
        {
            bool isValid = dateOfBirth > this.ValidationRules.MinDateOfBirth && dateOfBirth < this.ValidationRules.MaxDateOfBirth;

            return new Tuple<bool, string>(isValid, dateOfBirth.ToString(CultureInfo.CreateSpecificCulture("en-US")));
        }
    }
}