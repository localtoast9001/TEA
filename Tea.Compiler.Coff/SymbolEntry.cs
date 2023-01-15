// <copyright file="SymbolEntry.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>

namespace Tea.Compiler.Coff
{
    using Tea.Compiler.Binary;

    /// <summary>
    /// Serialized symbol representation in the file.
    /// </summary>
    internal class SymbolEntry : ISerializable
    {
        private const int NameSize = 8;

        /// <summary>
        /// Gets the name bytes.
        /// </summary>
        /// <remarks>
        /// Not null terminated, but null padded. UTF-8.
        /// </remarks>
        public byte[] Name { get; } = new byte[NameSize];

        /// <summary>
        /// Gets or sets the value of the symbol.
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Gets or sets the section index.
        /// </summary>
        public short SectionNumber { get; set; }

        /// <summary>
        /// Gets or sets the symbol type.
        /// </summary>
        public ushort Type { get; set; }

        /// <summary>
        /// Gets or sets the storage class.
        /// </summary>
        public byte StorageClass { get; set; }

        /// <summary>
        /// Gets or sets the auxiliary count.
        /// </summary>
        public byte AuxCount { get; set; }

        /// <inheritdoc/>
        public void Serialize(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
