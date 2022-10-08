using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEAC
{
    internal class Scope
    {
        private Dictionary<string, SymbolEntry> symbols = new Dictionary<string, SymbolEntry>();
        private int parameterOffset = 8;
        private int localOffset;
        private int tempVariableIndex;

        public int LocalOffset { get { return this.localOffset; } }

        public int ParameterOffset { get { return this.ParameterOffset; } }

        public LocalVariable? ReturnVariable { get; set; }

        public ParameterVariable? LargeReturnVariable { get; set; }

        public LocalVariable DefineTempVariable(TypeDefinition type)
        {
            string name = "$temp" + (this.tempVariableIndex++).ToString();
            this.DefineLocalVariable(name, type);
            return (LocalVariable)symbols[name];
        }

        public LocalVariable DefineLocalVariable(string name, TypeDefinition type)
        {
            int alignFix = localOffset % type.Size;
            if (alignFix > 0)
            {
                localOffset += type.Size - alignFix;
            }

            localOffset += type.Size;
            LocalVariable l = new LocalVariable(name)
            {
                Offset = this.localOffset,
                Type = type
            };

            symbols.Add(name, l);
            return l;
        }

        public ParameterVariable DefineParameter(string name, TypeDefinition type)
        {
            ParameterVariable p = new ParameterVariable(name)
            {
                Offset = this.parameterOffset,
                Type = type
            };

            parameterOffset += ((p.Type.Size + 3) / 4) * 4;
            symbols.Add(name, p);

            return p;
        }

        public bool TryLookup(string symbol, out SymbolEntry? value)
        {
            return this.symbols.TryGetValue(symbol, out value);
        }

        public void SaveSymbols(IDictionary<string, int> dictionary)
        {
            foreach (string symbolName in this.symbols.Keys)
            {
                var symbol = symbols[symbolName];
                LocalVariable? localVar = symbol as LocalVariable;
                if (localVar != null)
                {
                    dictionary.Add("_" + symbolName + "$", -localVar.Offset);
                }
                else
                {
                    ParameterVariable parVar = (ParameterVariable)symbol;
                    dictionary.Add("_" + symbolName + "$", parVar.Offset);
                }
            }
        }
    }
}
