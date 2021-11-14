using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCabinetApp.Entities.JsonSerialization;
using Microsoft.Extensions.Configuration;

namespace FileCabinetApp.Readers
{
    /// <summary>
    /// Class RulesReader.
    /// </summary>
    public static class RulesReader
    {
        /// <summary>
        /// Reads the specified rules.
        /// </summary>
        /// <param name="rules">The rules.</param>
        /// <param name="path">The path.</param>
        /// <returns>
        /// Return ValidationRules.
        /// </returns>
        public static ValidationRules Read(string rules, string path)
        {
            IConfigurationRoot configurationRoot = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(path).Build();
            var root = configurationRoot.GetSection(rules);
            var validationRules = root.Get<ValidationRules>();
            return validationRules;
        }
    }
}
