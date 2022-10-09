//-----------------------------------------------------------------------
// <copyright file="DereferenceExpression.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TEAC
{
    class DereferenceExpression : ReferenceExpression
    {
        public DereferenceExpression(Token start, ReferenceExpression inner)
            : base(start)
        {
            this.Inner = inner;
        }

        public ReferenceExpression Inner { get; }
    }
}