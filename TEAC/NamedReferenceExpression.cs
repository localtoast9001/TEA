//-----------------------------------------------------------------------
// <copyright file="NamedReferenceExpression.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TEAC
{
    internal class NamedReferenceExpression : ReferenceExpression
    {
        public NamedReferenceExpression(Token start, string identifier)
            : base(start)
        {
            this.Identifier = identifier;
        }

        public string Identifier { get; }
    }
}