using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Expression Value { get; private set; }
        public ReferenceExpression Storage { get; private set; }
    }
}
