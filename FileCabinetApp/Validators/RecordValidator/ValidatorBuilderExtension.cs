using System;
using System.Collections.Generic;
using System.IO;
using FileCabinetApp.Entities.JsonSerialization;
using Microsoft.Extensions.Configuration;

namespace FileCabinetApp.Validators.RecordValidator
{
    /// <summary>
    /// Class <c>ValidatorBuilderExtension</c>.
    /// </summary>
    public static class ValidatorBuilderExtension
    {
        /// <summary>
        /// Creates the record validator.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>Return Record Validator.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// builder
        /// or
        /// configuration.
        /// </exception>
        public static IRecordValidator CreateRecordValidator(this ValidatorBuilder builder, ValidationRules configuration)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            return builder.ValidateFirstName(configuration.FirstName.Max, configuration.FirstName.Min)
                .ValidateLastName(configuration.LastName.Max, configuration.LastName.Min)
                .ValidateDateOfBirth(configuration.DateOfBirth.Max, configuration.DateOfBirth.Min)
                .ValidateWorkingHours(configuration.WorkingHoursPerWeek.Max, configuration.WorkingHoursPerWeek.Min)
                .ValidateAnnualIncome(configuration.AnnualIncome.Max, configuration.AnnualIncome.Min)
                .ValidateDriverCategory(configuration.DriverCategories.ActualCategories)
                .Create();
        }
    }
}
