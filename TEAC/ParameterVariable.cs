//-----------------------------------------------------------------------
// <copyright file="ParameterVariable.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace TEAC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    class ParameterVariable : SymbolEntry
    {
        public ParameterVariable(string name)
            : base(name)
        {
        }

        public int Offset { get; set; }
    }
}
