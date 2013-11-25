using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEAC
{
    internal class ParameterDeclaration : ParseNode
    {
        private List<string> parameterNames = new List<string>();

        private TypeReference type;

        public ParameterDeclaration(Token start, IEnumerable<string> parameterNames, TypeReference type)
            : base(start)
        {
            this.type = type;
            this.parameterNames.AddRange(parameterNames);
        }

        public TypeReference Type { get { return this.type; } }

        public IEnumerable<string> ParameterNames { get { return this.parameterNames; } }
    }
}
