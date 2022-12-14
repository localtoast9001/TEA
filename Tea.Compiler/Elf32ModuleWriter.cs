//-----------------------------------------------------------------------
// <copyright file="Elf32ModuleWriter.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler
{
    using System;
    using System.IO;
    using Tea.Compiler.Elf;

    /// <summary>
    /// Module writer for the elf32-little format.
    /// </summary>
    public class Elf32ModuleWriter : ModuleWriter
    {
        private const byte Class32Bit = 1;

        private const byte ElfVersion = 1;

        private const byte LittleEndian = 1;

        private const byte SystemVAbi = 0;

        private const ushort RelocatableObjectType = 0x01;

        private const ushort X86TargetArch = 0x03;

        private const uint HeaderSize = 0x34;

        private static readonly byte[] MagicNumber = { 0x7f, 0x45, 0x4c, 0x46 };

        /// <summary>
        /// Initializes a new instance of the <see cref="Elf32ModuleWriter"/> class.
        /// </summary>
        /// <param name="path">The output file path.</param>
        public Elf32ModuleWriter(string path)
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
            Elf32Builder builder = new Elf32Builder
            {
                Type = ElfType.Rel,
                Machine = MachineIsa.X86,
                OSAbi = OSAbi.SystemV,
                SourceFileName = module.SourceFileName,
            };

            builder.Sections.Add(BuildCodeSection(module.CodeSegment));
            builder.Sections.Add(BuildDataSection(module.DataSegment));

            foreach (MethodInfo meth in module.ProtoList)
            {
                foreach (MethodImpl methImpl in module.CodeSegment)
                {
                    if (methImpl.Method! == meth)
                    {
                        continue;
                    }
                }

                builder.DefineExternalSymbol(meth.MangledName);
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
                Name = ".text",
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

                Symbol sym = textSection.StartSymbol(method.Method!.MangledName, SymbolType.Func, SymbolBinding.Global);
                WriteStatements(textSection, method.Statements);

                textSection.EndSymbol(sym);
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
                    Symbol label = textSection.StartSymbol(statement.Label!, SymbolType.Func, SymbolBinding.Local);
                    textSection.EndSymbol(label);
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
            Symbol sym = textSection.StartSymbol("main", SymbolType.Func, SymbolBinding.Global);
            AsmStatement[] mainBody = new AsmStatement[]
            {
                new AsmStatement { Instruction = X86.X86Instruction.Jmp(mainMethod) },
            };

            WriteStatements(textSection, mainBody);
            textSection.EndSymbol(sym);
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
                    sym = dataSection.StartSymbol(
                        dataEntry.Label!,
                        SymbolType.Object,
                        SymbolBinding.Local);
                }

                BuildDataEntry(dataSection, dataEntry);

                if (sym != null)
                {
                    dataSection.EndSymbol(sym!);
                }
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
                    dataSection.DefineRelocation(symbolRef, false);
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