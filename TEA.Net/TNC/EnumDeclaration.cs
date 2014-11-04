using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEAC
{
    internal class EnumDeclaration : TypeDeclaration
    {
        private List<string> values = new List<string>();

        public EnumDeclaration(Token start, string name)
            : base(start, name)
        {
        }

        public IEnumerable<string> Values 
        { 
            get 
            { 
                return this.values; 
            } 
        }

        public void AddValue(string name)
        {
            this.values.Add(name);
        }
    }
}
