//-----------------------------------------------------------------------
// <copyright file="Statement.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Language
{
    /// <summary>
    /// Abstract base class for all statement parse nodes.
    /// </summary>
    public abstract class Statement : ParseNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Statement"/> class.
        /// </summary>
        /// <param name="start">The first token in the parse node.</param>
        protected Statement(Token start)
            : base(start)
        {
        }
    }
}
