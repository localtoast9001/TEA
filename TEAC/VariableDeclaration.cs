//-----------------------------------------------------------------------
// <copyright file="VariableDeclaration.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TEAC
{
    /// <summary>
    /// A parse node for a variable declaration.
    /// </summary>
   internal class VariableDeclaration : ParseNode
    {
        private readonly List<string> variableNames = new List<string>();

        private readonly TypeReference type;

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

        public TypeReference Type { get { return this.type; } }
        public Expression? InitExpression { get; }
        public IEnumerable<string> VariableNames { get { return this.variableNames; } }
    }
}
