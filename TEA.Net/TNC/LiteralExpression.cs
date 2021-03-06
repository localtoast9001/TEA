﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEAC
{
    class LiteralExpression : Expression
    {
        public LiteralExpression(Token start, object value)
            :base(start)
        {
            this.Value = value;
        }

        public object Value { get; private set; }
    }
}
