//-----------------------------------------------------------------------
// <copyright file="NewExpression.cs" company="Jon Rowlett">
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
    /// New expression.
    /// </summary>
    internal class NewExpression : Expression
    {
        private readonly List<Expression> constructorArguments = new List<Expression>();
        private readonly TypeReference typeReference;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewExpression"/> class.
        /// </summary>
        /// <param name="start">The first token in the parse node.</param>
        /// <param name="typeReference">The type of the object to create.</param>
        /// <param name="constructorArguments">The constructor arguments.</param>
        public NewExpression(Token start, TypeReference typeReference, IEnumerable<Expression> constructorArguments)
            : base(start)
        {
            this.typeReference = typeReference;
            this.constructorArguments.AddRange(constructorArguments);
        }

        /// <summary>
        /// Gets the constructor arguments.
        /// </summary>
        public IEnumerable<Expression> ConstructorArguments
        {
            get
            {
                return this.constructorArguments;
            }
        }

        /// <summary>
        /// Gets the type of the object to create.
        /// </summary>
        public TypeReference Type
        {
            get
            {
                return this.typeReference;
            }
        }
    }
}
