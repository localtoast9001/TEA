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

        public ClassDeclaration(
            Token start, 
            string name, 
            string baseType,
            bool isStatic, 
            bool isPublic)
            : base(start, name)
        {
            this.IsPublic = isPublic;
            this.IsStatic = isStatic;
            this.BaseType = baseType;
        }

        public bool IsStatic { get; private set; }

        public bool IsPublic { get; private set; }

        public string BaseType { get; private set; }

        public VarBlock Fields { get; set; }

        public IEnumerable<MethodDeclaration> PublicMethods { get { return this.publicMethods; } }
        public IEnumerable<MethodDeclaration> ProtectedMethods { get { return this.protectedMethods; } }
        public IEnumerable<MethodDeclaration> PrivateMethods { get { return this.privateMethods; } }

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
    }
}
