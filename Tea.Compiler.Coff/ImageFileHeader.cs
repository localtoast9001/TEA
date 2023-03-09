// <copyright file="ImageFileHeader.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>

namespace Tea.Compiler.Coff
{
    using Tea.Compiler.Binary;

    /// <summary>
    /// COFF header.
    /// </summary>
    internal class ImageFileHeader : ISerializable
    {
        /// <summary>
        /// The size of the header when serialized.
        /// </summary>
        public const int BinarySize = 0x14;

        /// <summary>
        /// Gets or sets the architecture type of the computer.
        /// </summary>
        public Machine Machine { get; set; }

        /// <summary>
        /// Gets or sets the number of sections.
        /// </summary>
        public ushort NumberOfSections { get; set; }

        /// <summary>
        /// Gets or sets the date time stamp (UNIX time).
        /// </summary>
        public DateTime TimeDateStamp { get; set; }

        /// <summary>
        /// Gets or sets the file offset to the symbol table.
        /// </summary>
        public uint PointerToSymbolTable { get; set; }

        /// <summary>
        /// Gets or sets the number of symbols.
        /// </summary>
        public uint NumberOfSymbols { get; set; }

        /// <summary>
        /// Gets or sets the size of the optional header. For object files, this is 0.
        /// </summary>
        public ushort SizeOfOptionalHeader { get; set; }

        /// <summary>
        /// Gets or sets characteristics flags.
        /// </summary>
        public ushort Characteristics { get; set; }

        /// <inheritdoc/>
        public void Serialize(BinaryWriter writer)
        {
            writer.WriteUInt16(this.Machine.ToMagicNumber());
            writer.WriteUInt16(this.NumberOfSections);
            writer.WriteUInt32(this.GetTimeDateStampUnixTime());
            writer.WriteUInt32(this.PointerToSymbolTable);
            writer.WriteUInt32(this.NumberOfSymbols);
            writer.WriteUInt16(this.SizeOfOptionalHeader);
            writer.WriteUInt16(this.Characteristics);
        }

        private uint GetTimeDateStampUnixTime()
        {
            DateTimeOffset t = new DateTimeOffset(this.TimeDateStamp.ToUniversalTime());
            return (uint)t.ToUnixTimeSeconds();
        }
    }
}
