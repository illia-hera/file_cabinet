using System;
using System.Collections.Generic;

namespace FileCabinetApp.Validators.Validation_rules
{
    /// <summary>
    /// Abstract validation rules.
    /// </summary>
    public abstract class ValidationRules
    {
        /// <summary>Gets the minimum annual income.</summary>
        /// <value>The minimum annual income.</value>
        public abstract int MinAnnualIncome { get; }

        /// <summary>Gets the maximum annual income.</summary>
        /// <value>The maximum annual income.</value>
        public abstract int MaxAnnualIncome { get; }

        /// <summary>Gets the maximum name of the length last.</summary>
        /// <value>The maximum name of the length last.</value>
        public abstract int MaxLengthLastName { get; }

        /// <summary>Gets the minimum name of the length last.</summary>
        /// <value>The minimum name of the length last.</value>
        public abstract int MinLengthLastName { get; }

        /// <summary>Gets the maximum name of the length first.</summary>
        /// <value>The maximum name of the length first.</value>
        public abstract int MaxLengthFirstName { get; }

        /// <summary>Gets the minimum name of the length first.</summary>
        /// <value>The minimum name of the length first.</value>
        public abstract int MinLengthFirstName { get; }

        /// <summary>Gets the minimum working hours per week.</summary>
        /// <value>The minimum working hours per week.</value>
        public abstract int MinWorkingHoursPerWeek { get; }

        /// <summary>Gets the maximum working hours per week.</summary>
        /// <value>The maximum working hours per week.</value>
        public abstract int MaxWorkingHoursPerWeek { get; }

        /// <summary>Gets the minimum date of birth.</summary>
        /// <value>The minimum date of birth.</value>
        public abstract DateTime MinDateOfBirth { get; }

        /// <summary>Gets the maximum date of birth.</summary>
        /// <value>The maximum date of birth.</value>
        public abstract DateTime MaxDateOfBirth { get; }

        /// <summary>Gets the actual categories.</summary>
        /// <value>The actual categories.</value>
        public abstract List<char> ActualCategories { get; }
    }
}