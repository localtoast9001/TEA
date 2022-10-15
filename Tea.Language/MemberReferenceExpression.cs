//-----------------------------------------------------------------------
// <copyright file="MemberReferenceExpression.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Language
{
    /// <summary>
    /// Member reference expression.
    /// </summary>
    public class MemberReferenceExpression : ReferenceExpression
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberReferenceExpression"/> class.
        /// </summary>
        /// <param name="start">The first token in the parse node.</param>
        /// <param name="inner">The inner expression.</param>
        /// <param name="memberName">The name of the member.</param>
        public MemberReferenceExpression(Token start, ReferenceExpression inner, string memberName)
            : base(start)
        {
            this.MemberName = memberName;
            this.Inner = inner;
        }

        /// <summary>
        /// Gets the member name.
        /// </summary>
        public string MemberName { get; }

        /// <summary>
        /// Gets the inner reference expression.
        /// </summary>
        public ReferenceExpression Inner { get; }

        /// <inheritdoc/>
        public override bool UseVirtualDispatch
        {
            get
            {
                return this.Inner.UseVirtualDispatch;
            }
        }
    }
}