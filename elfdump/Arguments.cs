//-----------------------------------------------------------------------
// <copyright file="Arguments.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.ElfDump
{
    /// <summary>
    /// Program Arguments.
    /// </summary>
    internal class Arguments
    {
        private Arguments(string path)
        {
            this.Path = path;
        }

        /// <summary>
        /// Gets the path to the input file.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Tries to parse the arguments.
        /// </summary>
        /// <param name="args">The program arguments.</param>
        /// <param name="result">On success, receives the results as a new instance of the <see cref="Arguments"/> class.</param>
        /// <returns>True if successful; otherwise, false.</returns>
        public static bool TryParse(string[] args, out Arguments? result)
        {
            result = null;
            string path = string.Empty;
            if (args.Length != 1)
            {
                return false;
            }

            path = args[0];
            result = new Arguments(path);
            return true;
        }
    }
}