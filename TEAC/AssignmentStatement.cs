//-----------------------------------------------------------------------
// <copyright file="AssignmentStatement.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TEAC
{
    internal class AssignmentStatement : Statement
    {
        public AssignmentStatement(Token start, ReferenceExpression storage, Expression value)
            : base(start)
        {
            this.Value = value;
            this.Storage = storage;
        }

        public Expression Value { get; }
        public ReferenceExpression Storage { get; }
    }
}
