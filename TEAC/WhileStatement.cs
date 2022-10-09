//-----------------------------------------------------------------------
// <copyright file="WhileStatement.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TEAC
{
    /// <summary>
    /// Parse node for a while statement.
    /// </summary>
    class WhileStatement : Statement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WhileStatement"/> class.
        /// </summary>
        /// <param name="start">The first token of the parse node.</param>
        public WhileStatement(Token start)
            : base(start)
        {
        }

        /// <summary>
        /// Gets or sets the condition for the while loop.
        /// </summary>
        public Expression? Condition { get; set; }

        /// <summary>
        /// Gets or sets the body of the while loop.
        /// </summary>
        public Statement? BodyStatement { get; set; }
    }
}
