// <copyright file="CoffBuilder.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>

namespace Tea.Compiler.Coff
{
    using System;
    using System.Collections.ObjectModel;
    using System.Text;
    using Tea.Compiler.Binary;

    /// <summary>
    /// Builder pattern for creating a COFF object file.
    /// </summary>
    public class CoffBuilder
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
        /// Gets or sets the machine architecture.
        /// </summary>
        public Machine Machine { get; set; }

        /// <summary>
        /// Gets or sets the timestamp inside the header.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Defines an external symbol.
        /// </summary>
        /// <param name="name">The name of the external symbol.</param>
        /// <param name="type">The symbol type.</param>
        /// <returns>A new instance of the <see cref="Symbol"/> class.</returns>
        public Symbol DefineExternalSymbol(string name, SymbolType type)
        {
            Symbol sym = new Symbol(name, 0, StorageClass.External, type);
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
            // Create in-memory structures and calculate offsets before serializing.
            StringTable stringTable = new StringTable();
            SymbolTable symbolTable = new SymbolTable(stringTable);
            foreach (Symbol sym in this.ExternalSymbols)
            {
                symbolTable.DefineSymbol(sym, 0);
            }

            // 1. File header.
            ImageFileHeader header = this.CreateHeader();
            uint offset = ImageFileHeader.BinarySize;

            // 2. Optional header (not used for obj files).
            // 3. Section headers.
            ImageSectionHeader[] sectionHeaders = new ImageSectionHeader[this.Sections.Count];
            offset += (uint)(sectionHeaders.Length * ImageSectionHeader.BinarySize);
            for (int i = 0; i < this.Sections.Count; i++)
            {
                Section section = this.Sections[i];
                IReadOnlyCollection<Relocation> sectionRels = section.Relocations;
                ImageSectionHeader sectionHeader = new ImageSectionHeader()
                {
                    SizeOfRawData = section.Length,
                    PointerToRawData = offset,
                    PointerToRelocations = sectionRels.Count > 0 ? offset + section.Length : 0,
                    NumberOfRelocations = (ushort)sectionRels.Count,
                    Characteristics = section.Characteristics,
                };

                stringTable.SetStringField(section.Name, new Span<byte>(sectionHeader.Name, 0, sectionHeader.Name.Length));

                sectionHeaders[i] = sectionHeader;
                offset += section.Length;
                offset += (uint)(sectionRels.Count * Rel.BinarySize);

                foreach (Symbol sym in section.Symbols)
                {
                    _ = symbolTable.DefineSymbol(sym, i + 1);
                }
            }

            header.PointerToSymbolTable = offset;
            header.NumberOfSymbols = (uint)symbolTable.Count;

            // 4. For each section:
            // 4a. Section data (aligned).
            // 4b. Sections relocation tables.
            // 4c. Section line number tables.
            // 6. Symbol table.
            // 7. String table.

            // Serialize.
            StreamBinaryWriter writer = new StreamBinaryWriter(stream);
            header.Serialize(writer);
            foreach (ImageSectionHeader sectionHeader in sectionHeaders)
            {
                sectionHeader.Serialize(writer);
            }

            foreach (Section section in this.Sections)
            {
                ((ISerializable)section).Serialize(writer);
                foreach (Relocation rel in section.Relocations)
                {
                    Rel r = new ()
                    {
                        Addr = rel.Offset,
                        SymbolIndex = (uint)symbolTable.FindSymbol(rel.Symbol),
                        Type = rel.Relative ? RelType.Rel32 : RelType.Addr32,
                    };

                    r.Serialize(writer);
                }
            }

            symbolTable.Serialize(writer);
            stringTable.Serialize(writer);
        }

        private ImageFileHeader CreateHeader()
        {
            ImageFileHeader header = new ImageFileHeader()
            {
                Machine = this.Machine,
                TimeDateStamp = this.Timestamp,
                NumberOfSections = (ushort)this.Sections.Count,
            };

            return header;
        }
    }
}
