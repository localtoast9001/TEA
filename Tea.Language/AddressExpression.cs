//-----------------------------------------------------------------------
// <copyright file="AddressExpression.cs" company="Jon Rowlett">
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
    /// Expression to reference the address of something.
    /// </summary>
    public class AddressExpression : Expression
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddressExpression"/> class.
        /// </summary>
        /// <param name="start">First token in the parse node.</param>
        /// <param name="inner">The inner expression.</param>
        public AddressExpression(Token start, ReferenceExpression inner)
            : base(start)
        {
            this.Inner = inner ?? throw new ArgumentNullException(nameof(inner));
        }

        /// <summary>
        /// Gets the inner expression.
        /// </summary>
        public ReferenceExpression Inner { get; }
    }
}
