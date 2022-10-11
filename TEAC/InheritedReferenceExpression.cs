//-----------------------------------------------------------------------
// <copyright file="InheritedReferenceExpression.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TEAC
{
    /// <summary>
    /// Reference to an inherited member.
    /// </summary>
    internal class InheritedReferenceExpression : ReferenceExpression
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