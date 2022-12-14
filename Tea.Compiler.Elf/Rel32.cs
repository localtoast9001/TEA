//-----------------------------------------------------------------------
// <copyright file="Rel32.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler.Elf
{
    /// <summary>
    /// The type of relocation.
    /// </summary>
    internal enum Rel32Type : byte
    {
        /// <summary>
        /// No relocation
        /// </summary>
        R_386_NONE = 0,

        /// <summary>
        /// Symbol + Offset
        /// </summary>
        R_386_32 = 1,

        /// <summary>
        /// Symbol + Offset - Section Offset
        /// </summary>
        R_386_PC32 = 2,
    }

    /// <summary>
    /// 32-bit entry in a relocation section.
    /// </summary>
    internal struct Rel32 : ISerializable
    {
        /// <summary>
        /// The size when serialized.
        /// </summary>
        internal const uint BinarySize = 0x8;

        /// <summary>
        /// Gets or sets the relocation offset.
        /// </summary>
        public uint Offset { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public Rel32Type Type { get; set; }

        /// <summary>
        /// Gets or sets the index to the symbol that will replace this relocation.
        /// </summary>
        public uint SymbolRef { get; set; }

        /// <inheritdoc/>
        public void Serialize(BinaryWriter writer)
        {
            writer.WriteUInt32(this.Offset);
            uint info = (uint)(this.SymbolRef << 8) | (uint)this.Type;
            writer.WriteUInt32(info);
        }
    }
}