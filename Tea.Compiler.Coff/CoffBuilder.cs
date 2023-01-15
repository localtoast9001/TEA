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
        /// Gets or sets the machine architecture.
        /// </summary>
        public Machine Machine { get; set; }

        /// <summary>
        /// Gets or sets the timestamp inside the header.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

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
                };

                SetStringField(section.Name, new Span<byte>(sectionHeader.Name, 0, sectionHeader.Name.Length), stringTable);

                sectionHeaders[i] = sectionHeader;
            }

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



            stringTable.Serialize(writer);
        }

        private static void SetStringField(string value, Span<byte> field, StringTable stringTable)
        {
            byte[] utf8 = Encoding.UTF8.GetBytes(value);
            if (utf8.Length <= field.Length)
            {
                utf8.CopyTo(field);
            }
            else
            {
                uint index = stringTable.DefineString(value);
                field[4] = (byte)index;
                field[5] = (byte)(index >> 8);
                field[6] = (byte)(index >> 16);
                field[7] = (byte)(index >> 24);
            }
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
