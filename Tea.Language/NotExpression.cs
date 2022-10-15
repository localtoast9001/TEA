//-----------------------------------------------------------------------
// <copyright file="NotExpression.cs" company="Jon Rowlett">
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
    /// Logical not expression.
    /// </summary>
    public class NotExpression : Expression
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotExpression"/> class.
        /// </summary>
        /// <param name="start">The first token in the parse node.</param>
        /// <param name="inner">The inner expression.</param>
        public NotExpression(Token start, Expression inner)
            : base(start)
        {
            this.Inner = inner ?? throw new ArgumentNullException(nameof(inner));
        }

        /// <summary>
        /// Gets the inner expression.
        /// </summary>
        public Expression Inner { get; }
    }
}
