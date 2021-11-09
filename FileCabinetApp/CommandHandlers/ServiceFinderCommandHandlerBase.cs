using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCabinetApp.Entities;
using FileCabinetApp.Readers;
using FileCabinetApp.Services;
using FileCabinetApp.Utility;
using FileCabinetApp.Validators.InputValidators;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Class <c>ServiceFinderCommandHandlerBase</c>.
    /// </summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.CabinetServiceCommandHandlerBase" />
    public class ServiceFinderCommandHandlerBase : CabinetServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceFinderCommandHandlerBase"/> class.
        /// </summary>
        /// <param name="fileCabinetService">The file cabinet service.</param>
        public ServiceFinderCommandHandlerBase(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceFinderCommandHandlerBase"/> class.
        /// </summary>
        /// <param name="validator">The validator.</param>
        /// <param name="fileCabinetService">The file cabinet service.</param>
        public ServiceFinderCommandHandlerBase(InputValidator validator, IFileCabinetService fileCabinetService)
            : this(fileCabinetService)
        {
            this.InputValidator = validator;
        }

        /// <summary>
        /// Gets the input validator.
        /// </summary>
        /// <value>
        /// The input validator.
        /// </value>
        protected InputValidator InputValidator { get; }

        /// <summary>
        /// Gets the records.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="parameterValue">The parameter value.</param>
        /// <returns>
        /// Return file cabinet records.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">key - can not be null.</exception>
        protected virtual IEnumerable<FileCabinetRecord> GetRecordsBy(string key, string parameterValue)
        {
            if (key is null)
            {
                throw new ArgumentNullException(nameof(key), "can not be null");
            }

            object value = key.ToUpperInvariant() switch
            {
                "ID" => ParameterReaders.ReadInput(parameterValue, Converter.IntConverter, InputValidator.IdValidator),
                "FIRSTNAME" => ParameterReaders.ReadInput(parameterValue, Converter.StringConverter, this.InputValidator.FirstNameValidator),
                "LASTNAME" => ParameterReaders.ReadInput(parameterValue, Converter.StringConverter, this.InputValidator.LastNameValidator),
                "DATEOFBIRTH" => ParameterReaders.ReadInput(parameterValue, Converter.DateConverter, this.InputValidator.DateOfBirthValidator),
                "WORKINGHOURS" => ParameterReaders.ReadInput(parameterValue, Converter.ShortConverter, this.InputValidator.WorkingHoursValidator),
                "ANNUALINCOME" => ParameterReaders.ReadInput(parameterValue, Converter.DecimalConverter, this.InputValidator.AnnualIncomeValidator),
                "DRIVERCATEGORY" => ParameterReaders.ReadInput(parameterValue, Converter.CharConverter, this.InputValidator.DriverLicenseCategoryValidator),
                _ => throw new ArgumentException(
                    "Error in field name. Possible field naming options: id, firstname, lastname, accountType, bonuses, dateofbirth, money.")
            };

            return this.FileCabinetService.FindBy(key, value);
        }
    }
}
