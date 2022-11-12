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
        None,
        Bellmac32,
        Sparc,
        X86,
    }
}