//-----------------------------------------------------------------------
// <copyright file="Expression.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

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
