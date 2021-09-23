using System;
using System.Globalization;
using System.IO;
using FileCabinetApp.Entities;
using FileCabinetApp.Validators.ValidationRule;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Interface <c>IRecordValidator</c>.
    /// </summary>
    public interface IRecordValidator
    {
        /// <summary>
        /// Validates the parameters.
        /// </summary>
        /// <param name="container">The container.</param>
        void ValidateParameters(ParametersContainer container);
    }
}