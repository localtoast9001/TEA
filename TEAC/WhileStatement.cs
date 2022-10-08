using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEAC
{
    class WhileStatement : Statement
    {
        public WhileStatement(Token start)
            : base(start)
        {
        }

        public Expression? Condition { get; set; }

        public Statement? BodyStatement { get; set; }
    }
}
