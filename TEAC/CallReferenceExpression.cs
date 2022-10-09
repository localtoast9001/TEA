//-----------------------------------------------------------------------
// <copyright file="CallReferenceExpression.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace TEAC
{
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
}