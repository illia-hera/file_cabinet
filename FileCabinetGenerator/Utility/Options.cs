using CommandLine;

namespace FileCabinetGenerator.Utility
{
    /// <summary>
    /// Class Option.
    /// </summary>
    public class Options
    {
        /// <summary>
        /// Gets or sets the type of the output.
        /// </summary>
        /// <value>
        /// The type of the output.
        /// </value>
        [Option('t', "output-type", Required = true, HelpText = "Output format type (csv, xml).")]
        public string OutputType { get; set; }

        /// <summary>
        /// Gets or sets the rules.
        /// </summary>
        /// <value>
        /// The rules.
        /// </value>
        [Option('r', "rules", Required = true, HelpText = "Default or custom value rules.")]
        public string Rules { get; set; }

        /// <summary>
        /// Gets or sets the output.
        /// </summary>
        /// <value>
        /// The output.
        /// </value>
        [Option('o', "output", Required = true, HelpText = "Output file path.")]
        public string Output { get; set; }

        /// <summary>
        /// Gets or sets the records amount.
        /// </summary>
        /// <value>
        /// The records amount.
        /// </value>
        [Option('a', "records-amount", Required = true, HelpText = "Number of generated records. Must be greater than 0")]
        public int RecordsAmount { get; set; }

        /// <summary>
        /// Gets or sets the start identifier.
        /// </summary>
        /// <value>
        /// The start identifier.
        /// </value>
        [Option('i', "start-id", Required = true, HelpText = "ID value to start. Must be equal or greater than 0")]
        public int StartId { get; set; }
    }
}