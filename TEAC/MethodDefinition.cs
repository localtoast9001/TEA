//-----------------------------------------------------------------------
// <copyright file="MethodDefinition.cs" company="Jon Rowlett">
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
    /// Method definition parse node.
    /// </summary>
    internal class MethodDefinition : ParseNode
    {
        private readonly List<ParameterDeclaration> parameters = new List<ParameterDeclaration>();

        private readonly List<Expression> baseConstructorArgs = new List<Expression>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodDefinition"/> class.
        /// </summary>
        /// <param name="start">The first token in the parse node.</param>
        /// <param name="name">The name of the method.</param>
        public MethodDefinition(Token start, string name)
            : base(start)
        {
            this.MethodNameReference = name;
        }

        /// <summary>
        /// Gets or sets the extern implementation.
        /// </summary>
        public string? ExternImpl { get; set; }

        /// <summary>
        /// Gets the method name reference.
        /// </summary>
        public string MethodNameReference { get; }

        /// <summary>
        /// Gets or sets the return type.
        /// </summary>
        public TypeReference? ReturnType { get; set; }

        /// <summary>
        /// Gets or sets the local variables.
        /// </summary>
        public VarBlock? LocalVariables { get; set; }

        /// <summary>
        /// Gets or sets the method body.
        /// </summary>
        public BlockStatement? Body { get; set; }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        public IList<ParameterDeclaration> Parameters
        {
            get { return this.parameters; }
        }

        /// <summary>
        /// Gets the base constructor arguments.
        /// </summary>
        public IList<Expression> BaseConstructorArguments
        {
            get { return this.baseConstructorArgs; }
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
