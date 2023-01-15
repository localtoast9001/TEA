//-----------------------------------------------------------------------
// <copyright file="SymbolTableSection.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler.Elf
{
    using Tea.Compiler.Binary;

    /// <summary>
    /// Section for symbol entries.
    /// </summary>
    internal class SymbolTableSection : Section
    {
        private readonly IList<Elf32SymbolEntry> localSymbols = new List<Elf32SymbolEntry>();

        private readonly IList<Elf32SymbolEntry> globalSymbols = new List<Elf32SymbolEntry>();

        /// <inheritdoc/>
        internal override SectionType Type => SectionType.SymTab;

        /// <inheritdoc/>
        internal override SectionFlags Flags => SectionFlags.None;

        /// <inheritdoc/>
        internal override uint Size => (uint)(this.localSymbols.Count + this.globalSymbols.Count + 1) * Elf32SymbolEntry.BinarySize;

        /// <inheritdoc/>
        internal override uint EntrySize => Elf32SymbolEntry.BinarySize;

        /// <inheritdoc/>
        internal override uint Info
        {
            get
            {
                // The index of the 1st global symbol.
                return (uint)(this.localSymbols.Count + 1);
            }
        }

        /// <summary>
        /// Finds a symbol in the table by name.
        /// </summary>
        /// <param name="nameOffset">The name offset in the string table.</param>
        /// <returns>The index of the symbol in the table.</returns>
        public uint FindSymbol(uint nameOffset)
        {
            uint localSym = FindSymbol(this.localSymbols, nameOffset);
            if (localSym > 0)
            {
                return localSym;
            }

            uint globalSym = FindSymbol(this.globalSymbols, nameOffset);
            return globalSym > 0 ? globalSym + (uint)this.localSymbols.Count : 0;
        }

        /// <summary>
        /// Adds a new symbol.
        /// </summary>
        /// <param name="symbol">The symbol to add.</param>
        public void AddSymbol(Elf32SymbolEntry symbol)
        {
            if (symbol.Binding == SymbolBinding.Global)
            {
                this.globalSymbols.Add(symbol);
            }
            else
            {
                this.localSymbols.Add(symbol);
            }
        }

        /// <inheritdoc/>
        internal override void InternalSerialize(BinaryWriter writer)
        {
            writer.Skip(Elf32SymbolEntry.BinarySize);
            SerializeTable(writer, this.localSymbols);
            SerializeTable(writer, this.globalSymbols);
        }

        private static void SerializeTable(BinaryWriter writer, IList<Elf32SymbolEntry> symbols)
        {
            foreach (Elf32SymbolEntry sym in symbols)
            {
                sym.Serialize(writer);
            }
        }

        private static uint FindSymbol(IList<Elf32SymbolEntry> symbols, uint nameOffset)
        {
            for (uint i = 0; i < symbols.Count; i++)
            {
                if (symbols[(int)i].Name == nameOffset)
                {
                    return i + 1;
                }
            }

            return 0;
        }
    }
}