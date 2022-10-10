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

    internal class BlockStatement : Statement
    {
        private List<Statement> statements = new List<Statement>();

        public BlockStatement(Token start)
            : base(start)
        {
        }

        public IEnumerable<Statement> Statements
        {
            get { return this.statements; }
        }

        public void AddStatement(Statement statement)
        {
            this.statements.Add(statement);
        }
    }
}
