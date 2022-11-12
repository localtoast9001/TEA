//-----------------------------------------------------------------------
// <copyright file="SectionFlags.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler.Elf
{
    using System;

    /// <summary>
    /// Section flags.
    /// </summary>
    [Flags]
    internal enum SectionFlags : uint
    {
        /// <summary>
        /// No flags set.
        /// </summary>
        None = 0,

        /// <summary>
        /// Writable.
        /// </summary>
        Write = 1,

        /// <summary>
        /// Occupies memory during execution.
        /// </summary>
        Alloc = 2,

        /// <summary>
        /// Executable.
        /// </summary>
        ExecInstr = 4,

        /// <summary>
        /// Might be merged.
        /// </summary>
        Merge = 0x10,

        /// <summary>
        /// Contains null-terminated strings.
        /// </summary>
        Strings = 0x20,

        /// <summary>
        /// 'sh_info' contains SHT index.
        /// </summary>
        InfoLink = 0x40,

        /// <summary>
        /// Preserve order after combining.
        /// </summary>
        LinkOrder = 0x80,

        /// <summary>
        /// Non-standard OS specific handling required.
        /// </summary>
        OSNonConforming = 0x100,

        /// <summary>
        /// Section is member of a group.
        /// </summary>
        Group = 0x200,

        /// <summary>
        /// Section hold thread-local data.
        /// </summary>
        Tls = 0x400,

        /// <summary>
        /// OS-specific mask.
        /// </summary>
        MaskOS = 0x0FF00000,

        /// <summary>
        /// Processor-specific mask.
        /// </summary>
        MaskProc = 0xF0000000,

        /// <summary>
        /// Special ordering requirement (Solaris).
        /// </summary>
        Ordered = 0x4000000,

        /// <summary>
        /// Section is excluded unless referenced or allocated (Solaris).
        /// </summary>
        Exclude = 0x8000000,
    }
}