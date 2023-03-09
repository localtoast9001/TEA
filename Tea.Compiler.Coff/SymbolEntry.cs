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
        /// <summary>
        /// The size of the structure in bytes when serialized.
        /// </summary>
        public const int BinarySize = 0x12;

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
        public SymbolType Type { get; set; }

        /// <summary>
        /// Gets or sets the storage class.
        /// </summary>
        public StorageClass StorageClass { get; set; }

        /// <summary>
        /// Gets or sets the auxiliary data.
        /// </summary>
        public byte[] AuxData { get; set; } = Array.Empty<byte>();

        /// <summary>
        /// Gets the number of auxiliary entries occupied by the auxiliary data.
        /// </summary>
        public byte AuxCount => (byte)((this.AuxData.Length + BinarySize - 1) / BinarySize);

        /// <inheritdoc/>
        public void Serialize(BinaryWriter writer)
        {
            writer.WriteBytes(this.Name);
            writer.WriteInt32(this.Value);
            writer.WriteInt16(this.SectionNumber);
            writer.WriteUInt16(this.Type.Value);
            writer.WriteByte((byte)this.StorageClass);

            byte auxCount = this.AuxCount;
            writer.WriteByte(auxCount);
            if (auxCount > 0)
            {
                writer.WriteBytes(this.AuxData);

                int padding = (auxCount * BinarySize) - this.AuxData.Length;
                writer.Skip((uint)padding);
            }
        }
    }
}
