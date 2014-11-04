namespace TEAC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    class LocalVariable : SymbolEntry
    {
        public LocalVariable(string name)
            : base(name)
        {
        }

        public int Offset { get; set; }
    }
}
