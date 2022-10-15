//-----------------------------------------------------------------------
// <copyright file="MethodDeclaration.cs" company="Jon Rowlett">
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
    /// Method declaration parse node.
    /// </summary>
    public class MethodDeclaration : ParseNode
    {
        private readonly List<ParameterDeclaration> parameters = new List<ParameterDeclaration>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodDeclaration"/> class.
        /// </summary>
        /// <param name="start">The first token in the parse node.</param>
        /// <param name="name">The name of the method.</param>
        /// <param name="isStatic">True if the method is static; otherwise, false.</param>
        /// <param name="isVirtual">True if the method is virtual; otherwise, false.</param>
        /// <param name="isAbstract">True if the method is abstract; otherwise, false.</param>
        public MethodDeclaration(
            Token start,
            string name,
            bool isStatic,
            bool isVirtual,
            bool isAbstract)
            : base(start)
        {
            this.MethodName = name;
            this.IsStatic = isStatic;
            this.IsVirtual = isVirtual;
            this.IsAbstract = isAbstract;
        }

        /// <summary>
        /// Gets or sets the return type.
        /// </summary>
        public TypeReference? ReturnType { get; set; }

        /// <summary>
        /// Gets the method name.
        /// </summary>
        public string MethodName { get; }

        /// <summary>
        /// Gets a value indicating whether the method is static.
        /// </summary>
        public bool IsStatic { get; }

        /// <summary>
        /// Gets a value indicating whether the method is virtual.
        /// </summary>
        public bool IsVirtual { get; }

        /// <summary>
        /// Gets a value indicating whether the method is abstract.
        /// </summary>
        public bool IsAbstract { get; }

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
        /// <param name="parameterDecl">The parameter to add.</param>
        public void AddParameter(ParameterDeclaration parameterDecl)
        {
            this.parameters.Add(parameterDecl);
        }
    }
}
