using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCabinetApp.Entities;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Class composite validator.
    /// </summary>
    /// <seealso cref="FileCabinetApp.Validators.IRecordValidator" />
    public class CompositeValidator : IRecordValidator
    {
        private readonly List<IRecordValidator> validators;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeValidator"/> class.
        /// </summary>
        /// <param name="recordValidators">The record validators.</param>
        public CompositeValidator(IEnumerable<IRecordValidator> recordValidators)
        {
            this.validators = recordValidators.ToList();
        }

        /// <summary>
        /// Validates the parameters.
        /// </summary>
        /// <param name="container">The container.</param>
        public void ValidateParameters(ParametersContainer container)
        {
            foreach (IRecordValidator recordValidator in this.validators)
            {
                recordValidator.ValidateParameters(container);
            }
        }
    }
}
