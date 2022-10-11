//-----------------------------------------------------------------------
// <copyright file="ParameterDeclaration.cs" company="Jon Rowlett">
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
    /// Parameter declaration parse node.
    /// </summary>
    internal class ParameterDeclaration : ParseNode
    {
        private readonly List<string> parameterNames = new List<string>();

        private readonly TypeReference type;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterDeclaration"/> class.
        /// </summary>
        /// <param name="start">The first token of the parse node.</param>
        /// <param name="parameterNames">The parameter names.</param>
        /// <param name="type">The parameter type.</param>
        public ParameterDeclaration(Token start, IEnumerable<string> parameterNames, TypeReference type)
            : base(start)
        {
            this.type = type;
            this.parameterNames.AddRange(parameterNames);
        }

        /// <summary>
        /// Gets the parameter type.
        /// </summary>
        public TypeReference Type
        {
            get { return this.type; }
        }

        /// <summary>
        /// Gets the names of the parameters.
        /// </summary>
        public IEnumerable<string> ParameterNames
        {
            get { return this.parameterNames; }
        }
    }
}
