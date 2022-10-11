//-----------------------------------------------------------------------
// <copyright file="BlockStatement.cs" company="Jon Rowlett">
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
    /// A block statement.
    /// </summary>
    internal class BlockStatement : Statement
    {
        private readonly List<Statement> statements = new List<Statement>();

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockStatement"/> class.
        /// </summary>
        /// <param name="start">The first token in the parse node.</param>
        public BlockStatement(Token start)
            : base(start)
        {
        }

        /// <summary>
        /// Gets the statements.
        /// </summary>
        public IEnumerable<Statement> Statements
        {
            get { return this.statements; }
        }

        /// <summary>
        /// Adds a statement.
        /// </summary>
        /// <param name="statement">The statement to add.</param>
        public void AddStatement(Statement statement)
        {
            this.statements.Add(statement);
        }
    }
}
