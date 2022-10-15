//-----------------------------------------------------------------------
// <copyright file="VarBlock.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Parse node for a variable block..
    /// </summary>
    public class VarBlock : ParseNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VarBlock"/> class.
        /// </summary>
        /// <param name="start">The first token in the parse node.</param>
        public VarBlock(Token start)
            : base(start)
        {
            this.Variables = new List<VariableDeclaration>();
        }

        /// <summary>
        /// Gets the list of variables.
        /// </summary>
        public IList<VariableDeclaration> Variables { get; }
    }
}
