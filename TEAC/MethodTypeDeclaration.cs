using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEAC
{
    class MethodTypeDeclaration : TypeDeclaration
    {
        private List<ParameterDeclaration> parameters = new List<ParameterDeclaration>();

        public TypeReference? ReturnType { get; set; }
        public TypeReference? ImplicitArgType { get; set; }
        public IEnumerable<ParameterDeclaration> Parameters { get { return this.parameters; } } 
        public MethodTypeDeclaration(Token start, string name)
            : base(start, name)
        {
        }

        public void AddParameter(ParameterDeclaration item)
        {
            this.parameters.Add(item);
        }
    }
}
