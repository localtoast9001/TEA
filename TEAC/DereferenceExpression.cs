//-----------------------------------------------------------------------
// <copyright file="DereferenceExpression.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TEAC
{
    /// <summary>
    /// Dereference reference expression.
    /// </summary>
    internal class DereferenceExpression : ReferenceExpression
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DereferenceExpression"/> class.
        /// </summary>
        /// <param name="start">The first token in the parse node.</param>
        /// <param name="inner">The inner expression.</param>
        public DereferenceExpression(Token start, ReferenceExpression inner)
            : base(start)
        {
            this.Inner = inner ?? throw new ArgumentNullException(nameof(inner));
        }

        /// <summary>
        /// Gets the inner exprssion.
        /// </summary>
        public ReferenceExpression Inner { get; }
    }
}