//-----------------------------------------------------------------------
// <copyright file="RelationalExpression.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TEAC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A relational expression.
    /// </summary>
    internal class RelationalExpression : Expression
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RelationalExpression"/> class.
        /// </summary>
        /// <param name="start">The first token in the parse node.</param>
        /// <param name="left">The left hand side expression.</param>
        /// <param name="oper">The relational operator.</param>
        /// <param name="right">The right hand side expression.</param>
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

        /// <summary>
        /// Gets the left hand side of the expression.
        /// </summary>
        public Expression Left { get; }

        /// <summary>
        /// Gets the operator.
        /// </summary>
        public Keyword Operator { get; }

        /// <summary>
        /// Gets the right hand side.
        /// </summary>
        public Expression Right { get; }
    }
}
