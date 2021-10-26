using System;
using System.Security.Cryptography;
using FileCabinetApp.Entities.JsonSerialization;
using FileCabinetApp.Readers;
using RandomNameGeneratorLibrary;

namespace FileCabinetGenerator.RandomGenerator
{
    /// <summary>
    /// Random value generator.
    /// </summary>
    public static class Randomizer
    {
        private static ValidationRules Rules { get; set; }

        /// <summary>
        /// Randoms the day.
        /// </summary>
        /// <returns>Return random DateTime.</returns>
        public static DateTime RandomDateOfBirth()
        {
            IsRuleSet();

            int range = (Rules.DateOfBirth.To - Rules.DateOfBirth.From).Days;
            return Rules.DateOfBirth.From.AddDays(RandomNumberGenerator.GetInt32(range));
        }

        /// <summary>
        /// Randoms the annual income.
        /// </summary>
        /// <returns>Return annualIncome.</returns>
        public static decimal RandomAnnualIncome()
        {
            IsRuleSet();
            return new decimal(RandomNumberGenerator.GetInt32(Rules.AnnualIncome.Min, Rules.AnnualIncome.Max));
        }

        /// <summary>
        /// Randoms the working hours.
        /// </summary>
        /// <returns>Return random working hours.</returns>
        public static short RandomWorkingHours()
        {
            IsRuleSet();
            return (short)RandomNumberGenerator.GetInt32(Rules.WorkingHoursPerWeek.Min, Rules.WorkingHoursPerWeek.Max);
        }

        /// <summary>
        /// Randoms the first name.
        /// </summary>
        /// <returns>Return random first name.</returns>
        public static string RandomFirstName()
        {
            var personGenerator = new PersonNameGenerator();
            string value;
            do
            {
                value = personGenerator.GenerateRandomFirstName();
            }
            while (value.Length < Rules.LastName.Min || value.Length > Rules.LastName.Max);

            return value;
        }

        /// <summary>
        /// Randoms the last name.
        /// </summary>
        /// <returns>Return Random last name.</returns>
        public static string RandomLastName()
        {
            var personGenerator = new PersonNameGenerator();
            string value;
            do
            {
                value = personGenerator.GenerateRandomLastName();
            }
            while (value.Length < Rules.LastName.Min || value.Length > Rules.LastName.Max);

            return value;
        }


        /// <summary>
        /// Randoms the driver category.
        /// </summary>
        /// <returns>Return random category.</returns>
        public static char RandomDriverCategory()
        {
            IsRuleSet();
            char[] actualCategories = Rules.DriverCategories.ActualCategories;
            int countActualCategories = actualCategories.Length;

            return actualCategories[RandomNumberGenerator.GetInt32(0, countActualCategories)];
        }

        /// <summary>
        /// Creates the rules.
        /// </summary>
        /// <param name="rules">The rules.</param>
        public static void CreateRules(string rules)
        {
            Rules = RulesReader.Read(rules, @"C:\Users\prast\Desktop\file_cabinet\FileCabinetApp\bin\Debug\net5.0\validation-rules.json");
        }

        private static void IsRuleSet()
        {
            if (Rules is null)
            {
                throw new ArgumentNullException($"{nameof(Rules)} is not set.");
            }
        }
    }
}
