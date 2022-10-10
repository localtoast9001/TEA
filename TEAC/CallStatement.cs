//-----------------------------------------------------------------------
// <copyright file="CallStatement.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TEAC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal class CallStatement : Statement
    {
        public CallStatement(Token start, ReferenceExpression callExpression)
            : base(start)
        {
            this.Expression = callExpression;
        }

        public ReferenceExpression Expression { get; }
    }
}
