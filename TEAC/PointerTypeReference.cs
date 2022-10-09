//-----------------------------------------------------------------------
// <copyright file="PointerTypeReference.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TEAC
{
    /// <summary>
    /// Type reference to a pointer of a type.
    /// </summary>
    internal class PointerTypeReference : TypeReference
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PointerTypeReference"/> class.
        /// </summary>
        /// <param name="start">The first token of the parse node.</param>
        /// <param name="elementType">The type that this reference is a pointer of.</param>
        public PointerTypeReference(Token start, TypeReference elementType)
            : base(start)
        {
            this.ElementType = elementType;
        }

        /// <summary>
        /// Gets the reference to the type of which this is the pointer.
        /// </summary>
        public TypeReference ElementType { get; }
    }
}