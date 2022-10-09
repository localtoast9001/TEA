//-----------------------------------------------------------------------
// <copyright file="ReferenceExpression.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TEAC
{
    abstract class ReferenceExpression : Expression
    {
        protected ReferenceExpression(Token start)
            : base(start)
        {
        }

        public virtual bool UseVirtualDispatch
        {
            get
            {
                return true;
            }
        }
    }
}
