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
        /// <summary>
        /// No type.
        /// </summary>
        None,

        /// <summary>
        /// Relocatable objects.
        /// </summary>
        Rel,

        /// <summary>
        /// Executable code.
        /// </summary>
        Exec,

        /// <summary>
        /// dynamic module.
        /// </summary>
        Dyn,

        /// <summary>
        /// Core binary.
        /// </summary>
        Core,
    }
}