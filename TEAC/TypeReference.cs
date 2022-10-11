//-----------------------------------------------------------------------
// <copyright file="TypeReference.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TEAC
{
    /// <summary>
    /// Abstract type reference parse node.
    /// </summary>
    internal abstract class TypeReference : ParseNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeReference"/> class.
        /// </summary>
        /// <param name="start">The first token in the parse node.</param>
        protected TypeReference(Token start)
            : base(start)
        {
        }
    }
}
