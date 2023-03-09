// <copyright file="ImageSectionHeader.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>

namespace Tea.Compiler.Coff
{
    using System;
    using Tea.Compiler.Binary;

    /// <summary>
    /// Flags for the image section characteristics member.
    /// </summary>
    [Flags]
    internal enum ImageSectionCharacteristicFlags : uint
    {
        /// <summary>
        /// No flags for the section.
        /// </summary>
        None = 0,

        /// <summary>
        /// IMAGE_SCN_TYPE_NO_PAD. The section should not be padded to the next boundary. This flag is obsolete and is replaced by IMAGE_SCN_ALIGN_1BYTES.
        /// </summary>
        NoPad = 0x8,

        /// <summary>
        /// IMAGE_SCN_CNT_CODE. The section contains executable code.
        /// </summary>
        Code = 0x20,

        /// <summary>
        /// IMAGE_SCN_CNT_INITIALIZED_DATA. The section contains initialized data.
        /// </summary>
        InitializedData = 0x40,

        /// <summary>
        /// IMAGE_SCN_CNT_UNINITIALIZED_DATA. The section contains uninitialized data.
        /// </summary>
        UninitializedData = 0x80,

        /// <summary>
        /// IMAGE_SCN_LNK_OTHER. Reserved.
        /// </summary>
        LinkOther = 0x100,

        /// <summary>
        /// IMAGE_SCN_LNK_INFO. The section contains comments or other information. This is valid only for object files.
        /// </summary>
        LinkInfo = 0x200,

        /// <summary>
        /// IMAGE_SCN_LNK_REMOVE. The section will not become part of the image. This is valid only for object files.
        /// </summary>
        LinkRemove = 0x800,

        /// <summary>
        /// IMAGE_SCN_LNK_COMDAT. The section contains COMDAT data. This is valid only for object files.
        /// </summary>
        LinkComdat = 0x1000,

        /// <summary>
        /// IMAGE_SCN_NO_DEFER_SPEC_EXC. Reset speculative exceptions handling bits in the TLB entries for this section.
        /// </summary>
        NoDeferSpeculativeExceptions = 0x4000,

        /// <summary>
        /// IMAGE_SCN_GPREL. The section contains data referenced through the global pointer.
        /// </summary>
        GPRel = 0x8000,

        /// <summary>
        /// IMAGE_SCN_MEM_PURGEABLE. Reserved.
        /// </summary>
        MemPurgeable = 0x20000,

        /// <summary>
        /// IMAGE_SCN_MEM_LOCKED. Reserved.
        /// </summary>
        MemLocked = 0x40000,

        /// <summary>
        /// IMAGE_SCN_MEM_PRELOAD. Reserved.
        /// </summary>
        MemPreload = 0x80000,

        /// <summary>
        /// IMAGE_SCN_ALIGN_1BYTES. Align data on a 1-byte boundary. This is valid only for object files.
        /// </summary>
        Align1Bytes = 0x100000,

        /// <summary>
        /// IMAGE_SCN_ALIGN_8192BYTES. Align data on a 8192-byte boundary. This is valid only for object files.
        /// </summary>
        Align8192Bytes = 0x00E00000,

        /// <summary>
        /// IMAGE_SCN_LNK_NRELOC_OVFL.
        /// The section contains extended relocations.
        /// </summary>
        /// <remarks>
        /// The count of relocations for the section exceeds the 16 bits that is reserved for it in the section header.
        /// If the NumberOfRelocations field in the section header is 0xffff, the actual relocation count is stored in the VirtualAddress field of the first relocation.
        /// It is an error if IMAGE_SCN_LNK_NRELOC_OVFL is set and there are fewer than 0xffff relocations in the section.
        /// </remarks>
        LinkRelocOverflow = 0x01000000,

        /// <summary>
        /// IMAGE_SCN_MEM_DISCARDABLE. The section can be discarded as needed.
        /// </summary>
        MemDiscardable = 0x02000000,

        /// <summary>
        /// IMAGE_SCN_MEM_NOT_CACHED. The section cannot be cached.
        /// </summary>
        MemNotCached = 0x04000000,

        /// <summary>
        /// IMAGE_SCN_MEM_NOT_PAGED. The section cannot be paged.
        /// </summary>
        MemNotPaged = 0x08000000,

        /// <summary>
        /// IMAGE_SCN_MEM_SHARED. The section can be shared in memory.
        /// </summary>
        MemShared = 0x10000000,

        /// <summary>
        /// IMAGE_SCN_MEM_EXECUTE. The section can be executed as code.
        /// </summary>
        MemExecute = 0x20000000,

        /// <summary>
        /// IMAGE_SCN_MEM_READ. The section can be read.
        /// </summary>
        MemRead = 0x40000000,

        /// <summary>
        /// IMAGE_SCN_MEM_WRITE. The section can be written to.
        /// </summary>
        MemWrite = 0x80000000,
    }

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
        public ImageSectionCharacteristicFlags Characteristics { get; set; }

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
            writer.WriteUInt32((uint)this.Characteristics);
        }
    }
}
