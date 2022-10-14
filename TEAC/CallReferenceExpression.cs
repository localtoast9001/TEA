//-----------------------------------------------------------------------
// <copyright file="CallReferenceExpression.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace TEAC
{
    /// <summary>
    /// Parse node for a call expression to a function or procedure.
    /// </summary>
    internal class CallReferenceExpression : ReferenceExpression
    {
        private readonly List<Expression> arguments = new List<Expression>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CallReferenceExpression"/> class.
        /// </summary>
        /// <param name="start">The first token in the parse node.</param>
        /// <param name="inner">The inner reference expression to the method.</param>
        public CallReferenceExpression(Token start, ReferenceExpression inner)
            : base(start)
        {
            this.Inner = inner ?? throw new ArgumentNullException(nameof(inner));
        }

        /// <summary>
        /// Gets the inner expression.
        /// </summary>
        public ReferenceExpression Inner { get; }

        /// <summary>
        /// Gets the arguments.
        /// </summary>
        public IEnumerable<Expression> Arguments
        {
            get { return this.arguments; }
        }

        /// <summary>
        /// Adds an argument.
        /// </summary>
        /// <param name="arg">The argument to add.</param>
        public void AddArgument(Expression arg)
        {
            this.arguments.Add(arg);
        }
    }
}