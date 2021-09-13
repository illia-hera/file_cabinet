using FileCabinetApp.Validators.Validation_rules;

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
    }
}
