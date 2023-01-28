// <copyright file="CoffModuleWriter.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>

namespace Tea.Compiler
{
    using Tea.Compiler.Coff;

    /// <summary>
    /// Writes the module to a COFF binary.
    /// </summary>
    public class CoffModuleWriter : ModuleWriter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CoffModuleWriter"/> class.
        /// </summary>
        /// <param name="path">The path to the output file.</param>
        public CoffModuleWriter(string path)
        {
            this.Output = new FileStream(path, FileMode.Create);
        }

        /// <summary>
        /// Gets the output stream.
        /// </summary>
        internal Stream Output { get; }

        /// <inheritdoc/>
        public override bool Write(Module module)
        {
            CoffBuilder builder = new CoffBuilder
            {
                Machine = Machine.I386,
                Timestamp = DateTime.UtcNow,
            };

            builder.Sections.Add(BuildCodeSection(module.CodeSegment));
            builder.Sections.Add(BuildDataSection(module.DataSegment));

            foreach (MethodInfo meth in module.ProtoList)
            {
                if (!module.CodeSegment.Any(e => e.Method! == meth))
                {
                    builder.DefineExternalSymbol(meth.MangledName);
                }
            }

            foreach (string symbol in module.ExternList)
            {
                builder.DefineExternalSymbol(symbol);
            }

            builder.Save(this.Output);
            return true;
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.Output.Close();
        }

        private static ProgramSection BuildCodeSection(IList<MethodImpl> codeSegment)
        {
            ProgramSection textSection = new ProgramSection
            {
                Name = ".text$mn",
                Executable = true,
            };

            string? mainMethod = null;
            foreach (MethodImpl method in codeSegment)
            {
                if ((method.Method?.IsStatic ?? false) &&
                    string.CompareOrdinal("Main", method.Method.Name) == 0 &&
                    method.Method.Parameters.Count == 2 &&
                    string.CompareOrdinal(method.Method?.Parameters[0].Type?.FullName, "integer") == 0 &&
                    string.CompareOrdinal(method.Method?.Parameters[1].Type?.FullName, "#0#0character") == 0)
                {
                    mainMethod = method.Method?.MangledName;
                }

                Symbol sym = textSection.DefineSymbol(method.Method!.MangledName, true);
                WriteStatements(textSection, method.Statements);
            }

            if (mainMethod != null)
            {
                BuildMainWrapper(textSection, mainMethod!);
            }

            return textSection;
        }

        private static void WriteStatements(ProgramSection textSection, IEnumerable<AsmStatement> statements)
        {
            foreach (AsmStatement statement in statements)
            {
                if (statement.Label != null)
                {
                    Symbol label = textSection.DefineSymbol(statement.Label!, false);
                }

                foreach (RelocationEntry relEntry in statement.Instruction!.RelocationEntries)
                {
                    textSection.DefineRelocation(relEntry.Symbol, relEntry.Relative, relEntry.Offset);
                }

                textSection.ContentWriter.WriteBytes(statement.Instruction!.ToArray());
            }
        }

        private static void BuildMainWrapper(ProgramSection textSection, string mainMethod)
        {
            Symbol sym = textSection.DefineSymbol("main", true);
            AsmStatement[] mainBody = new AsmStatement[]
            {
                new AsmStatement { Instruction = X86.X86Instruction.Jmp(mainMethod) },
            };

            WriteStatements(textSection, mainBody);
        }

        private static ProgramSection BuildDataSection(IList<DataEntry> dataSegment)
        {
            ProgramSection dataSection = new ProgramSection
            {
                Name = ".data",
                Writeable = true,
            };

            foreach (DataEntry dataEntry in dataSegment)
            {
                Symbol? sym = null;
                if (!string.IsNullOrEmpty(dataEntry.Label))
                {
                    sym = dataSection.DefineSymbol(
                        dataEntry.Label!,
                        false);
                }

                BuildDataEntry(dataSection, dataEntry);
            }

            return dataSection;
        }

        private static void BuildDataEntry(ProgramSection dataSection, DataEntry entry)
        {
            foreach (object obj in entry.Value ?? Array.Empty<object>())
            {
                if (obj is byte)
                {
                    byte val = (byte)obj;
                    dataSection.ContentWriter.WriteByte(val);
                }
                else if (obj is ushort)
                {
                    ushort val = (ushort)obj;
                    dataSection.ContentWriter.WriteUInt16(val);
                }
                else if (obj is string)
                {
                    string symbolRef = (string)obj;
                    if (!string.Equals(symbolRef, "0", StringComparison.Ordinal))
                    {
                        dataSection.DefineRelocation(symbolRef, false);
                    }

                    dataSection.ContentWriter.WriteUInt32(0);
                }
                else
                {
                    uint val = (uint)obj;
                    dataSection.ContentWriter.WriteUInt32(val);
                }
            }
        }
    }
}
