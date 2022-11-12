//-----------------------------------------------------------------------
// <copyright file="Constants.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler.Elf
{
    /// <summary>
    /// Constants used potentially multiple classes.
    /// </summary>
    internal static class Constants
    {
        /// <summary>
        /// Value for a 32 bit ELF.
        /// </summary>
        public const byte Class32Bit = 1;

        /// <summary>
        /// The ELF version.
        /// </summary>
        public const byte ElfVersion = 1;

        /// <summary>
        /// The value to indicate the ELF file is little endian.
        /// </summary>
        public const byte LittleEndian = 1;

        /// <summary>
        /// The ELF magic number.
        /// </summary>
        public static readonly byte[] MagicNumber = { 0x7f, 0x45, 0x4c, 0x46 };
    }
}