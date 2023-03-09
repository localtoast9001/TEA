// <copyright file="CoffModuleWriter.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>

namespace Tea.Compiler
{
    using Tea.Compiler.Coff;
    using Tea.Compiler.X86;

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
                    builder.DefineExternalSymbol(DecorateSymbol(meth.MangledName), SymbolType.Function);
                }
            }

            foreach (string symbol in module.ExternList)
            {
                builder.DefineExternalSymbol(DecorateSymbol(symbol), SymbolType.Function);
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

        /// <summary>
        /// Adds an extra underscore to the name when needed to be compatible with the MS compilers.
        /// </summary>
        /// <param name="symbol">The symbol name.</param>
        /// <returns>The decorated string.</returns>
        /// <remarks>
        /// The MS compilers prepend an extra underscore to the symbol name inside the obj files.
        /// For internally generated symbols, the extra underscore is not there and those will start with '$'.
        /// </remarks>
        private static string DecorateSymbol(string symbol)
        {
            return !symbol.StartsWith("$") ? "_" + symbol : symbol;
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

                Symbol sym = textSection.DefineSymbol(DecorateSymbol(method.Method!.MangledName), StorageClass.External, SymbolType.Function);
                WriteStatements(textSection, method.Statements);
            }

            if (mainMethod != null)
            {
                BuildMainWrapper(textSection, mainMethod!);
            }

            FixRelocationOffsets(textSection);

            return textSection;
        }

        /// <summary>
        /// Corrects relative offsets in relocation data.
        /// </summary>
        /// <param name="textSection">The text section to modify.</param>
        /// <remarks>
        /// The <see cref="CodeGenerator"/> emits relative relocation bases natively for ELF assuming the instruction pointer is the byte after the relocation.
        /// For example, the data at the relocation of 32-bit pointer size will be -4 (0xfffffffc or 0xfc 0xff 0xff 0xff in bytes).
        /// COFF assumes the instruction pointer is at the start of the relocation, so the data at the relocation will change to be 0.
        /// </remarks>
        private static void FixRelocationOffsets(ProgramSection textSection)
        {
            foreach (Relocation rel in textSection.Relocations)
            {
                if (rel.Relative)
                {
                    textSection.ModifyContent(rel, new ReadOnlySpan<byte>(0.ToBytes()));
                }
            }
        }

        private static void WriteStatements(ProgramSection textSection, IEnumerable<AsmStatement> statements)
        {
            foreach (AsmStatement statement in statements)
            {
                if (statement.Label != null)
                {
                    _ = textSection.DefineSymbol(statement.Label!, StorageClass.Label, SymbolType.None);
                }

                foreach (RelocationEntry relEntry in statement.Instruction!.RelocationEntries)
                {
                    textSection.DefineRelocation(DecorateSymbol(relEntry.Symbol), relEntry.Relative, relEntry.Offset);
                }

                textSection.ContentWriter.WriteBytes(statement.Instruction!.ToArray());
            }
        }

        private static void BuildMainWrapper(ProgramSection textSection, string mainMethod)
        {
            _ = textSection.DefineSymbol("_main", StorageClass.External, SymbolType.Function);
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
                if (!string.IsNullOrEmpty(dataEntry.Label))
                {
                    _ = dataSection.DefineSymbol(
                        dataEntry.Label!,
                        StorageClass.Static,
                        SymbolType.None);
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
                else if (obj is string symbolRef)
                {
                    if (!string.Equals(symbolRef, "0", StringComparison.Ordinal))
                    {
                        dataSection.DefineRelocation(DecorateSymbol(symbolRef), false);
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
