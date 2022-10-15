//-----------------------------------------------------------------------
// <copyright file="LiteralExpression.cs" company="Jon Rowlett">
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
    /// Literal Expression parse node.
    /// </summary>
    public class LiteralExpression : Expression
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LiteralExpression"/> class.
        /// </summary>
        /// <param name="start">The first token in the parse node.</param>
        /// <param name="value">The value of the literal.</param>
        public LiteralExpression(Token start, object? value)
            : base(start)
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets the literal value.
        /// </summary>
        public object? Value { get; }
    }
}
