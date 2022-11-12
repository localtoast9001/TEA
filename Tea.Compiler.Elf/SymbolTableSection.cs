//-----------------------------------------------------------------------
// <copyright file="SymbolTableSection.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler.Elf
{
    /// <summary>
    /// Section for symbol entries.
    /// </summary>
    internal class SymbolTableSection : Section
    {
        /// <summary>
        /// Gets the list of symbols.
        /// </summary>
        public IList<Elf32SymbolEntry> Symbols { get; } = new List<Elf32SymbolEntry>();

        /// <inheritdoc/>
        internal override SectionType Type => SectionType.SymTab;

        /// <inheritdoc/>
        internal override SectionFlags Flags => SectionFlags.None;

        /// <inheritdoc/>
        internal override uint Size => (uint)(this.Symbols.Count + 1) * Elf32SymbolEntry.BinarySize;

        /// <inheritdoc/>
        internal override uint EntrySize => Elf32SymbolEntry.BinarySize;

        /// <inheritdoc/>
        internal override uint Info
        {
            get
            {
                uint result = 0;
                for (int i = 0; i < this.Symbols.Count; i++)
                {
                    Elf32SymbolEntry entry = this.Symbols[i];
                    if (entry.Binding == SymbolBinding.Local)
                    {
                        result = (uint)(i + 2);
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Finds a symbol in the table by name.
        /// </summary>
        /// <param name="nameOffset">The name offset in the string table.</param>
        /// <returns>The index of the symbol in the table.</returns>
        public uint FindSymbol(uint nameOffset)
        {
            for (uint i = 1; i < this.Symbols.Count; i++)
            {
                if (this.Symbols[(int)i].Name == nameOffset)
                {
                    return i;
                }
            }

            return 0;
        }

        /// <inheritdoc/>
        internal override void InternalSerialize(BinaryWriter writer)
        {
            writer.Skip(Elf32SymbolEntry.BinarySize);
            foreach (Elf32SymbolEntry sym in this.Symbols)
            {
                sym.Serialize(writer);
            }
        }
    }
}