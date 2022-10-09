//-----------------------------------------------------------------------
// <copyright file="ParseNode.cs" company="Jon Rowlett">
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
    internal abstract class ParseNode
    {
        private Token start;

        protected ParseNode(Token start)
        {
            this.start = start;
        }

        public Token Start { get { return this.start; } }
    }
}
