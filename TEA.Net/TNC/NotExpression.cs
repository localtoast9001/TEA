namespace TEAC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    class NotExpression : Expression
    {
        public NotExpression(Token start, Expression inner)
            : base(start)
        {
            this.Inner = inner;
        }

        public Expression Inner { get; private set; }
    }
}
