using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEAC
{
    internal class DeleteStatement : Statement
    {
        public DeleteStatement(Token start, Expression value)
            : base(start)
        {
            this.Value = value;
        }

        public Expression Value { get; }
    }
}
