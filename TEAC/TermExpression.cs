//-----------------------------------------------------------------------
// <copyright file="TermExpression.cs" company="Jon Rowlett">
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
    internal class TermExpression : Expression
    {
        public TermExpression(
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

        public Expression Left { get; private set; }

        public Keyword Operator { get; private set; }

        public Expression Right { get; private set; }

    }
}
