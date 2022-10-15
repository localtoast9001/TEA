//-----------------------------------------------------------------------
// <copyright file="TermExpression.cs" company="Jon Rowlett">
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
    /// A term expression from the grammar.
    /// </summary>
    public class TermExpression : Expression
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TermExpression"/> class.
        /// </summary>
        /// <param name="start">The first token in the parse node.</param>
        /// <param name="left">The left side expression.</param>
        /// <param name="oper">The operator.</param>
        /// <param name="right">The right side expression.</param>
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

        /// <summary>
        /// Gets the left expression.
        /// </summary>
        public Expression Left { get; }

        /// <summary>
        /// Gets the operator.
        /// </summary>
        public Keyword Operator { get; }

        /// <summary>
        /// Gets the right expression.
        /// </summary>
        public Expression Right { get; }
    }
}
