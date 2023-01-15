// <copyright file="Rel.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>

namespace Tea.Compiler.Coff
{
    using Tea.Compiler.Binary;

    /// <summary>
    /// Relocation serialized structure.
    /// </summary>
    internal class Rel : ISerializable
    {
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
        public ushort Type { get; set; }

        /// <inheritdoc/>
        public void Serialize(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
