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
        private readonly List<string> includes = new List<string>();

        private Arguments(
            string inputFile,
            string outputListing,
            string? outputElfObj,
            string? outputCoffObj,
            IEnumerable<string> includeDirs)
        {
            this.InputFile = inputFile;
            this.OutputListing = outputListing;
            this.OutputElfObj = outputElfObj;
            this.OutputCoffObj = outputCoffObj;
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
        /// Gets the output ELF obj path.
        /// </summary>
        public string? OutputElfObj { get; }

        /// <summary>
        /// Gets the output COFF obj path.
        /// </summary>
        public string? OutputCoffObj { get; }

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
            List<string> outputObjs = new List<string>();
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
                    string outputObj = arg.Substring(3);
                    outputObjs.Add(outputObj);
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

            if (outputObjs.Count == 0)
            {
                outputObjs.Add(GetOutputPath(inputFile, ".o"));
            }

            string? outputElfObj = null;
            string? outputCoffObj = null;
            foreach (string o in outputObjs)
            {
                if (o.EndsWith(".o"))
                {
                    outputElfObj = o;
                }
                else if (o.EndsWith(".obj"))
                {
                    outputCoffObj = o;
                }
            }

            result = new Arguments(
                inputFile,
                outputListing,
                outputElfObj,
                outputCoffObj,
                includes);
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
