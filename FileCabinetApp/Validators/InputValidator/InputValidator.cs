using System;
using System.Globalization;
using System.IO;
using FileCabinetApp.Validators.InputValidator.ValidationRule;

namespace FileCabinetApp.Validators.InputValidator
{
    /// <summary>
    /// Class <c>InputValidator</c>.
    /// </summary>
    public class InputValidator
    {
        private readonly ValidationRules validationRules;

        /// <summary>
        /// Initializes a new instance of the <see cref="InputValidator"/> class.
        /// </summary>
        /// <param name="rules">The rules.</param>
        public InputValidator(ValidationRules rules)
        {
            this.validationRules = rules;
        }

        /// <summary>
        /// Validates the import export parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Return validation result and value.</returns>
        public static Tuple<bool, string, string> ValidateImportExportParameters(string parameters)
        {
            string[] inputs = parameters?.Split(' ', 2);
            const int parameterIndex = 0;
            const int valueIndex = 1;
            string fileFormat = inputs[parameterIndex];
            string filePath = inputs.Length > 1 ? inputs[valueIndex] : string.Empty;
            int lastIndexOfBackSlash = filePath.LastIndexOf("\\", StringComparison.OrdinalIgnoreCase);
            string fileDirection = lastIndexOfBackSlash > 0 ? filePath[..lastIndexOfBackSlash] : string.Empty;

            if (string.IsNullOrWhiteSpace(filePath) || (!string.IsNullOrEmpty(fileDirection) && (!Directory.Exists(fileDirection))))
            {
                Console.WriteLine($"Export failed: can't open file {filePath}.");
                return new Tuple<bool, string, string>(false, null, null);
            }

            return new Tuple<bool, string, string>(true, filePath, fileFormat);
        }

        /// <summary>Firsts name validator.</summary>
        /// <param name="firstName">The first name.</param>
        /// <returns>
        ///   Return the result of validation an value.
        /// </returns>
        public Tuple<bool, string> FirstNameValidator(string firstName)
        {
            bool isValid = firstName?.Length >= this.validationRules.MinLengthFirstName && firstName?.Length <= this.validationRules.MaxLengthFirstName;

            return new Tuple<bool, string>(isValid, firstName);
        }

        /// <summary>
        /// Lasts the name validator.
        /// </summary>
        /// <param name="lastName">The last name.</param>
        /// <returns>Return the result of validation an value.</returns>
        public Tuple<bool, string> LastNameValidator(string lastName)
        {
            bool isValid = lastName?.Length >= this.validationRules.MinLengthLastName && lastName?.Length <= this.validationRules.MaxLengthLastName;

            return new Tuple<bool, string>(isValid, lastName);
        }

        /// <summary>
        /// Annuals the income validator.
        /// </summary>
        /// <param name="annualIncome">The annual income.</param>
        /// <returns>Return the result of validation an value.</returns>
        public Tuple<bool, string> AnnualIncomeValidator(decimal annualIncome)
        {
            bool isValid = annualIncome >= this.validationRules.MinAnnualIncome && annualIncome <= this.validationRules.MaxAnnualIncome;

            return new Tuple<bool, string>(isValid, annualIncome.ToString(CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Workings the hours validator.
        /// </summary>
        /// <param name="workingHours">The working hours.</param>
        /// <returns>Return the result of validation an value.</returns>
        public Tuple<bool, string> WorkingHoursValidator(short workingHours)
        {
            bool isValid = workingHours >= this.validationRules.MinWorkingHoursPerWeek && workingHours <= this.validationRules.MaxWorkingHoursPerWeek;

            return new Tuple<bool, string>(isValid, workingHours.ToString(CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Drivers the license category validator.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns>Return the result of validation an value.</returns>
        public Tuple<bool, string> DriverLicenseCategoryValidator(char category)
        {
            bool isValid = this.validationRules.ActualCategories.Contains(char.ToUpper(category, CultureInfo.CurrentCulture));

            return new Tuple<bool, string>(isValid, category.ToString(CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Dates the of birth validator.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <returns>Return the result of validation an value.</returns>
        public Tuple<bool, string> DateOfBirthValidator(DateTime dateOfBirth)
        {
            bool isValid = dateOfBirth > this.validationRules.MinDateOfBirth && dateOfBirth < this.validationRules.MaxDateOfBirth;

            return new Tuple<bool, string>(isValid, dateOfBirth.ToString(CultureInfo.CreateSpecificCulture("en-US")));
        }
    }
}
