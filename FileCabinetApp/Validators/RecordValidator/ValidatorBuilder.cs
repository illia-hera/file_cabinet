using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCabinetApp.Validators.RecordValidator.ParametersValidators;

namespace FileCabinetApp.Validators.RecordValidator
{
    /// <summary>
    /// Class <c>ValidatorBuilder</c>.
    /// </summary>
    public class ValidatorBuilder
    {
        private readonly List<IRecordValidator> validators = new List<IRecordValidator>();

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns>
        ///   <see cref="CompositeValidator" />.
        /// </returns>
        public CompositeValidator Create()
        {
            return new CompositeValidator(this.validators);
        }

        /// <summary>
        /// Validates the first name.
        /// </summary>
        /// <param name="max">The maximum.</param>
        /// <param name="min">The minimum.</param>
        /// <returns>
        ///   <see cref="ValidatorBuilder" />.
        /// </returns>
        public ValidatorBuilder ValidateFirstName(int max, int min)
        {
            this.validators.Add(new FirstNameValidator(max, min));
            return this;
        }

        /// <summary>
        /// Validates the last name.
        /// </summary>
        /// <param name="max">The maximum.</param>
        /// <param name="min">The minimum.</param>
        /// <returns>
        ///   <see cref="ValidatorBuilder" />.
        /// </returns>
        public ValidatorBuilder ValidateLastName(int max, int min)
        {
            this.validators.Add(new LastNameValidator(max, min));
            return this;
        }

        /// <summary>
        /// Validates the date of birth.
        /// </summary>
        /// <param name="max">The maximum.</param>
        /// <param name="min">The minimum.</param>
        /// <returns>
        ///   <see cref="ValidatorBuilder" />.
        /// </returns>
        public ValidatorBuilder ValidateDateOfBirth(DateTime max, DateTime min)
        {
            this.validators.Add(new DateOfBirthdayValidator(max, min));
            return this;
        }

        /// <summary>
        /// Validates the date of birthday.
        /// </summary>
        /// <param name="max">The maximum.</param>
        /// <param name="min">The minimum.</param>
        /// <returns>
        ///   <see cref="ValidatorBuilder" />.
        /// </returns>
        public ValidatorBuilder ValidateWorkingHours(short max, short min)
        {
            this.validators.Add(new WorkingHoursValidator(max, min));
            return this;
        }

        /// <summary>
        /// Validates the date of birthday.
        /// </summary>
        /// <param name="max">The maximum.</param>
        /// <param name="min">The minimum.</param>
        /// <returns>
        ///   <see cref="ValidatorBuilder" />.
        /// </returns>
        public ValidatorBuilder ValidateAnnualIncome(int max, int min)
        {
            this.validators.Add(new AnnualIncomeValidator(max, min));
            return this;
        }

        /// <summary>
        /// Validates the date of birthday.
        /// </summary>
        /// <param name="actualCategories">The actual categories.</param>
        /// <returns>
        ///   <see cref="ValidatorBuilder" />.
        /// </returns>
        public ValidatorBuilder ValidateDriverCategory(IList<char> actualCategories)
        {
            this.validators.Add(new DriverCategoryValidator(actualCategories));
            return this;
        }
    }
}
