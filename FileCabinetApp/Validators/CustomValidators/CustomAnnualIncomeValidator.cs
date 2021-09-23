using System;
using FileCabinetApp.Entities;
using FileCabinetApp.Validators.ValidationRule;

namespace FileCabinetApp.Validators.CustomValidators
{
    /// <summary>
    /// Validation Class.
    /// </summary>
    /// <seealso cref="FileCabinetApp.Validators.IRecordValidator" />
    public class CustomAnnualIncomeValidator : IRecordValidator
    {
        private ValidationRules ValidationRules { get; } = new ValidationCustomRules();

        /// <summary>
        /// Validates the parameters.
        /// </summary>
        /// <param name="container">The container.</param>
        public void ValidateParameters(ParametersContainer container)
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            if (container.AnnualIncome < this.ValidationRules.MinAnnualIncome || container.AnnualIncome > this.ValidationRules.MaxAnnualIncome)
            {
                throw new ArgumentException($"The Annual income min value - {this.ValidationRules.MinAnnualIncome}, max value - {this.ValidationRules.MaxAnnualIncome}.");
            }
        }
    }
}
