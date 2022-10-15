//-----------------------------------------------------------------------
// <copyright file="AssignmentStatement.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Language
{
    /// <summary>
    /// Assignment statement parse node.
    /// </summary>
    public class AssignmentStatement : Statement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssignmentStatement"/> class.
        /// </summary>
        /// <param name="start">The first token in the parse node.</param>
        /// <param name="storage">The left hand side of the expression.</param>
        /// <param name="value">The right hand side of the expression.</param>
        public AssignmentStatement(Token start, ReferenceExpression storage, Expression value)
            : base(start)
        {
            this.Value = value;
            this.Storage = storage;
        }

        /// <summary>
        /// Gets the right hand side of the expression.
        /// </summary>
        public Expression Value { get; }

        /// <summary>
        /// Gets the left hand side of the expression.
        /// </summary>
        public ReferenceExpression Storage { get; }
    }
}
