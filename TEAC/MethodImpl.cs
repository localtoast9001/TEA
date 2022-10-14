//-----------------------------------------------------------------------
// <copyright file="MethodImpl.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TEAC
{
    /// <summary>
    /// Method implementation created during code generation.
    /// </summary>
    internal class MethodImpl
    {
        private readonly List<AsmStatement> statements = new List<AsmStatement>();
        private readonly Dictionary<string, int> symbols = new Dictionary<string, int>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodImpl"/> class.
        /// </summary>
        /// <param name="module">The module that contains the method impl.</param>
        public MethodImpl(Module module)
        {
            this.Module = module;
        }

        /// <summary>
        /// Gets the statements.
        /// </summary>
        public List<AsmStatement> Statements
        {
            get { return this.statements; }
        }

        /// <summary>
        /// Gets the symbols.
        /// </summary>
        public IDictionary<string, int> Symbols
        {
            get { return this.symbols; }
        }

        /// <summary>
        /// Gets or sets the method info.
        /// </summary>
        public MethodInfo? Method { get; set; }

        /// <summary>
        /// Gets the containing module.
        /// </summary>
        public Module? Module { get; }
    }
}