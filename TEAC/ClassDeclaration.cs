using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEAC
{
    internal class ClassDeclaration : TypeDeclaration
    {
        private List<MethodDeclaration> publicMethods = new List<MethodDeclaration>();
        private List<MethodDeclaration> protectedMethods = new List<MethodDeclaration>();
        private List<MethodDeclaration> privateMethods = new List<MethodDeclaration>();
        private List<string> interfaces = new List<string>();

        public ClassDeclaration(
            Token start, 
            string name, 
            string? baseType,
            bool isStatic)
            : base(start, name)
        {
            this.IsStatic = isStatic;
            this.BaseType = baseType;
        }

        public bool IsStatic { get; }

        public string? BaseType { get; }

        public VarBlock? Fields { get; set; }

        public IEnumerable<MethodDeclaration> PublicMethods { get { return this.publicMethods; } }
        public IEnumerable<MethodDeclaration> ProtectedMethods { get { return this.protectedMethods; } }
        public IEnumerable<MethodDeclaration> PrivateMethods { get { return this.privateMethods; } }
        public IEnumerable<string> Interfaces { get { return this.interfaces; } }

        public void AddPublicMethod(MethodDeclaration method)
        {
            this.publicMethods.Add(method);
        }

        public void AddProtectedMethod(MethodDeclaration method)
        {
            this.protectedMethods.Add(method);
        }

        public void AddPrivateMethod(MethodDeclaration method)
        {
            this.privateMethods.Add(method);
        }

        public void AddInterface(string interfaceName)
        {
            this.interfaces.Add(interfaceName);
        }
    }
}
