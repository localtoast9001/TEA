using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEAC
{
    internal class MethodDefinition : ParseNode
    {
        private List<ParameterDeclaration> parameters = new List<ParameterDeclaration>();

        public MethodDefinition(Token start, string name)
            : base(start)
        {
            this.MethodNameReference = name;
        }

        public string ExternImpl { get; set; }

        public string MethodNameReference { get; private set; }

        public TypeReference ReturnType { get; set; }

        public VarBlock LocalVariables { get; set; }

        public BlockStatement Body { get; set; }

        public IList<ParameterDeclaration> Parameters { get { return this.parameters; } }

        public void AddParameter(ParameterDeclaration parameterDecl)
        {
            this.parameters.Add(parameterDecl);
        }
    }
}
