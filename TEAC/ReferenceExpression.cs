using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEAC
{
    abstract class ReferenceExpression : Expression
    {
        protected ReferenceExpression(Token start)
            : base(start)
        {
        }

        public virtual bool UseVirtualDispatch
        {
            get
            {
                return true;
            }
        }
    }

    class NamedReferenceExpression : ReferenceExpression
    {
        public NamedReferenceExpression(Token start, string identifier)
            : base(start)
        {
            this.Identifier = identifier;
        }

        public string Identifier { get; }
    }

    class InheritedReferenceExpression : ReferenceExpression
    {
        public InheritedReferenceExpression(Token start)
            : base(start)
        {
        }

        public override bool UseVirtualDispatch
        {
            get
            {
                return false;
            }
        }
    }

    class MemberReferenceExpression : ReferenceExpression
    {
        public MemberReferenceExpression(Token start, ReferenceExpression inner, string memberName)
            : base(start)
        {
            this.MemberName = memberName;
            this.Inner = inner;
        }

        public string MemberName { get; }

        public ReferenceExpression Inner { get; }

        public override bool UseVirtualDispatch
        {
            get
            {
                return this.Inner.UseVirtualDispatch;
            }
        }
    }

    class CallReferenceExpression : ReferenceExpression
    {
        private List<Expression> arguments = new List<Expression>();

        public CallReferenceExpression(Token start, ReferenceExpression inner)
            : base(start)
        {
            this.Inner = inner;
        }

        public ReferenceExpression Inner { get; }
        public IEnumerable<Expression> Arguments { get { return this.arguments; } }

        public void AddArgument(Expression arg)
        {
            this.arguments.Add(arg);
        }
    }

    class ArrayIndexReferenceExpression : ReferenceExpression
    {
        public ArrayIndexReferenceExpression(Token start, ReferenceExpression inner, Expression index)
            : base(start)
        {
            this.Inner = inner;
            this.Index = index;
        }

        public ReferenceExpression Inner { get; }
        public Expression Index { get; }
    }

    class DereferenceExpression : ReferenceExpression
    {
        public DereferenceExpression(Token start, ReferenceExpression inner)
            : base(start)
        {
            this.Inner = inner;
        }

        public ReferenceExpression Inner { get; }
    }
}
