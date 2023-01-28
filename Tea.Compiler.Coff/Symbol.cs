// <copyright file="Symbol.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>

namespace Tea.Compiler.Coff
{
    /// <summary>
    /// A symbol that references a location in a <see cref="ProgramSection" />.
    /// </summary>
    public class Symbol
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Symbol" /> class.
        /// </summary>
        /// <param name="name">The symbol name.</param>
        /// <param name="offset">The offset of the symbol in the section.</param>
        /// <param name="global">True if the symbol is global; otherwise, false.</param>
        internal Symbol(string name, long offset, bool global)
        {
            this.Name = name;
            this.Offset = offset;
            this.Global = global;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the offset of the symbol in the section.
        /// </summary>
        public long Offset { get; }

        /// <summary>
        /// Gets a value indicating whether the symbol is global.
        /// </summary>
        public bool Global { get; }
    }
}
