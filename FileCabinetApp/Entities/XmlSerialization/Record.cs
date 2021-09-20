using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FileCabinetApp.Entities.XmlSerialization
{
    /// <summary>File cabinet record.</summary>
    [Serializable]
    public class Record
    {
        /// <summary>Initializes a new instance of the <see cref="Record"/> class.</summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <param name="workingHours">The workingHours.</param>
        /// <param name="annualIncome">The annualIncome.</param>
        /// <param name="driverCategory">Type of the account.</param>
        public Record(string firstName, string lastName, DateTime dateOfBirth, short workingHours, decimal annualIncome, char driverCategory)
        {
            this.Name = new Name { FirstName = firstName, LastName = lastName };
            this.DateOfBirth = dateOfBirth;
            this.WorkingHoursPerWeek = workingHours;
            this.AnnualIncome = annualIncome;
            this.DriverLicenseCategory = driverCategory;
        }

        /// <summary>Initializes a new instance of the <see cref="Record"/> class.</summary>
        public Record()
        {
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [XmlAttribute("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [XmlElement("name")]
        public Name Name { get; set; }

        /// <summary>
        /// Gets or sets the date of birth.
        /// </summary>
        /// <value>
        /// The date of birth.
        /// </value>
        [XmlElement("dateOfBirth", DataType = "date")]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the working hourth per week.
        /// </summary>
        /// <value>
        /// The working hours per week.
        /// </value>
        [XmlElement("workinghours")]
        public short WorkingHoursPerWeek { get; set; }

        /// <summary>
        /// Gets or sets the annual income.
        /// </summary>
        /// <value>
        /// The annual income.
        /// </value>
        [XmlElement("annualIncome")]
        public decimal AnnualIncome { get; set; }

        /// <summary>
        /// Gets or sets the driver license category.
        /// </summary>
        /// <value>
        /// The driver license category.
        /// </value>
        [XmlElement(ElementName = "driverLicenseCategory")]
        public char DriverLicenseCategory { get; set; }
    }
}
