// <copyright file="ImageSectionHeader.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>

namespace Tea.Compiler.Coff
{
    using Tea.Compiler.Binary;

    /// <summary>
    /// Image section header structure.
    /// </summary>
    internal class ImageSectionHeader : ISerializable
    {
        /// <summary>
        /// The size of the structure in bytes when serialized.
        /// </summary>
        public const int BinarySize = 0x28;

        private const int ImageSizeOfShortName = 8;

        /// <summary>
        /// Gets the name bytes.
        /// </summary>
        /// <remarks>
        /// Not null terminated, but null padded. UTF-8.
        /// </remarks>
        public byte[] Name { get; } = new byte[ImageSizeOfShortName];

        /// <summary>
        /// Gets or sets the misc union of physical address or virtual size.
        /// </summary>
        /// <remarks>
        /// The total size of the section when loaded into memory, in bytes. If this value is greater than the SizeOfRawData member, the section is filled with zeroes.
        /// This field is valid only for executable images and should be set to 0 for object files.
        /// </remarks>
        public uint MiscPhysicalAddressOrVirtualSize { get; set; }

        /// <summary>
        /// Gets or sets the virtual address.
        /// </summary>
        public uint VirtualAddress { get; set; }

        /// <summary>
        /// Gets or sets the size of raw data.
        /// </summary>
        /// <remarks>
        /// The size of the initialized data on disk, in bytes. This value must be a multiple of the FileAlignment member of the IMAGE_OPTIONAL_HEADER structure.
        /// If this value is less than the VirtualSize member, the remainder of the section is filled with zeroes. If the section contains only uninitialized data, the member is zero.
        /// </remarks>
        public uint SizeOfRawData { get; set; }

        /// <summary>
        /// Gets or sets the pointer to raw data.
        /// </summary>
        /// <remarks>
        /// A file pointer to the first page within the COFF file. This value must be a multiple of the FileAlignment member of the IMAGE_OPTIONAL_HEADER structure.
        /// If a section contains only uninitialized data, set this member is zero.
        /// </remarks>
        public uint PointerToRawData { get; set; }

        /// <summary>
        /// Gets or sets the pointer to relocations.
        /// </summary>
        /// <remarks>
        /// A file pointer to the beginning of the relocation entries for the section. If there are no relocations, this value is zero.
        /// </remarks>
        public uint PointerToRelocations { get; set; }

        /// <summary>
        /// Gets or sets the pointer to line numbers.
        /// </summary>
        /// <remarks>
        /// A file pointer to the beginning of the line-number entries for the section. If there are no COFF line numbers, this value is zero.
        /// </remarks>
        public uint PointerToLineNumbers { get; set; }

        /// <summary>
        /// Gets or sets the number of relocations.
        /// </summary>
        /// <remarks>
        /// The number of relocation entries for the section. This value is zero for executable images.
        /// </remarks>
        public ushort NumberOfRelocations { get; set; }

        /// <summary>
        /// Gets or sets the number of line numbers.
        /// </summary>
        /// <remarks>
        /// The number of line-number entries for the section.
        /// </remarks>
        public ushort NumberOfLineNumbers { get; set; }

        /// <summary>
        /// Gets or sets characteristics flags.
        /// </summary>
        public uint Characteristics { get; set; }

        /// <inheritdoc/>
        public void Serialize(BinaryWriter writer)
        {
            writer.WriteBytes(this.Name);
            writer.WriteUInt32(this.MiscPhysicalAddressOrVirtualSize);
            writer.WriteUInt32(this.VirtualAddress);
            writer.WriteUInt32(this.SizeOfRawData);
            writer.WriteUInt32(this.PointerToRawData);
            writer.WriteUInt32(this.PointerToRelocations);
            writer.WriteUInt32(this.PointerToLineNumbers);
            writer.WriteUInt16(this.NumberOfRelocations);
            writer.WriteUInt16(this.NumberOfLineNumbers);
            writer.WriteUInt32(this.Characteristics);
        }
    }
}
