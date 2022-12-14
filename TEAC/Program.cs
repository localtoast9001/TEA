//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TEAC
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Tea.Compiler;
    using Tea.Language;

    /// <summary>
    /// Program entry point.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Main program entry routine.
        /// </summary>
        /// <param name="args">Program arguments.</param>
        /// <returns>Program exit code.</returns>
        public static int Main(string[] args)
        {
            try
            {
                if (!Arguments.TryParse(args, out Arguments? arguments))
                {
                    Console.Error.WriteLine(Properties.Resources.Usage);
                    return 1;
                }

                MessageLog log = new MessageLog();
                CompilerContext context = new CompilerContext();
                context.AddIncludePaths(arguments!.Includes);
                context.SourceFileName = System.IO.Path.GetFileName(arguments.InputFile);
                using (TokenReader reader = new TokenReader(arguments.InputFile, log))
                {
                    Parser parser = new Parser(log);
                    ProgramUnit? programUnit = null;

                    // parsing pass.
                    if (parser.TryParse(reader, out programUnit))
                    {
                        // code generation pass.
                        CodeGenerator codeGen = new CodeGenerator(log);
                        if (codeGen.CreateTypes(context, programUnit!))
                        {
                            Module? module = null;
                            if (codeGen.CreateModule(context, programUnit!, out module))
                            {
                                using (AsmModuleWriter moduleWriter = new AsmModuleWriter(arguments!.OutputListing))
                                {
                                    moduleWriter.Write(module!);
                                }

                                using (Elf32ModuleWriter objWriter = new Elf32ModuleWriter(arguments!.OutputObj))
                                {
                                    objWriter.Write(module!);
                                }
                            }
                        }
                    }
                }

                foreach (Message message in log.Messages)
                {
                    TextWriter target = message.Severity == Severity.Error ?
                        Console.Error :
                        Console.Out;
                    target.WriteLine(message.ToString());
                }

                return log.HasErrors ? 1 : 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
                return 1;
            }
        }
    }
}
