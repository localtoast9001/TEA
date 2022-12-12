//-----------------------------------------------------------------------
// <copyright file="Elf32Builder.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler.Elf
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;

    /// <summary>
    /// Builder pattern for a 32bit little endian ELF file.
    /// </summary>
    public class Elf32Builder
    {
        /// <summary>
        /// Gets the sections.
        /// </summary>
        public Collection<Section> Sections { get; } = new Collection<Section>();

        /// <summary>
        /// Gets the collection of external symbols.
        /// </summary>
        public Collection<Symbol> ExternalSymbols { get; } = new Collection<Symbol>();

        /// <summary>
        /// Gets or sets the type of file.
        /// </summary>
        public ElfType Type { get; set; }

        /// <summary>
        /// Gets or sets the machine ISA.
        /// </summary>
        public MachineIsa Machine { get; set; }

        /// <summary>
        /// Gets or sets the OS ABI.
        /// </summary>
        public OSAbi OSAbi { get; set; }

        /// <summary>
        /// Defines an external symbol.
        /// </summary>
        /// <param name="name">The name of the external symbol.</param>
        /// <returns>A new instance of the <see cref="Symbol"/> class.</returns>
        public Symbol DefineExternalSymbol(string name)
        {
            Symbol sym = new Symbol(name, 0, SymbolType.Func, SymbolBinding.Global);
            this.ExternalSymbols.Add(sym);
            return sym;
        }

        /// <summary>
        /// Saves the contents to disk.
        /// </summary>
        /// <param name="path">The path of the file to create.</param>
        public void Save(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.CreateNew))
            {
                this.Save(stream);
            }
        }

        /// <summary>
        /// Saves the contents to a stream.
        /// </summary>
        /// <param name="stream">The stream to write.</param>
        public void Save(Stream stream)
        {
            StreamBinaryWriter writer = new StreamBinaryWriter(stream);

            uint offset = Elf32Header.BinarySize;

            List<Section> expandedSections = new List<Section>();
            SymbolTableSection symbolTable = new SymbolTableSection();
            symbolTable.Name = ".symtab";

            foreach (Section section in this.Sections)
            {
                expandedSections.Add(section);

                ProgramSection? progSection = section as ProgramSection;
                if (progSection != null)
                {
                    if (progSection!.Relocations.Count > 0)
                    {
                        RelocationTableSection relSection = new RelocationTableSection(progSection)
                        {
                            Name = ".rel" + section.Name,
                        };

                        expandedSections.Add(relSection);
                        relSection.Link = symbolTable;
                    }
                }
            }

            StringTableSection stringTable = new StringTableSection();
            stringTable.DefineString(string.Empty);
            stringTable.Name = ".strtab";
            symbolTable.Link = stringTable;
            expandedSections.Add(symbolTable);
            expandedSections.Add(stringTable);

            foreach (Symbol sym in this.ExternalSymbols)
            {
                Elf32SymbolEntry symbolEntry = new Elf32SymbolEntry()
                {
                    Name = stringTable.DefineString(sym.Name),
                    Type = SymbolType.Func,
                };

                symbolTable.Symbols.Add(symbolEntry);
            }

            StringTableSection sectionHeaderStrings = new StringTableSection();
            sectionHeaderStrings.DefineString(string.Empty);
            sectionHeaderStrings.Name = ".shrtrtab";
            expandedSections.Add(sectionHeaderStrings);

            // create symbol entries.
            int sectionHeaderIndex = 1;
            foreach (Section section in expandedSections)
            {
                ProgramSection? progSection = section as ProgramSection;
                if (progSection != null)
                {
                    foreach (Symbol sym in progSection!.Symbols)
                    {
                        Elf32SymbolEntry symEntry = new Elf32SymbolEntry()
                        {
                            Name = stringTable.DefineString(sym.Name),
                            Value = (uint)sym.Offset,
                            Size = sym.Size,
                            SectionHeaderIndex = (ushort)sectionHeaderIndex,
                            Type = sym.Type,
                            Binding = sym.Binding,
                        };

                        symbolTable.Symbols.Add(symEntry);
                    }
                }

                sectionHeaderIndex++;
            }

            // Populate relocation sections.
            foreach (RelocationTableSection relSection in expandedSections.OfType<RelocationTableSection>())
            {
                ProgramSection targetSection = relSection.Target;
                foreach (Relocation rel in targetSection.Relocations)
                {
                    uint nameOffset = stringTable.DefineString(rel.Symbol);
                    uint symbolRef = symbolTable.FindSymbol(nameOffset);
                    Rel32 rel32 = new Rel32
                    {
                        Offset = rel.Offset,
                        Type = rel.Relative ? Rel32Type.R_386_PC32 : Rel32Type.R_386_32,
                        SymbolRef = symbolRef,
                    };

                    relSection.Relocations.Add(rel32);
                }
            }

            Elf32Header header = this.CreateHeader(expandedSections.Count);
            SectionHeaderEntry32[] sections = new SectionHeaderEntry32[expandedSections.Count + 1];
            sections[0] = new SectionHeaderEntry32();
            sectionHeaderIndex = 1;
            foreach (Section section in expandedSections)
            {
                AdvanceOffset(ref offset, 0, section.Align);
                sections[sectionHeaderIndex] = CreateSectionHeader(section, offset, sectionHeaderStrings);

                AdvanceOffset(ref offset, section.Size, 0);
                sectionHeaderIndex++;
            }

            // set linked sections.
            for (int i = 0; i < expandedSections.Count; i++)
            {
                Section section = expandedSections[i];
                SectionHeaderEntry32 entry = sections[i + 1];
                if (section.Link != null)
                {
                    int linkedSectionIndex = expandedSections.IndexOf(section.Link);
                    if (linkedSectionIndex >= 0)
                    {
                        entry.Link = (uint)linkedSectionIndex + 1;
                        sections[i + 1] = entry;
                    }
                }

                RelocationTableSection? relTableSection = section as RelocationTableSection;
                if (relTableSection != null)
                {
                    int infoLink = expandedSections.IndexOf(relTableSection!.Target);
                    if (infoLink >= 0)
                    {
                        entry.Info = (uint)infoLink + 1;
                        sections[i + 1] = entry;
                    }
                }
            }

            AdvanceOffset(ref offset, 0, sizeof(uint));
            header.SectionHeaderTableOffset = offset;

            header.Serialize(writer);
            sectionHeaderIndex = 1;
            offset = Elf32Header.BinarySize;
            foreach (Section section in expandedSections)
            {
                SectionHeaderEntry32 entry = sections[sectionHeaderIndex];
                writer.Skip(entry.Offset - offset);
                ((ISerializable)section).Serialize(writer);
                offset = entry.Offset + entry.Size;
                sectionHeaderIndex++;
            }

            writer.Skip(header.SectionHeaderTableOffset - offset);
            for (int i = 0; i < sections.Length; i++)
            {
                sections[i].Serialize(writer);
            }
        }

        private static SectionHeaderEntry32 CreateSectionHeader(
            Section section,
            uint offset,
            StringTableSection sectionHeaderStrings)
        {
            return new SectionHeaderEntry32()
            {
                Offset = offset,
                AddrAlign = section.Align,
                Flags = section.Flags,
                NameOffset = sectionHeaderStrings.DefineString(section.Name),
                Size = section.Size,
                Type = section.Type,
                Info = section.Info,
                EntrySize = section.EntrySize,
            };
        }

        private static void AdvanceOffset(
            ref uint offset,
            uint count,
            uint alignment)
        {
            offset += count;
            if (alignment == 0)
            {
                return;
            }

            uint remainder = offset % alignment;
            if (remainder > 0)
            {
                offset += alignment - remainder;
            }
        }

        private Elf32Header CreateHeader(int expandedSectionCount)
        {
            Elf32Header header = new Elf32Header();
            Constants.MagicNumber.CopyTo(header.Magic);
            header.Class = Constants.Class32Bit;
            header.Endian = Constants.LittleEndian;
            header.Version = Constants.ElfVersion;
            header.AbiVersion = 0;
            header.Type = this.Type;
            header.Machine = this.Machine;
            header.OSAbi = this.OSAbi;
            header.SectionHeaderEntryCount = (ushort)(expandedSectionCount + 1);
            header.SectionHeaderEntrySize = (ushort)SectionHeaderEntry32.BinarySize;
            header.SectionHeaderStringIndex = (ushort)expandedSectionCount;
            return header;
        }
    }
}