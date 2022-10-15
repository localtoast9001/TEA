//-----------------------------------------------------------------------
// <copyright file="Expression.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Language
{
    /// <summary>
    /// Abstract expression parse node.
    /// </summary>
    public abstract class Expression : ParseNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Expression"/> class.
        /// </summary>
        /// <param name="start">The first token in the parse node.</param>
        protected Expression(Token start)
            : base(start)
        {
        }
    }
}
