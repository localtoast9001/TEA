using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEAC
{
    internal abstract class TypeDeclaration : ParseNode
    {
        protected TypeDeclaration(Token start, string name)
            : base(start)
        {
            this.Name = name;
        }

        public string Name { get; private set; }
    }
}
