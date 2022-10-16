//-----------------------------------------------------------------------
// <copyright file="SymbolEntry.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Base symbol definition.
    /// </summary>
    public abstract class SymbolEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolEntry"/> class.
        /// </summary>
        /// <param name="name">The symbol name.</param>
        protected SymbolEntry(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets the name of the symbol.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the type of the symbol.
        /// </summary>
        public TypeDefinition? Type { get; set; }
    }
}
