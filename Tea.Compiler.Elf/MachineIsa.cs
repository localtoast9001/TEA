//-----------------------------------------------------------------------
// <copyright file="MachineIsa.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler.Elf
{
    /// <summary>
    /// Machine Instruction set architecture.
    /// </summary>
    public enum MachineIsa : ushort
    {
        /// <summary>
        /// No machine ISA.
        /// </summary>
        None,

        /// <summary>
        /// Bellmac 32 machine ISA.
        /// </summary>
        Bellmac32,

        /// <summary>
        /// Sun Sparc machine ISA.
        /// </summary>
        Sparc,

        /// <summary>
        /// X86 machine ISA.
        /// </summary>
        X86,
    }
}