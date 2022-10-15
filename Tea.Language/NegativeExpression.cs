//-----------------------------------------------------------------------
// <copyright file="NegativeExpression.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Language
{
    /// <summary>
    /// Negative operator expression.
    /// </summary>
    public class NegativeExpression : Expression
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NegativeExpression"/> class.
        /// </summary>
        /// <param name="start">The first token in the parse node.</param>
        /// <param name="inner">The inner expression.</param>
        public NegativeExpression(Token start, Expression inner)
            : base(start)
        {
            this.Inner = inner;
        }

        /// <summary>
        /// Gets the inner expression.
        /// </summary>
        public Expression Inner { get; }
    }
}
