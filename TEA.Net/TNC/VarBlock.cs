using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEAC
{
    internal class VariableDeclaration : ParseNode
    {
        private List<string> variableNames = new List<string>();

        private TypeReference type;

        public VariableDeclaration(
            Token start, 
            IEnumerable<string> variableNames, 
            TypeReference type,
            Expression initExpression)
            : base(start)
        {
            this.type = type;
            this.variableNames.AddRange(variableNames);
            this.InitExpression = initExpression;
        }

        public TypeReference Type { get { return this.type; } }
        public Expression InitExpression { get; private set; }
        public IEnumerable<string> VariableNames { get { return this.variableNames; } }
    }

    internal class VarBlock : ParseNode
    {
        public VarBlock(Token start)
            : base(start)
        {
            this.Variables = new List<VariableDeclaration>();
        }

        public IList<VariableDeclaration> Variables { get; private set; }
    }
}
