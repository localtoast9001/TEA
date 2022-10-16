//-----------------------------------------------------------------------
// <copyright file="LocalVariable.cs" company="Jon Rowlett">
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
    /// Symbol entry for a local variable.
    /// </summary>
    public class LocalVariable : SymbolEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalVariable"/> class.
        /// </summary>
        /// <param name="name">The variable name.</param>
        public LocalVariable(string name)
            : base(name)
        {
        }

        /// <summary>
        /// Gets or sets the offset on the frame.
        /// </summary>
        public int Offset { get; set; }
    }
}
