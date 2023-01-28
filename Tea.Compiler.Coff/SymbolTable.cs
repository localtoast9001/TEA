// <copyright file="SymbolTable.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>

namespace Tea.Compiler.Coff
{
    using Tea.Compiler.Binary;

    /// <summary>
    /// Symbol table inside a COFF file.
    /// </summary>
    internal class SymbolTable : ISerializable
    {
        private readonly List<SymbolEntry> symbols = new List<SymbolEntry>();
        private readonly Dictionary<string, int> indexByName = new Dictionary<string, int>(StringComparer.Ordinal);
        private readonly StringTable stringTable;
        private int lastIndex = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolTable"/> class.
        /// </summary>
        /// <param name="stringTable">The string table in which to store long strings as symbols are defined.</param>
        public SymbolTable(StringTable stringTable)
        {
            this.stringTable = stringTable;
        }

        /// <summary>
        /// Gets the number of symbols including the auxiliary entries.
        /// </summary>
        public int Count => this.lastIndex;

        /// <inheritdoc/>
        public void Serialize(BinaryWriter writer)
        {
            foreach (SymbolEntry sym in this.symbols)
            {
                sym.Serialize(writer);
            }
        }

        /// <summary>
        /// Defines a new symbol and adds it to the table.
        /// </summary>
        /// <param name="symbol">The symbol to define.</param>
        /// <param name="sectionIndex">The index of the section in the COFF file where it belongs.</param>
        /// <returns>The 0-based index of the symbol in the table.</returns>
        /// <remarks>If the symbol is already defined in the table, this method returns the index of the existing symbol.</remarks>
        public int DefineSymbol(Symbol symbol, int sectionIndex)
        {
            int index = this.FindSymbol(symbol.Name);
            if (index >= 0)
            {
                return index;
            }

            SymbolEntry entry = new SymbolEntry()
            {
                SectionNumber = (short)sectionIndex,
                Value = (int)symbol.Offset,
                StorageClass = 0,
            };

            if (sectionIndex > 0)
            {
                entry.StorageClass = symbol.Global ? (byte)2 : (byte)3;
            }

            this.stringTable.SetStringField(symbol.Name, new Span<byte>(entry.Name, 0, entry.Name.Length));
            this.symbols.Add(entry);
            this.indexByName[symbol.Name] = this.lastIndex;

            int result = this.lastIndex;
            this.lastIndex += 1 + entry.AuxCount;
            return result;
        }

        /// <summary>
        /// Finds an existing symbol by name.
        /// </summary>
        /// <param name="name">The name of the symbol to find using ordinal string compare.</param>
        /// <returns>The 0-based index of the matching symbol or a value less than zero if the symbol is not defined.</returns>
        public int FindSymbol(string name)
        {
            if (this.indexByName.TryGetValue(name, out int index))
            {
                return index;
            }

            return -1;
        }
    }
}