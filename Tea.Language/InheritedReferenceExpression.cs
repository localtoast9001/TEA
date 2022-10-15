//-----------------------------------------------------------------------
// <copyright file="InheritedReferenceExpression.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Language
{
    /// <summary>
    /// Reference to an inherited member.
    /// </summary>
    public class InheritedReferenceExpression : ReferenceExpression
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InheritedReferenceExpression"/> class.
        /// </summary>
        /// <param name="start">The first token in the parse node.</param>
        public InheritedReferenceExpression(Token start)
            : base(start)
        {
        }

        /// <inheritdoc/>
        public override bool UseVirtualDispatch
        {
            get
            {
                return false;
            }
        }
    }
}