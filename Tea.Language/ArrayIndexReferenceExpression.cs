//-----------------------------------------------------------------------
// <copyright file="ArrayIndexReferenceExpression.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Language
{
    /// <summary>
    /// Expression to index an array.
    /// </summary>
    public class ArrayIndexReferenceExpression : ReferenceExpression
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayIndexReferenceExpression"/> class.
        /// </summary>
        /// <param name="start">The first token in the parse node.</param>
        /// <param name="inner">The inner expression.</param>
        /// <param name="index">The index expression.</param>
        public ArrayIndexReferenceExpression(
            Token start,
            ReferenceExpression inner,
            Expression index)
            : base(start)
        {
            this.Inner = inner;
            this.Index = index;
        }

        /// <summary>
        /// Gets the inner reference to the array.
        /// </summary>
        public ReferenceExpression Inner { get; }

        /// <summary>
        /// Gets the index expression.
        /// </summary>
        public Expression Index { get; }
    }
}