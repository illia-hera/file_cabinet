using System;
using System.Collections.Generic;
using FileCabinetApp.Entities;
using Randomizer;
using RandomNameGeneratorLibrary;

namespace FileCabinetGenerator
{
    /// <summary>
    /// Generate random value.
    /// </summary>
    public static class RandomRecordsGenerator
    {
        /// <summary>
        /// Generates the specified records amount.
        /// </summary>
        /// <param name="recordsAmount">The records amount.</param>
        /// <param name="startId">The start identifier.</param>
        /// <returns>Return FileCabinetRecords.</returns>
        public static List<FileCabinetRecord> Generate(int recordsAmount, int startId)
        {
            var records = new List<FileCabinetRecord>(recordsAmount);

            var personGenerator = new PersonNameGenerator();

            var datetimeGenerator = new RandomDateTimeGenerator();

            var shortGenerator = new RandomShortGenerator();

            var decimalGenerator = new RandomDecimalGenerator();

            var charGenerator = new RandomAlphanumericCharGenerator();

            for (int i = 0; i < recordsAmount; i++)
            {
                string firstName = personGenerator.GenerateRandomFirstName();
                string lastName = personGenerator.GenerateRandomLastName();
                DateTime birthDay = datetimeGenerator.GenerateValue(new DateTime(1950, 1, 1), DateTime.Now.Date);
                short workingHours = shortGenerator.GenerateValue(0, 40);
                decimal annualIncome = decimalGenerator.GenerateValue(0, 1_000m);
                char driverLicense = charGenerator.GenerateValue('A', 'D');

                records.Add(CreateRecord(startId, new ParametersContainer(firstName, lastName, birthDay, workingHours, annualIncome, driverLicense)));
            }

            return records;
        }

        private static FileCabinetRecord CreateRecord(int startId, ParametersContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            return new FileCabinetRecord(container, startId + 1);
        }
    }
}