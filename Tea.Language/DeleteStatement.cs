//-----------------------------------------------------------------------
// <copyright file="DeleteStatement.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Language
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Deletee statement parse node.
    /// </summary>
    public class DeleteStatement : Statement
    {
        /// <summary>
        /// Initializes a new instance of the <seee cref="DeleteStatement"/> class.
        /// </summary>
        /// <param name="start">The first token in the parse node.</param>
        /// <param name="value">The expression for the object to delete.</param>
        public DeleteStatement(Token start, Expression value)
            : base(start)
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets the expression for the object to delete.
        /// </summary>
        public Expression Value { get; }
    }
}
