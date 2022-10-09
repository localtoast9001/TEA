//-----------------------------------------------------------------------
// <copyright file="RelationalExpression.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEAC
{
    class RelationalExpression : Expression
    {
        public RelationalExpression(
            Token start,
            Expression left,
            Keyword oper,
            Expression right)
            : base(start)
        {
            this.Left = left;
            this.Operator = oper;
            this.Right = right;
        }

        public Expression Left { get; }

        public Keyword Operator { get; }

        public Expression Right { get; }
    }
}
