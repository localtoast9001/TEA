//-----------------------------------------------------------------------
// <copyright file="InheritedReferenceExpression.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TEAC
{
    class InheritedReferenceExpression : ReferenceExpression
    {
        public InheritedReferenceExpression(Token start)
            : base(start)
        {
        }

        public override bool UseVirtualDispatch
        {
            get
            {
                return false;
            }
        }
    }
}