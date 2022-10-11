//-----------------------------------------------------------------------
// <copyright file="NamedReferenceExpression.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TEAC
{
    /// <summary>
    /// A named reference expression.
    /// </summary>
    internal class NamedReferenceExpression : ReferenceExpression
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NamedReferenceExpression"/> class.
        /// </summary>
        /// <param name="start">The first token in the parse node.</param>
        /// <param name="identifier">The named identifier.</param>
        public NamedReferenceExpression(Token start, string identifier)
            : base(start)
        {
            this.Identifier = identifier;
        }

        /// <summary>
        /// Gets the named identifier.
        /// </summary>
        public string Identifier { get; }
    }
}