//-----------------------------------------------------------------------
// <copyright file="ArrayIndexReferenceExpression.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TEAC
{
    class ArrayIndexReferenceExpression : ReferenceExpression
    {
        public ArrayIndexReferenceExpression(Token start, ReferenceExpression inner, Expression index)
            : base(start)
        {
            this.Inner = inner;
            this.Index = index;
        }

        public ReferenceExpression Inner { get; }
        public Expression Index { get; }
    }
}