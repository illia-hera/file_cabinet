using FileCabinetApp.Entities;

namespace FileCabinetApp.Validators.RecordValidator
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