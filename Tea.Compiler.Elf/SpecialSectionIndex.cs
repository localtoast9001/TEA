//-----------------------------------------------------------------------
// <copyright file="SpecialSectionIndex.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler.Elf;

/// <summary>
/// Special section indexes used internally.
/// </summary>
internal enum SpecialSectionIndex : ushort
{
    /// <summary>
    /// An undefined, missing, irrelevant, or otherwise meaningless section reference. For example, a symbol defined relative to section number SHN_UNDEF is an undefined symbol.
    /// </summary>
    Undef = 0,

    /// <summary>
    /// The lower boundary of the range of reserved indexes.
    /// </summary>
    LoReserve = 0xff00,

    /// <summary>
    /// x64 specific common block label. This label is similar to SHN_COMMON, but provides for identifying a large common block.
    /// </summary>
    Amd64LCommon = 0xff02,

    /// <summary>
    /// Absolute values for the corresponding reference. For example, symbols defined relative to section number SHN_ABS have absolute values and are not affected by relocation.
    /// </summary>
    Abs = 0xfff1,

    /// <summary>
    /// Symbols defined relative to this section are common symbols, such as FORTRAN COMMON or unallocated C external variables. These symbols are sometimes referred to as tentative.
    /// </summary>
    Common = 0xfff2,

    /// <summary>
    /// An escape value indicating that the actual section header index is too large to fit in the containing field. The header section index is found in another location specific to the structure where the section index appears.
    /// </summary>
    XIndex = 0xffff,

    /// <summary>
    /// The upper boundary of the range of reserved indexes. The system reserves indexes between SHN_LORESERVE and SHN_HIRESERVE, inclusive. The values do not reference the section header table. The section header table does not contain entries for the reserved indexes.
    /// </summary>
    HiReserve = 0xffff,
}