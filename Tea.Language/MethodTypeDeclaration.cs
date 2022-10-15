//-----------------------------------------------------------------------
// <copyright file="MethodTypeDeclaration.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Method type declaration.
    /// </summary>
    public class MethodTypeDeclaration : TypeDeclaration
    {
        private readonly List<ParameterDeclaration> parameters = new List<ParameterDeclaration>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodTypeDeclaration"/> class.
        /// </summary>
        /// <param name="start">The first token in the parse node.</param>
        /// <param name="name">The method name.</param>
        public MethodTypeDeclaration(Token start, string name)
            : base(start, name)
        {
        }

        /// <summary>
        /// Gets or sets the return type.
        /// </summary>
        public TypeReference? ReturnType { get; set; }

        /// <summary>
        /// Gets or sets the implicit argument type.
        /// </summary>
        public TypeReference? ImplicitArgType { get; set; }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        public IEnumerable<ParameterDeclaration> Parameters
        {
            get { return this.parameters; }
        }

        /// <summary>
        /// Adds a parameter.
        /// </summary>
        /// <param name="item">The parameter to add.</param>
        public void AddParameter(ParameterDeclaration item)
        {
            this.parameters.Add(item);
        }
    }
}
