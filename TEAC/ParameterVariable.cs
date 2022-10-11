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

    /// <summary>
    /// <see cref="SymbolEntry"/> for a parameter.
    /// </summary>
    internal class ParameterVariable : SymbolEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterVariable"/> class.
        /// </summary>
        /// <param name="name">Name of the parameter.</param>
        public ParameterVariable(string name)
            : base(name)
        {
        }

        /// <summary>
        /// Gets or sets the offset of the parameter on the stack.
        /// </summary>
        public int Offset { get; set; }
    }
}
