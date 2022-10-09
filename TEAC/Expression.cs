//-----------------------------------------------------------------------
// <copyright file="Expression.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
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
