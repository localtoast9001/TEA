
namespace TEAC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class NegativeExpression : Expression
    {
        public NegativeExpression(Token start, Expression inner)
            : base(start)
        {
            this.Inner = inner;
        }

        public Expression Inner { get; private set; }
    }
}
