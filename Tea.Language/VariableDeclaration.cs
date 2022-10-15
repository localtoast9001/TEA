//-----------------------------------------------------------------------
// <copyright file="VariableDeclaration.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Language
{
    /// <summary>
    /// A parse node for a variable declaration.
    /// </summary>
    public class VariableDeclaration : ParseNode
    {
        private readonly List<string> variableNames = new List<string>();

        private readonly TypeReference type;

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableDeclaration"/> class.
        /// </summary>
        /// <param name="start">The first token in the parse node.</param>
        /// <param name="variableNames">The list of variable names.</param>
        /// <param name="type">The type of the variable.</param>
        /// <param name="initExpression">The init expression.</param>
        public VariableDeclaration(
            Token start,
            IEnumerable<string> variableNames,
            TypeReference type,
            Expression? initExpression)
            : base(start)
        {
            this.type = type;
            this.variableNames.AddRange(variableNames);
            this.InitExpression = initExpression;
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public TypeReference Type
        {
            get { return this.type; }
        }

        /// <summary>
        /// Gets the init expression.
        /// </summary>
        public Expression? InitExpression { get; }

        /// <summary>
        /// Gets the variable names.
        /// </summary>
        public IEnumerable<string> VariableNames
        {
            get { return this.variableNames; }
        }
    }
}
