//-----------------------------------------------------------------------
// <copyright file="Elf32ModuleWriter.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.ElfDump
{
    /// <summary>
    /// Program logic.
    /// </summary>
    public static class Program
    {
        private const int SuccessExitCode = 0;

        private const int ErrorExitCode = 1;

        /// <summary>
        /// Main routine for the program.
        /// </summary>
        /// <param name="args">Program args.</param>
        /// <returns>Program exit code.</returns>
        public static int Main(string[] args)
        {
            if (!Arguments.TryParse(args, out Arguments? arguments))
            {
                PrintUsage();
                return ErrorExitCode;
            }

            try
            {

                return SuccessExitCode;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
                return ErrorExitCode;
            }
        }

        private static void PrintUsage()
        {
            Console.Error.WriteLine("Usage:");
            Console.Error.WriteLine("elfdump <path>");
            Console.Error.WriteLine("Options:");
            Console.Error.WriteLine("  path - Path to the ELF binary to dump.");
        }
    }
}