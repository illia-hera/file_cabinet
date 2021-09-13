using System;
using System.Collections.Generic;
using System.Globalization;

namespace FileCabinetApp.Validators.Validation_rules
{
    /// <summary>
    ///   Custom validation rules.
    /// </summary>
    public class ValidationCustomRules : ValidationRules
    {
        /// <summary>Gets the minimum annual income.</summary>
        /// <value>The minimum annual income.</value>
        public override int MinAnnualIncome { get; } = 500;

        /// <summary>Gets the maximum annual income.</summary>
        /// <value>The maximum annual income.</value>
        public override int MaxAnnualIncome { get; } = 1500;

        /// <summary>Gets the maximum name of the length last.</summary>
        /// <value>The maximum name of the length last.</value>
        public override int MaxLengthLastName { get; } = 10;

        /// <summary>Gets the minimum name of the length last.</summary>
        /// <value>The minimum name of the length last.</value>
        public override int MinLengthLastName { get; } = 5;

        /// <summary>Gets the maximum name of the length first.</summary>
        /// <value>The maximum name of the length first.</value>
        public override int MaxLengthFirstName { get; } = 10;

        /// <summary>Gets the minimum name of the length first.</summary>
        /// <value>The minimum name of the length first.</value>
        public override int MinLengthFirstName { get; } = 5;

        /// <summary>Gets the minimum working hours per week.</summary>
        /// <value>The minimum working hours per week.</value>
        public override int MinWorkingHoursPerWeek { get; } = 20;

        /// <summary>Gets the maximum working hours per week.</summary>
        /// <value>The maximum working hours per week.</value>
        public override int MaxWorkingHoursPerWeek { get; } = 30;

        /// <summary>Gets the minimum date of birth.</summary>
        /// <value>The minimum date of birth.</value>
        public override DateTime MinDateOfBirth { get; } = DateTime.Parse("10-Dec-1970", CultureInfo.CreateSpecificCulture("en-US"));

        /// <summary>Gets the maximum date of birth.</summary>
        /// <value>The maximum date of birth.</value>
        public override DateTime MaxDateOfBirth { get; } = DateTime.Now;

        /// <summary>Gets the actual categories.</summary>
        /// <value>The actual categories.</value>
        public override List<char> ActualCategories { get; } = new List<char>() { 'A', 'B' };
    }
}
