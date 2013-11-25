using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEAC
{
    internal abstract class Expression : ParseNode
    {
        protected Expression(Token start)
            : base(start)
        {
        }
    }
}
