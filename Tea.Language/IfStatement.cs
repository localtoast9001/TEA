//-----------------------------------------------------------------------
// <copyright file="IfStatement.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Language
{
    /// <summary>
    /// If statement parse node.
    /// </summary>
    public class IfStatement : Statement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IfStatement"/> class.
        /// </summary>
        /// <param name="start">The first token in the parse node.</param>
        public IfStatement(Token start)
            : base(start)
        {
        }

        /// <summary>
        /// Gets or sets the condition.
        /// </summary>
        public Expression? Condition { get; set; }

        /// <summary>
        /// Gets or sets the statement to execute if the condition is true.
        /// </summary>
        public Statement? TrueStatement { get; set; }

        /// <summary>
        /// Gets or sets the statement to execute if the condition is false.
        /// </summary>
        public Statement? FalseStatement { get; set; }
    }
}
