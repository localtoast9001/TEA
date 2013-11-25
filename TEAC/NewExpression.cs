using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEAC
{
    internal class NewExpression : Expression
    {
        List<Expression> constructorArguments = new List<Expression>();
        TypeReference typeReference;

        public NewExpression(Token start, TypeReference typeReference, IEnumerable<Expression> constructorArguments)
            : base(start)
        {
            this.typeReference = typeReference;
            this.constructorArguments.AddRange(constructorArguments);
        }

        public IEnumerable<Expression> ConstructorArguments
        {
            get
            {
                return this.constructorArguments;
            }
        }

        public TypeReference Type
        {
            get
            {
                return this.typeReference;
            }
        }
    }
}
