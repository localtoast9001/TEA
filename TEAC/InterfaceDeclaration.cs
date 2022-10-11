//-----------------------------------------------------------------------
// <copyright file="InterfaceDeclaration.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TEAC
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Interface declaration parse node.
    /// </summary>
    internal class InterfaceDeclaration : TypeDeclaration
    {
        private readonly List<MethodDeclaration> methods = new List<MethodDeclaration>();

        /// <summary>
        /// Initializes a new instance of the <see cref="InterfaceDeclaration"/> class.
        /// </summary>
        /// <param name="start">The first token in the parse node.</param>
        /// <param name="name">The name of the interface.</param>
        public InterfaceDeclaration(
            Token start,
            string name)
            : base(start, name)
        {
        }

        /// <summary>
        /// Gets or sets the base interface type.
        /// </summary>
        public string? BaseInterfaceType { get; set; }

        /// <summary>
        /// Gets the list of methods.
        /// </summary>
        public IList<MethodDeclaration> Methods
        {
            get { return this.methods; }
        }
    }
}
