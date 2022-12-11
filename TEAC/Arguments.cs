//-----------------------------------------------------------------------
// <copyright file="Arguments.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TEAC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Program Arguments.
    /// </summary>
    internal class Arguments
    {
        /// <summary>
        /// List of search directories for includes.
        /// </summary>
        private List<string> includes = new List<string>();

        private Arguments(
            string inputFile,
            string outputListing,
            string outputObj,
            IEnumerable<string> includeDirs)
        {
            this.InputFile = inputFile;
            this.OutputListing = outputListing;
            this.OutputObj = outputObj;
            this.includes.AddRange(includeDirs);
        }

        /// <summary>
        /// Gets the input file.
        /// </summary>
        public string InputFile { get; }

        /// <summary>
        /// Gets the output listing path.
        /// </summary>
        public string OutputListing { get; }

        /// <summary>
        /// Gets the output obj path.
        /// </summary>
        public string OutputObj { get; }

        /// <summary>
        /// Gets the list of include search paths.
        /// </summary>
        public IList<string> Includes
        {
            get { return this.includes; }
        }

        /// <summary>
        /// Tries to parse the program arguments.
        /// </summary>
        /// <param name="args">The program arguments.</param>
        /// <param name="result">On success, receives the parsed arguments.</param>
        /// <returns>True if parsed; otherwise, false.</returns>
        public static bool TryParse(string[] args, out Arguments? result)
        {
            result = null;
            string? inputFile = null;
            string? outputListing = null;
            string? outputObj = null;
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
                else if (arg.StartsWith("/Fo", StringComparison.Ordinal))
                {
                    outputObj = arg.Substring(3);
                }
                else if (arg.StartsWith("/I", StringComparison.Ordinal))
                {
                    string includeDir = arg.Substring(2);
                    includes.Add(includeDir);
                }
                else
                {
                    break;
                }
            }

            if (index != args.Length - 1)
            {
                return false;
            }

            inputFile = args[index];
            if (string.IsNullOrEmpty(outputListing))
            {
                outputListing = GetOutputPath(inputFile, ".asm");
            }

            if (string.IsNullOrEmpty(outputObj))
            {
                outputObj = GetOutputPath(inputFile, ".o");
            }

            result = new Arguments(inputFile, outputListing, outputObj, includes);
            return true;
        }

        private static string GetOutputPath(string inputFile, string defaultExt)
        {
            int extIndex = inputFile.LastIndexOf('.');
            if (extIndex > 0 && string.Compare(inputFile.Substring(extIndex), ".tea", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return inputFile.Substring(0, extIndex) + defaultExt;
            }
            else
            {
                return inputFile + defaultExt;
            }
        }
    }
}
