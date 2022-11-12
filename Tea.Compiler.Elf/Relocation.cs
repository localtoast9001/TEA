//-----------------------------------------------------------------------
// <copyright file="Relocation.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler.Elf
{
    /// <summary>
    /// A relocation inside a section.
    /// </summary>
    public class Relocation
    {
        internal Relocation(uint offset, string symbol)
        {
            this.Offset = offset;
            this.Symbol = symbol;
        }

        /// <summary>
        /// Gets the offset into the section.
        /// </summary>
        public uint Offset { get; }

        /// <summary>
        /// Gets the name of the symbol reference.
        /// </summary>
        public string Symbol { get; }
    }
}