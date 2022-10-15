//-----------------------------------------------------------------------
// <copyright file="SimpleExpression.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Parse node for a simple expression.
    /// </summary>
    public class SimpleExpression : Expression
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleExpression"/> class.
        /// </summary>
        /// <param name="start">The first token in the parse node.</param>
        /// <param name="left">The left side of the expresion.</param>
        /// <param name="oper">The operator of the simple expression.</param>
        /// <param name="right">The right side of the expression.</param>
        public SimpleExpression(
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
        /// Gets the left side of the expression.
        /// </summary>
        public Expression Left { get; }

        /// <summary>
        /// Gets the operator of the expression.
        /// </summary>
        public Keyword Operator { get; }

        /// <summary>
        /// Gets the right side of the expression.
        /// </summary>
        public Expression Right { get; }
    }
}
