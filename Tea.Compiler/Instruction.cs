//-----------------------------------------------------------------------
// <copyright file="Instruction.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Base class for instructions from different architectures.
    /// </summary>
    public abstract class Instruction
    {
        /// <summary>
        /// Gets the set of relocation entries for this instruction.
        /// </summary>
        public virtual IEnumerable<RelocationEntry> RelocationEntries => Array.Empty<RelocationEntry>();

        /// <summary>
        /// Converts the instruction to an array of bytes.
        /// </summary>
        /// <returns>The little-endian machine code.</returns>
        public abstract byte[] ToArray();
    }
}