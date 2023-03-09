//-----------------------------------------------------------------------
// <copyright file="Elf32Header.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler.Elf
{
    using Tea.Compiler.Binary;

    /// <summary>
    /// ELF file header.
    /// </summary>
    internal class Elf32Header : ISerializable
    {
        /// <summary>
        /// The size of the header when serialized.
        /// </summary>
        public const int BinarySize = 0x34;

        private const int ClassOffset = 4;

        private const int EndianOffset = 5;

        private const int VersionOffset = 6;

        private const int OSAbiOffset = 7;

        private const int AbiVersionOffset = 8;

        private readonly byte[] ident = new byte[0x10];

        /// <summary>
        /// Initializes a new instance of the <see cref="Elf32Header"/> class.
        /// </summary>
        public Elf32Header()
        {
        }

        /// <summary>
        /// Gets the span for the magic number.
        /// </summary>
        public Span<byte> Magic => new Span<byte>(this.ident, 0, 4);

        /// <summary>
        /// Gets or sets the class.
        /// </summary>
        public byte Class
        {
            get { return this.ident[ClassOffset]; }
            set { this.ident[ClassOffset] = value; }
        }

        /// <summary>
        /// Gets or sets the endian.
        /// </summary>
        public byte Endian
        {
            get { return this.ident[EndianOffset]; }
            set { this.ident[EndianOffset] = value; }
        }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public byte Version
        {
            get { return this.ident[VersionOffset]; }
            set { this.ident[VersionOffset] = value; }
        }

        /// <summary>
        /// Gets or sets the OS ABI.
        /// </summary>
        public OSAbi OSAbi
        {
            get { return (OSAbi)this.ident[OSAbiOffset]; }
            set { this.ident[OSAbiOffset] = (byte)value; }
        }

        /// <summary>
        /// Gets or sets the ABI version.
        /// </summary>
        public byte AbiVersion
        {
            get { return this.ident[AbiVersionOffset]; }
            set { this.ident[AbiVersionOffset] = value; }
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public ElfType Type { get; set; } = ElfType.None;

        /// <summary>
        /// Gets or sets the machine ISA.
        /// </summary>
        public MachineIsa Machine { get; set; } = MachineIsa.None;

        /// <summary>
        /// Gets or sets the entry point.
        /// </summary>
        public uint EntryPoint { get; set; } = 0;

        /// <summary>
        /// Gets or sets the offset to the program header.
        /// </summary>
        public uint ProgramHeaderTableOffset { get; set; } = 0;

        /// <summary>
        /// Gets or sets the offset to the section header.
        /// </summary>
        public uint SectionHeaderTableOffset { get; set; } = 0;

        /// <summary>
        /// Gets or sets the flags.
        /// </summary>
        public uint Flags { get; set; } = 0;

        /// <summary>
        /// Gets or sets the size of a program header entry.
        /// </summary>
        public ushort ProgramHeaderEntrySize { get; set; } = 0;

        /// <summary>
        /// Gets or sets the number of program header entries.
        /// </summary>
        public ushort ProgramHeaderEntryCount { get; set; } = 0;

        /// <summary>
        /// Gets or sets the size of a section header entry.
        /// </summary>
        public ushort SectionHeaderEntrySize { get; set; } = 0;

        /// <summary>
        /// Gets or sets the number of section header entries.
        /// </summary>
        public ushort SectionHeaderEntryCount { get; set; } = 0;

        /// <summary>
        /// Gets or sets the index of the section that has the names of the sections.
        /// </summary>
        public ushort SectionHeaderStringIndex { get; set; } = 0;

        /// <inheritdoc/>
        public void Serialize(BinaryWriter writer)
        {
            writer.WriteBytes(this.ident);
            writer.WriteUInt16((ushort)this.Type);
            writer.WriteUInt16((ushort)this.Machine);
            writer.WriteUInt32((uint)this.Version);
            writer.WriteUInt32(this.EntryPoint);
            writer.WriteUInt32(this.ProgramHeaderTableOffset);
            writer.WriteUInt32(this.SectionHeaderTableOffset);
            writer.WriteUInt32(this.Flags);
            writer.WriteUInt16(BinarySize);
            writer.WriteUInt16(this.ProgramHeaderEntrySize);
            writer.WriteUInt16(this.ProgramHeaderEntryCount);
            writer.WriteUInt16(this.SectionHeaderEntrySize);
            writer.WriteUInt16(this.SectionHeaderEntryCount);
            writer.WriteUInt16(this.SectionHeaderStringIndex);
        }
    }
}