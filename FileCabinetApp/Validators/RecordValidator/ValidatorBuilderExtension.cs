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
        /// Creates the default validator.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>Return Record Validator.</returns>
        public static IRecordValidator CreateDefaultValidator(this ValidatorBuilder builder)
        {
            return CreateRecordValidator(builder, "default");
        }

        /// <summary>
        /// Creates the custom validator.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>Return Record Validator.</returns>
        public static IRecordValidator CreateCustomValidator(this ValidatorBuilder builder)
        {
            return CreateRecordValidator(builder, "custom");
        }

        /// <summary>
        /// Creates the record validator.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="type">The type.</param>
        /// <returns>
        /// Return Record Validator.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">builder
        /// or
        /// configuration.</exception>
        private static IRecordValidator CreateRecordValidator(ValidatorBuilder builder, string type)
        {
            IConfigurationRoot configurationRoot = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("validation-rules.json").Build();
            ValidationRules configuration = configurationRoot.GetSection(type).Get<ValidationRules>();

            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.ValidateFirstName(configuration.FirstName.Max, configuration.FirstName.Min)
                .ValidateLastName(configuration.LastName.Max, configuration.LastName.Min)
                .ValidateDateOfBirth(configuration.DateOfBirth.To, configuration.DateOfBirth.From)
                .ValidateWorkingHours(configuration.WorkingHoursPerWeek.Max, configuration.WorkingHoursPerWeek.Min)
                .ValidateAnnualIncome(configuration.AnnualIncome.Max, configuration.AnnualIncome.Min)
                .ValidateDriverCategory(configuration.DriverCategories.ActualCategories)
                .Create();
        }
    }
}
