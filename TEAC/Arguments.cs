using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEAC
{
    internal class Arguments
    {
        public string InputFile { get; private set; }

        public string OutputListing { get; private set; }

        public static bool TryParse(string[] args, out Arguments result)
        {
            result = null;
            string inputFile = null;
            string outputListing = null;
            if (args.Length > 2 || args.Length < 1)
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

            result = new Arguments(inputFile, outputListing);
            return true;
        }

        private Arguments(string inputFile, string outputListing)
        {
            this.InputFile = inputFile;
            this.OutputListing = outputListing;
        }
    }
}
