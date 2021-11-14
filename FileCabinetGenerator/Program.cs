using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CommandLine;
using FileCabinetApp.Utility;
using FileCabinetGenerator.RandomGenerator;
using FileCabinetGenerator.Utility;
using FileCabinetGenerator.Writer;

namespace FileCabinetGenerator
{
    /// <summary>
    /// Class Program.
    /// </summary>
    public static class Program
    {
        private static string outputPath;

        private static string outputType;

        private static int recordsAmount;

        private static int startIndex;

        private static string rule;

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            ParseArgs(args);

            var records = RandomRecordsGenerator.Generate(recordsAmount, startIndex, rule);
            var writer = new RecordsWriter(records);

            if (IsReWriteFile())
            {
                writer.WriteRecords(outputPath, outputType);
                Console.WriteLine($"All records are exported to file {outputPath?.Split('\\')[^1]}");
            }
            else
            {
                Console.WriteLine("File was not write.");
            }
        }

        private static void ParseArgs(IEnumerable<string> args)
        {
            string[] arguments = args.ToArray();
            Parser parser = Parser.Default;
            ParserResult<Options> result = parser.ParseArguments<Options>(arguments);
            try
            {
                result.WithParsed(
                        o =>
                        {
                            outputType = o.OutputType;
                            outputPath = o.Output;
                            recordsAmount = o.RecordsAmount;
                            startIndex = o.StartId;
                            rule = o.Rules;
                        })
                    .WithNotParsed(errs => DisplayHelp(result.ToString()));

                Validate();
            }
            catch (ArgumentException ex)
            {
                DisplayHelp(ex.Message);
            }
        }

        private static void Validate()
        {
            if ((outputType != null) && (!outputType.Equals("csv", StringComparison.OrdinalIgnoreCase) && !outputType.Equals("xml", StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException("The output type has to be xml or csv. (Parameter: -t, --output-type)");
            }

            int lastIndexOfBackSlash = outputPath.LastIndexOf("\\", StringComparison.OrdinalIgnoreCase);
            string fileDirection = lastIndexOfBackSlash > 0 ? outputPath[..lastIndexOfBackSlash] : string.Empty;

            if (string.IsNullOrWhiteSpace(outputPath) || (!string.IsNullOrEmpty(fileDirection) && (!Directory.Exists(fileDirection))))
            {
                throw new ArgumentException($"Export failed: can't open file {outputPath}. (Parameter: -o, --output).");
            }

            if (recordsAmount <= 0)
            {
                throw new ArgumentException("Value should be greater than zero. (Parameter: -a, --records-amount)");
            }

            if (startIndex < 0)
            {
                throw new ArgumentException("Value should be greater than zero. (Parameter: -i, --start-id)");
            }

            if (rule is null)
            {
                throw new ArgumentException("Rules is undefined. (Parameter: -r, --rules)");
            }
        }

        private static bool IsReWriteFile()
        {
            if (File.Exists(outputPath))
            {
                Console.Write($"File is exist - rewrite {outputPath}? [y/n] ");
                string answer = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(answer) || !answer.Equals("y", StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            return true;
        }

        private static void DisplayHelp(string errorMessage)
        {
            var sb = new StringBuilder();

            sb.AppendLine(errorMessage);
            sb.AppendLine();
            sb.AppendLine("Please enter the arguments as follows:");
            sb.AppendLine();
            sb.AppendLine("--output-type=\"file type\" --output=\"file path\" --records-amount=\"number\" --start-id=\"number\" (without quotes),");
            sb.AppendLine("or");
            sb.AppendLine("-t \"file type\" -o \"file path\" -a \"number\" -i \"number\" (without quotes),");
            sb.AppendLine();

            Console.WriteLine(sb.ToString());
        }
    }
}