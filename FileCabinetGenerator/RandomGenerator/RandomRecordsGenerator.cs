using System;
using System.Collections.Generic;
using FileCabinetApp.Entities;
using RandomNameGeneratorLibrary;

[assembly: CLSCompliant(false)]

namespace FileCabinetGenerator.RandomGenerator
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
        /// <param name="rules">The rules.</param>
        /// <returns>
        /// Return FileCabinetRecords.
        /// </returns>
        public static IReadOnlyCollection<FileCabinetRecord> Generate(int recordsAmount, int startId, string rules)
        {
            var records = new List<FileCabinetRecord>(recordsAmount);

            var personGenerator = new PersonNameGenerator();
            Randomizer.CreateRules(rules);

            for (int i = 0; i < recordsAmount; i++)
            {
                string firstName = personGenerator.GenerateRandomFirstName();
                string lastName = personGenerator.GenerateRandomLastName();
                DateTime birthDay = Randomizer.RandomDateOfBirth();
                short workingHours = Randomizer.RandomWorkingHours();
                decimal annualIncome = Randomizer.RandomAnnualIncome();
                char driverLicense = Randomizer.RandomDriverCategory();

                records.Add(CreateRecord(startId + i - 1, new ParametersContainer(firstName, lastName, birthDay, workingHours, annualIncome, driverLicense)));
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