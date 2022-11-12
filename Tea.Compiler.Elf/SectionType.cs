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
        Null = 0,
        ProgBits = 1,
        SymTab = 2,
        StrTab = 3,
        RelA = 4,
        Hash = 5,
        Dynamic = 6,
        Note = 7,
        NoBits = 8,
        Rel = 9,
        ShLib = 10,
        DynSym = 11,
        InitArray = 0xe,
        FiniArray = 0xf,
        PreInitArray = 0x10,
        Group = 0x11,
        SymTabShndx = 0x12,
        Num = 0x13,
    }
}