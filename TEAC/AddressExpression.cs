//-----------------------------------------------------------------------
// <copyright file="AddressExpression.cs" company="Jon Rowlett">
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
    /// Expression to reference the address of something.
    /// </summary>
    internal class AddressExpression : Expression
    {
        public AddressExpression(Token start, ReferenceExpression inner)
            : base(start)
        {
            this.Inner = inner;
        }

        public ReferenceExpression Inner { get; }
    }
}
