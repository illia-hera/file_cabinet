using System;
using System.Globalization;
using FileCabinetApp.Entities;
using FileCabinetApp.Validators.ValidationRule;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Custom validator for person parameters.
    /// </summary>
    /// <seealso cref="IRecordValidator" />
    public class CustomValidator : IRecordValidator
    {
        /// <summary>
        /// Gets the validation rules.
        /// </summary>
        /// <value>
        /// The validation rules.
        /// </value>
        public ValidationRules ValidationRules { get; } = new ValidationCustomRules();

        /// <summary>
        /// Validates the parameters.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <exception cref="System.ArgumentNullException">container</exception>
        public void ValidateParameters(ParametersContainer container)
        {

            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            this.ValidateFirstName(container);
            this.ValidateLastName(container);
            this.ValidateDateOfBirth(container);
            this.ValidateWorkingHours(container);
            this.ValidateAnnualIncome(container);
            this.ValidateDriverCategory(container);
        }

        private void ValidateFirstName(ParametersContainer container)
        {
            if (container.FirstName is null)
            {
                throw new ArgumentNullException(nameof(container), $"{container.FirstName}can not be null");
            }

            if (container.FirstName.Length < this.ValidationRules.MinLengthFirstName || container.FirstName.Length > this.ValidationRules.MaxLengthFirstName)
            {
                throw new ArgumentException($"First name must to have from {this.ValidationRules.MinLengthFirstName} to {this.ValidationRules.MaxLengthFirstName} characters.");
            }
        }

        private void ValidateLastName(ParametersContainer container)
        {
            if (container.LastName is null)
            {
                throw new ArgumentNullException(nameof(container), $"{container.LastName}can not be null");
            }

            if (container.LastName.Length < this.ValidationRules.MinLengthLastName || container.LastName.Length > this.ValidationRules.MaxLengthLastName)
            {
                throw new ArgumentException($"Last name must to have from {this.ValidationRules.MinLengthLastName} to {this.ValidationRules.MaxLengthLastName} characters.");
            }
        }

        private void ValidateDateOfBirth(ParametersContainer container)
        {
            if (container.DateOfBirthday < this.ValidationRules.MinDateOfBirth || container.DateOfBirthday > this.ValidationRules.MaxDateOfBirth)
            {
                throw new ArgumentException($"The minimum date is {this.ValidationRules.MinDateOfBirth}, the maximum date is {this.ValidationRules.MaxDateOfBirth}.");
            }
        }

        private void ValidateWorkingHours(ParametersContainer container)
        {
            if (container.WorkingHoursPerWeek > this.ValidationRules.MaxWorkingHoursPerWeek || container.WorkingHoursPerWeek < this.ValidationRules.MinWorkingHoursPerWeek)
            {
                throw new ArgumentException($"The minimum hours is {this.ValidationRules.MinWorkingHoursPerWeek} hour, the maximum hours is the {this.ValidationRules.MaxWorkingHoursPerWeek}.");
            }
        }

        private void ValidateAnnualIncome(ParametersContainer container)
        {
            if (container.AnnualIncome < this.ValidationRules.MinAnnualIncome || container.AnnualIncome > this.ValidationRules.MaxAnnualIncome)
            {
                throw new ArgumentException($"The Annual income min value - {this.ValidationRules.MinAnnualIncome}, max value - {this.ValidationRules.MaxAnnualIncome}.");
            }
        }

        private void ValidateDriverCategory(ParametersContainer container)
        {
            if (!this.ValidationRules.ActualCategories.Contains(char.ToUpper(container.DriverLicenseCategory, CultureInfo.CurrentCulture)))
            {
                throw new ArgumentException($"The Driver License Category can be only - {this.ValidationRules.ActualCategories}");
            }
        }
    }
}
