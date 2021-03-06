﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEAC
{
    internal class AddressExpression : Expression
    {
        public AddressExpression(Token start, ReferenceExpression inner)
            : base(start)
        {
            this.Inner = inner;
        }

        public ReferenceExpression Inner { get; private set; }
    }
}
