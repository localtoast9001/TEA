﻿//-----------------------------------------------------------------------
// <copyright file="SymbolEntry.cs" company="Jon Rowlett">
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

    internal abstract class SymbolEntry
    {
        protected SymbolEntry(string name)
        {
            this.Name = name;
        }

        public string Name { get; }
        public TypeDefinition? Type { get; set; }
    }
}
