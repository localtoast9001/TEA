using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEAC
{
    internal class Arguments
    {
        /// <summary>
        /// List of search directories for includes.
        /// </summary>
        private List<string> includes = new List<string>();

        public string InputFile { get; private set; }

        public string OutputListing { get; private set; }

        public string OutputAssembly { get; private set; }

        public IList<string> Includes { get { return this.includes; } }

        public static bool TryParse(string[] args, out Arguments result)
        {
            result = null;
            string inputFile = null;
            string outputListing = null;
            string outputAssembly = null;
            List<string> includes = new List<string>();
            if (args.Length < 1)
            {
                return false;
            }

            int index = 0;
            for (; index < args.Length; index++)
            {
                string arg = args[index];
                if (arg.Length < 1)
                {
                    break;
                }

                if (arg[0] != '/')
                {
                    break;
                }

                if (arg.StartsWith("/Fa", StringComparison.Ordinal))
                {
                    outputListing = arg.Substring(3);
                }
                else if(arg.StartsWith("/Fe", StringComparison.Ordinal))
                {
                    outputAssembly = arg.Substring(3);
                }
                else if (arg.StartsWith("/I", StringComparison.Ordinal))
                {
                    string includeDir = arg.Substring(2);
                    includes.Add(includeDir);
                }
                else
                {
                    return false;
                }
            }

            if (index != args.Length - 1)
            {
                return false;
            }

            inputFile = args[index];
            if (string.IsNullOrEmpty(outputListing))
            {
                int extIndex = inputFile.LastIndexOf('.');
                if (extIndex > 0 && string.Compare(inputFile.Substring(extIndex), ".tea", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    outputListing = inputFile.Substring(0, extIndex) + ".asm";
                }
                else
                {
                    outputListing = inputFile + ".asm";
                }
            }

            result = new Arguments(inputFile, outputListing, includes, outputAssembly);
            return true;
        }

        private Arguments(string inputFile, string outputListing, IEnumerable<string> includeDirs, string outputAssembly)
        {
            this.InputFile = inputFile;
            this.OutputListing = outputListing;
            this.includes.AddRange(includeDirs);
            this.OutputAssembly = outputAssembly;
        }
    }
}
