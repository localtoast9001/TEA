//-----------------------------------------------------------------------
// <copyright file="MethodImpl.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TEAC
{
    internal class MethodImpl
    {
        private List<AsmStatement> statements = new List<AsmStatement>();
        private Dictionary<string, int> symbols = new Dictionary<string, int>();

        public MethodImpl(Module module)
        {
            this.Module = module;
        }

        public List<AsmStatement> Statements { get { return this.statements; } }
        public IDictionary<string, int> Symbols { get { return this.symbols; } }
        public MethodInfo? Method { get; set; }
        public Module? Module { get; private set; }
    }
}