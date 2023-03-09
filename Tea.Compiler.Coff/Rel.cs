// <copyright file="Rel.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>

namespace Tea.Compiler.Coff
{
    using Tea.Compiler.Binary;

    /// <summary>
    /// The type of relocation.
    /// </summary>
    /// <remarks>
    /// Information comes from http://www.delorie.com/djgpp/doc/coff/reloc.html.
    /// </remarks>
    internal enum RelType : ushort
    {
        /// <summary>
        /// No type or default (invalid).
        /// </summary>
        None = 0,

        /// <summary>
        /// 32-bit absolute reference.
        /// </summary>
        Addr32 = 6,

        /// <summary>
        /// 32-bit relative reference.
        /// </summary>
        Rel32 = 20,
    }

    /// <summary>
    /// Relocation serialized structure.
    /// </summary>
    internal class Rel : ISerializable
    {
        /// <summary>
        /// The size of the structure in bytes when serialized.
        /// </summary>
        public const int BinarySize = 0x0a;

        /// <summary>
        /// Gets or sets the section relative address of the relocation.
        /// </summary>
        public uint Addr { get; set; }

        /// <summary>
        /// Gets or sets the index of the symbol for the relocation.
        /// </summary>
        public uint SymbolIndex { get; set; }

        /// <summary>
        /// Gets or sets the relocation type.
        /// </summary>
        public RelType Type { get; set; }

        /// <inheritdoc/>
        public void Serialize(BinaryWriter writer)
        {
            writer.WriteUInt32(this.Addr);
            writer.WriteUInt32(this.SymbolIndex);
            writer.WriteUInt16((ushort)this.Type);
        }
    }
}
