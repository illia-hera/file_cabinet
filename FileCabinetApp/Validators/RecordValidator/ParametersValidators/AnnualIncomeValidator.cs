using System;
using FileCabinetApp.Entities;

namespace FileCabinetApp.Validators.RecordValidator.ParametersValidators
{
    /// <summary>
    /// Validation Class.
    /// </summary>
    /// <seealso cref="IRecordValidator" />
    public class AnnualIncomeValidator : IRecordValidator
    {
        private readonly int min;

        private readonly int max;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnnualIncomeValidator"/> class.
        /// </summary>
        /// <param name="max">The maximum.</param>
        /// <param name="min">The minimum.</param>
        public AnnualIncomeValidator(int max, int min)
        {
            this.max = max;
            this.min = min;
        }

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

            if (container.AnnualIncome < this.min || container.AnnualIncome > this.max)
            {
                throw new ArgumentException($"The Annual income min value - {this.min}, max value - {this.max}.");
            }
        }
    }
}
