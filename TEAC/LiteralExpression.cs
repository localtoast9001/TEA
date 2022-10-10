//-----------------------------------------------------------------------
// <copyright file="LiteralExpression.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TEAC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    class LiteralExpression : Expression
    {
        public LiteralExpression(Token start, object? value)
            : base(start)
        {
            this.Value = value;
        }

        public object? Value { get; }
    }
}
