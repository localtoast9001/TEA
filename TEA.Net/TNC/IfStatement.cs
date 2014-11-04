using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEAC
{
    class IfStatement : Statement
    {
        public IfStatement(Token start)
            : base(start)
        {
        }

        public Expression Condition { get; set; }

        public Statement TrueStatement { get; set; }

        public Statement FalseStatement { get; set; }
    }
}
