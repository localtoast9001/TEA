//-----------------------------------------------------------------------
// <copyright file="SectionType.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler.Elf
{
    /// <summary>
    /// ELF section type.
    /// </summary>
    public enum SectionType : uint
    {
        /// <summary>
        /// Null or undefined section type.
        /// </summary>
        Null = 0,

        /// <summary>
        /// Program bits section type.
        /// </summary>
        ProgBits = 1,

        /// <summary>
        /// Symbol table section type.
        /// </summary>
        SymTab = 2,

        /// <summary>
        /// String table section type.
        /// </summary>
        StrTab = 3,

        /// <summary>
        /// Relocation table with append section type.
        /// </summary>
        RelA = 4,

        /// <summary>
        /// Some hash.
        /// </summary>
        Hash = 5,

        /// <summary>
        /// Dynamic relocation section.
        /// </summary>
        Dynamic = 6,

        /// <summary>
        /// Note section.
        /// </summary>
        Note = 7,

        /// <summary>
        /// No bits (BSS).
        /// </summary>
        NoBits = 8,

        /// <summary>
        /// Relocation section.
        /// </summary>
        Rel = 9,

        /// <summary>
        /// Shared lib section.
        /// </summary>
        ShLib = 10,

        /// <summary>
        /// Dynamic symbols section.
        /// </summary>
        DynSym = 11,

        /// <summary>
        /// Init array section.
        /// </summary>
        InitArray = 0xe,

        /// <summary>
        /// Fini array section.
        /// </summary>
        FiniArray = 0xf,

        /// <summary>
        /// Pre-init array section.
        /// </summary>
        PreInitArray = 0x10,

        /// <summary>
        /// Group section.
        /// </summary>
        Group = 0x11,

        /// <summary>
        /// Symbol table shared index.
        /// </summary>
        SymTabShndx = 0x12,

        /// <summary>
        /// Num section.
        /// </summary>
        Num = 0x13,
    }
}