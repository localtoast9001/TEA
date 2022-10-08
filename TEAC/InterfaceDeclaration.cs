using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEAC
{
    internal class InterfaceDeclaration : TypeDeclaration
    {
        private List<MethodDeclaration> methods = new List<MethodDeclaration>();

        public InterfaceDeclaration(
            Token start,
            string name)
            : base(start, name)
        {
        }

        public string? BaseInterfaceType { get; set; }
        public IList<MethodDeclaration> Methods { get { return this.methods; } }
    }
}
