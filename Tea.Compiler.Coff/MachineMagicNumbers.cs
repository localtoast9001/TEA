// <copyright file="MachineMagicNumbers.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>

namespace Tea.Compiler.Coff
{
    /// <summary>
    /// Magic numbers for different machines that appear in the header.
    /// </summary>
    internal static class MachineMagicNumbers
    {
        /// <summary>
        /// The header magic number for X86 (32-bit) files.
        /// </summary>
        public const ushort I386 = 0x014C;

        /// <summary>
        /// The header magic number for I860.
        /// </summary>
        public const ushort I860 = 0x14d;

        /// <summary>
        /// The header magic number for MIPS R3000.
        /// </summary>
        public const ushort MIPSR3000 = 0x162;

        /// <summary>
        /// The header magic number for MIPS R4000.
        /// </summary>
        public const ushort MIPSR4000 = 0x166;

        /// <summary>
        /// The header magic number for DEC Alpha AXP.
        /// </summary>
        public const ushort DECAlphaAXP = 0x183;

        /// <summary>
        /// The header magic number for Itanium files.
        /// </summary>
        public const ushort IA64 = 0x8664;

        /// <summary>
        /// The header magic number for AMD64 files.
        /// </summary>
        public const ushort AMD64 = 0x0200;

        private static readonly ushort[] MagicNumberLookup = new[]
        {
            I386,
            IA64,
            AMD64,
        };

        /// <summary>
        /// Converts a machine to its magic number.
        /// </summary>
        /// <param name="m">The machine to convert.</param>
        /// <returns>The magic number for the machine.</returns>
        public static ushort ToMagicNumber(this Machine m)
        {
            return MagicNumberLookup[(int)m];
        }
    }
}