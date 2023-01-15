// <copyright file="Machine.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>

namespace Tea.Compiler.Coff
{
    /// <summary>
    /// Machine architecture for the COFF image file.
    /// </summary>
    public enum Machine
    {
        /// <summary>
        /// Intel x86 architecture.
        /// </summary>
        I386,

        /// <summary>
        /// Intel Itanium architecture.
        /// </summary>
        IA64,

        /// <summary>
        /// AMD64 architecture.
        /// </summary>
        Amd64,
    }
}
