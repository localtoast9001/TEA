//-----------------------------------------------------------------------
// <copyright file="NegativeExpression.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TEAC
{
    internal class NegativeExpression : Expression
    {
        public NegativeExpression(Token start, Expression inner)
            : base(start)
        {
            this.Inner = inner;
        }

        public Expression Inner { get; private set; }
    }
}
