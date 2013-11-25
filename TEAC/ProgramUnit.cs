using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEAC
{
    internal class ProgramUnit : ParseNode
    {
        private Dictionary<string, TypeDeclaration> types = new Dictionary<string, TypeDeclaration>();
        private List<TypeDeclaration> typeList = new List<TypeDeclaration>();
        private List<MethodDefinition> methods = new List<MethodDefinition>();
        private string nameSpace = string.Empty;
        private List<string> usesReferences = new List<string>();

        public ProgramUnit(Token start)
            : base(start)
        {
        }

        public string Namespace
        {
            get
            {
                return this.nameSpace;
            }

            set
            {
                this.nameSpace = value;
            }
        }

        public IEnumerable<string> Uses
        {
            get { return this.usesReferences; }
        }

        public IEnumerable<TypeDeclaration> Types
        {
            get { return this.typeList; }
        }

        public VarBlock Variables { get; set; }

        public IEnumerable<MethodDefinition> Methods { get { return this.methods; } }

        public void AddUses(string value)
        {
            this.usesReferences.Add(value);
        }

        public void AddType(TypeDeclaration type)
        {
            types.Add(type.Name, type);
            typeList.Add(type);
        }

        public void AddMethod(MethodDefinition method)
        {
            methods.Add(method);
        }
    }
}
