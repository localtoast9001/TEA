//-----------------------------------------------------------------------
// <copyright file="VarBlock.cs" company="Jon Rowlett">
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
    internal class VarBlock : ParseNode
    {
        public VarBlock(Token start)
            : base(start)
        {
            this.Variables = new List<VariableDeclaration>();
        }

        public IList<VariableDeclaration> Variables { get; }
    }
}
