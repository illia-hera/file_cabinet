namespace FileCabinetApp
{
    /// <summary>
    /// Custom validator for person parameters.
    /// </summary>
    /// <seealso cref="FileCabinetApp.IRecordValidator" />
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
