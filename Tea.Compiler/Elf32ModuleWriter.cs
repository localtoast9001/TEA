//-----------------------------------------------------------------------
// <copyright file="Elf32ModuleWriter.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler
{
    using System;
    using System.IO;
    using Elf;

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
            };

            ProgramSection textSection = new ProgramSection
            {
                Name = ".text",
                Executable = true,
            };

            builder.Sections.Add(textSection);
            builder.Sections.Add(BuildDataSection(module.DataSegment));

            builder.Save(this.Output);
            return true;
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.Output.Close();
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
                        SymbolBinding.Global);
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
                    dataSection.DefineRelocation(symbolRef);
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