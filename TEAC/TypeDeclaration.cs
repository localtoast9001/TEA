//-----------------------------------------------------------------------
// <copyright file="TypeDeclaration.cs" company="Jon Rowlett">
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
    /// Abstract parse node for a type declaration.
    /// </summary>
    internal abstract class TypeDeclaration : ParseNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeDeclaration"/> class.
        /// </summary>
        /// <param name="start">The first token in the parse node.</param>
        /// <param name="name">The name of the type.</param>
        protected TypeDeclaration(Token start, string name)
            : base(start)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets the name of the type.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the type is public.
        /// </summary>
        public bool IsPublic { get; set; }
    }
}
