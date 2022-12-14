//-----------------------------------------------------------------------
// <copyright file="RelocationEntry.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler
{
    /// <summary>
    /// Relocation reference to a symbol in an <see cref="Instruction"/> or data.
    /// </summary>
    public class RelocationEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RelocationEntry"/> class.
        /// </summary>
        /// <param name="symbol">The symbol name.</param>
        /// <param name="offset">The byte offset into the instruction where the relocation starts.</param>
        /// <param name="size">The size of the relocation entry in bytes.</param>
        /// <param name="relative">True if the relocation is PC relative; otherwise, false.</param>
        public RelocationEntry(string symbol, uint offset, uint size, bool relative)
        {
            this.Symbol = symbol;
            this.Offset = offset;
            this.Size = size;
            this.Relative = relative;
        }

        /// <summary>
        /// Gets the name of the symbol that will replace the entry.
        /// </summary>
        public string Symbol { get; }

        /// <summary>
        /// Gets the byte offset of the relocation in the instruction.
        /// </summary>
        public uint Offset { get; }

        /// <summary>
        /// Gets the size of the relocation entry.
        /// </summary>
        public uint Size { get; }

        /// <summary>
        /// Gets a value indicating whether the relocation is PC relative.
        /// </summary>
        public bool Relative { get; }
    }
}