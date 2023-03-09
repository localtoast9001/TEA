//-----------------------------------------------------------------------
// <copyright file="Elf32SymbolEntry.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler.Elf
{
    using Tea.Compiler.Binary;

    /// <summary>
    /// An entry in a 32bit ELF symbol table.
    /// </summary>
    internal struct Elf32SymbolEntry : ISerializable
    {
        /// <summary>
        /// The binary size when serialized.
        /// </summary>
        public const uint BinarySize = 0x10;

        /// <summary>
        /// Gets or sets the offset in the string table for the name.
        /// </summary>
        public uint Name { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public uint Value { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        public uint Size { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public SymbolType Type { get; set; }

        /// <summary>
        /// Gets or sets the binding.
        /// </summary>
        public SymbolBinding Binding { get; set; }

        /// <summary>
        /// Gets or sets the other byte. TODO: Explain.
        /// </summary>
        public byte Other { get; set; }

        /// <summary>
        /// Gets or sets the section header index.
        /// </summary>
        public ushort SectionHeaderIndex { get; set; }

        /// <inheritdoc/>
        public void Serialize(BinaryWriter writer)
        {
            writer.WriteUInt32(this.Name);
            writer.WriteUInt32(this.Value);
            writer.WriteUInt32(this.Size);
            writer.WriteByte((byte)((uint)this.Type | ((uint)this.Binding << 4)));
            writer.WriteByte(this.Other);
            writer.WriteUInt16(this.SectionHeaderIndex);
        }
    }
}