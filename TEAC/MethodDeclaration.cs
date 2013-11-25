using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEAC
{
    internal class MethodDeclaration : ParseNode
    {
        private List<ParameterDeclaration> parameters = new List<ParameterDeclaration>();

        public MethodDeclaration(Token start, string name, bool isStatic, bool isVirtual)
            : base(start)
        {
            this.MethodName = name;
            this.IsStatic = isStatic;
            this.IsVirtual = isVirtual;
        }

        public TypeReference ReturnType { get; set; }

        public string MethodName { get; private set; }

        public bool IsStatic { get; private set; }

        public bool IsVirtual { get; private set; }

        public IEnumerable<ParameterDeclaration> Parameters { get { return this.parameters; } }

        public void AddParameter(ParameterDeclaration parameterDecl)
        {
            this.parameters.Add(parameterDecl);
        }
    }
}
