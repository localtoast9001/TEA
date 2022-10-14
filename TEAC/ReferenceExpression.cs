//-----------------------------------------------------------------------
// <copyright file="ReferenceExpression.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TEAC
{
    /// <summary>
    /// Parse node for a reference.
    /// </summary>
    internal abstract class ReferenceExpression : Expression
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceExpression"/> class.
        /// </summary>
        /// <param name="start">The first token in the parse node.</param>
        protected ReferenceExpression(Token start)
            : base(start)
        {
        }

        /// <summary>
        /// Gets a value indicating whether or not to use virtual dispatch to traverse the reference.
        /// </summary>
        public virtual bool UseVirtualDispatch
        {
            get
            {
                return true;
            }
        }
    }
}
