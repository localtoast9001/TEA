//-----------------------------------------------------------------------
// <copyright file="ArrayTypeReference.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TEAC
{
    internal class ArrayTypeReference : TypeReference
    {
        public ArrayTypeReference(Token start, Expression elementCount, TypeReference elementType)
            : base(start)
        {
            this.ElementCount = elementCount;
            this.ElementType = elementType;
        }

        public Expression ElementCount { get; }

        public TypeReference ElementType { get; }
    }
}