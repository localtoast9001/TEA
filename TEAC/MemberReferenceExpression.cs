//-----------------------------------------------------------------------
// <copyright file="MemberReferenceExpression.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TEAC
{
    class MemberReferenceExpression : ReferenceExpression
    {
        public MemberReferenceExpression(Token start, ReferenceExpression inner, string memberName)
            : base(start)
        {
            this.MemberName = memberName;
            this.Inner = inner;
        }

        public string MemberName { get; }

        public ReferenceExpression Inner { get; }

        public override bool UseVirtualDispatch
        {
            get
            {
                return this.Inner.UseVirtualDispatch;
            }
        }
    }
}