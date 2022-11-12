//-----------------------------------------------------------------------
// <copyright file="ElfType.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler.Elf
{
    /// <summary>
    /// The type of ELF file.
    /// </summary>
    public enum ElfType : ushort
    {
        None,
        Rel,
        Exec,
        Dyn,
        Core,
    }
}