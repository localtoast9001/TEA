//-----------------------------------------------------------------------
// <copyright file="Statement.cs" company="Jon Rowlett">
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
    internal abstract class Statement : ParseNode
    {
        protected Statement(Token start)
            : base(start)
        {
        }
    }
}
