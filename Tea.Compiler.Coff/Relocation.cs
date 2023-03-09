// <copyright file="Relocation.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>

namespace Tea.Compiler.Coff
{
    /// <summary>
    /// A relocation inside a section.
    /// </summary>
    public class Relocation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Relocation"/> class.
        /// </summary>
        /// <param name="offset">The byte offset into the section.</param>
        /// <param name="relative">True if PC relative; otherwise, false.</param>
        /// <param name="symbol">The symbol reference.</param>
        internal Relocation(uint offset, bool relative, string symbol)
        {
            this.Offset = offset;
            this.Symbol = symbol;
            this.Relative = relative;
        }

        /// <summary>
        /// Gets the offset into the section.
        /// </summary>
        public uint Offset { get; }

        /// <summary>
        /// Gets the name of the symbol reference.
        /// </summary>
        public string Symbol { get; }

        /// <summary>
        /// Gets a value indicating whether the relocation is PC relative.
        /// </summary>
        public bool Relative { get; }
    }
}
