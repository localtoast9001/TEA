//-----------------------------------------------------------------------
// <copyright file="SectionHeaderEntry32.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler.Elf
{
    /// <summary>
    /// Section header entry.
    /// </summary>
    internal class SectionHeaderEntry32 : ISerializable
    {
        /// <summary>
        /// The binary size of an entry when serialized.
        /// </summary>
        internal const uint BinarySize = 0x28;

        /// <summary>
        /// Gets or sets the offset to the name of the section in the string table.
        /// </summary>
        public uint NameOffset { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public SectionType Type { get; set; }

        /// <summary>
        /// Gets or sets the flags.
        /// </summary>
        public SectionFlags Flags { get; set; }

        /// <summary>
        /// Gets or sets the virtual address of the section in memory, for sections that are loaded.
        /// </summary>
        public uint Addr { get; set; }

        /// <summary>
        /// Gets or sets the offset of the section in the file image.
        /// </summary>
        public uint Offset { get; set; }

        /// <summary>
        /// Gets or sets the size in bytes of the section in the file image. May be 0.
        /// </summary>
        public uint Size { get; set; }

        /// <summary>
        /// Gets or sets the section index of an associated section. This field is used for several purposes, depending on the type of section.
        /// </summary>
        public uint Link { get; set; }

        /// <summary>
        /// Gets or sets extra information about the section. This field is used for several purposes, depending on the type of section.
        /// </summary>
        public uint Info { get; set; }

        /// <summary>
        /// Gets or sets the required alignment of the section. This field must be a power of two.
        /// </summary>
        public uint AddrAlign { get; set; }

        /// <summary>
        /// Gets or sets the size, in bytes, of each entry, for sections that contain fixed-size entries. Otherwise, this field contains zero.
        /// </summary>
        public uint EntrySize { get; set; }

        /// <inheritdoc/>
        public void Serialize(BinaryWriter writer)
        {
            writer.WriteUInt32(this.NameOffset);
            writer.WriteUInt32((uint)this.Type);
            writer.WriteUInt32((uint)this.Flags);
            writer.WriteUInt32(this.Addr);
            writer.WriteUInt32(this.Offset);
            writer.WriteUInt32(this.Size);
            writer.WriteUInt32(this.Link);
            writer.WriteUInt32(this.Info);
            writer.WriteUInt32(this.AddrAlign);
            writer.WriteUInt32(this.EntrySize);
        }
    }
}