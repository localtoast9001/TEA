//-----------------------------------------------------------------------
// <copyright file="Scope.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TEAC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Scope for resolving identifiers.
    /// </summary>
    internal class Scope
    {
        private Dictionary<string, SymbolEntry> symbols = new Dictionary<string, SymbolEntry>();
        private int parameterOffset = 8;
        private int localOffset;
        private int tempVariableIndex;

        /// <summary>
        /// Gets the offset to local variables in the frame.
        /// </summary>
        public int LocalOffset
        {
            get { return this.localOffset; }
        }

        /// <summary>
        /// Gets the offset to parameters in the frame.
        /// </summary>
        public int ParameterOffset
        {
            get { return this.parameterOffset; }
        }

        /// <summary>
        /// Gets or sets the return variable.
        /// </summary>
        public LocalVariable? ReturnVariable { get; set; }

        /// <summary>
        /// Gets or sets the parameter for large return values.
        /// </summary>
        public ParameterVariable? LargeReturnVariable { get; set; }

        /// <summary>
        /// Defines a temporary local variable.
        /// </summary>
        /// <param name="type">The type of the temporary local variable.</param>
        /// <returns>A new instance of the <see cref="LocalVariable"/> class.</returns>
        public LocalVariable DefineTempVariable(TypeDefinition type)
        {
            string name = "$temp" + (this.tempVariableIndex++).ToString();
            this.DefineLocalVariable(name, type);
            return (LocalVariable)this.symbols[name];
        }

        /// <summary>
        /// Defines a local variable.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <param name="type">The type of the variable.</param>
        /// <returns>A new instance of the <see cref="LocalVariable"/> class.</returns>
        public LocalVariable DefineLocalVariable(string name, TypeDefinition type)
        {
            int alignFix = this.localOffset % type.Size;
            if (alignFix > 0)
            {
                this.localOffset += type.Size - alignFix;
            }

            this.localOffset += type.Size;
            LocalVariable l = new LocalVariable(name)
            {
                Offset = this.localOffset,
                Type = type,
            };

            this.symbols.Add(name, l);
            return l;
        }

        /// <summary>
        /// Defines a parameter variable.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="type">The type of the parameter.</param>
        /// <returns>A new instance of the <see cref="ParameterVariable"/> class.</returns>
        public ParameterVariable DefineParameter(string name, TypeDefinition type)
        {
            ParameterVariable p = new ParameterVariable(name)
            {
                Offset = this.parameterOffset,
                Type = type,
            };

            this.parameterOffset += ((p.Type.Size + 3) / 4) * 4;
            this.symbols.Add(name, p);

            return p;
        }

        /// <summary>
        /// Tries to lookup a symbol by name.
        /// </summary>
        /// <param name="symbol">The symbol to lookup.</param>
        /// <param name="value">On success, receives the matching variable.</param>
        /// <returns>True if there is a match; otherwise, false.</returns>
        public bool TryLookup(string symbol, out SymbolEntry? value)
        {
            return this.symbols.TryGetValue(symbol, out value);
        }

        /// <summary>
        /// Saves the symbol names and offsets to the given dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary where copies of the symbols will be saved.</param>
        public void SaveSymbols(IDictionary<string, int> dictionary)
        {
            foreach (string symbolName in this.symbols.Keys)
            {
                var symbol = this.symbols[symbolName];
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
