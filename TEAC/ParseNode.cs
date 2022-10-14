//-----------------------------------------------------------------------
// <copyright file="ParseNode.cs" company="Jon Rowlett">
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
    /// Base parse node.
    /// </summary>
    internal abstract class ParseNode
    {
        private readonly Token start;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseNode"/> class.
        /// </summary>
        /// <param name="start">The first token in the parse node.</param>
        protected ParseNode(Token start)
        {
            this.start = start;
        }

        /// <summary>
        /// Gets the first token in the parse node.
        /// </summary>
        public Token Start
        {
            get { return this.start; }
        }
    }
}
