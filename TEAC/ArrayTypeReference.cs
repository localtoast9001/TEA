//-----------------------------------------------------------------------
// <copyright file="ArrayTypeReference.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TEAC
{
    /// <summary>
    /// Array type reference.
    /// </summary>
    internal class ArrayTypeReference : TypeReference
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayTypeReference"/> class.
        /// </summary>
        /// <param name="start">The first token in the parse node.</param>
        /// <param name="elementCount">The expression for the element count.</param>
        /// <param name="elementType">The element type.</param>
        public ArrayTypeReference(Token start, Expression elementCount, TypeReference elementType)
            : base(start)
        {
            this.ElementCount = elementCount;
            this.ElementType = elementType;
        }

        /// <summary>
        /// Gets the element count.
        /// </summary>
        public Expression ElementCount { get; }

        /// <summary>
        /// Gets the element type.
        /// </summary>
        public TypeReference ElementType { get; }
    }
}