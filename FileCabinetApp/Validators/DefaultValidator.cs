﻿using FileCabinetApp.Validators.ValidationRule;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Default validator for person parameters.
    /// </summary>
    /// <seealso cref="IRecordValidator" />
    public class DefaultValidator : IRecordValidator
    {
        /// <summary>
        /// Gets the validation rules.
        /// </summary>
        /// <value>
        /// The validation rules.
        /// </value>
        public ValidationRules ValidationRules { get; } = new ValidationDefaultRules();
    }
}