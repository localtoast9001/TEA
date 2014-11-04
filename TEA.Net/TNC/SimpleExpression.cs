using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEAC
{
    class SimpleExpression : Expression
    {
        public SimpleExpression(
            Token start,
            Expression left,
            Keyword oper,
            Expression right)
            : base(start)
        {
            this.Left = left;
            this.Operator = oper;
            this.Right = right;
        }

        public Expression Left { get; private set; }

        public Keyword Operator { get; private set; }

        public Expression Right { get; private set; }

    }
}
