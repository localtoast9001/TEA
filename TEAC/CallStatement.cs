//-----------------------------------------------------------------------
// <copyright file="CallStatement.cs" company="Jon Rowlett">
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
    /// Procedure or function call statement.
    /// </summary>
    internal class CallStatement : Statement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CallStatement"/> class.
        /// </summary>
        /// <param name="start">The first token in the parse node.</param>
        /// <param name="callExpression">The call expression.</param>
        public CallStatement(Token start, ReferenceExpression callExpression)
            : base(start)
        {
            this.Expression = callExpression;
        }

        /// <summary>
        /// Gets the call expression.
        /// </summary>
        public ReferenceExpression Expression { get; }
    }
}
