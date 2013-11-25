using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEAC
{
    internal class TypeReference : ParseNode
    {
        protected TypeReference(Token start)
            : base(start)
        {
        }
    }

    internal class NamedTypeReference : TypeReference
    {
        public NamedTypeReference(Token start, string typeName)
            : base(start)
        {
            this.TypeName = typeName;
        }

        public string TypeName { get; private set; }
    }

    internal class ArrayTypeReference : TypeReference
    {
        public ArrayTypeReference(Token start, Expression elementCount, TypeReference elementType)
            : base(start)
        {
            this.ElementCount = elementCount;
            this.ElementType = elementType;
        }

        public Expression ElementCount { get; private set; }

        public TypeReference ElementType { get; private set; }
    }

    internal class PointerTypeReference : TypeReference
    {
        public PointerTypeReference(Token start, TypeReference elementType)
            : base(start)
        {
            this.ElementType = elementType;
        }

        public TypeReference ElementType { get; private set; }
    }
}
