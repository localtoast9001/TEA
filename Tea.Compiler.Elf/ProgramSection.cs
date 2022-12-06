//-----------------------------------------------------------------------
// <copyright file="ProgramSection.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler.Elf
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Program (code or data) section.
    /// </summary>
    public class ProgramSection : Section
    {
        private readonly List<Symbol> symbols = new List<Symbol>();

        private readonly List<Relocation> relocations = new List<Relocation>();

        private readonly MemoryStream content = new MemoryStream();

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgramSection"/> class.
        /// </summary>
        public ProgramSection()
        : base()
        {
            this.ContentWriter = new StreamBinaryWriter(this.content);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this section is loaded into writeable memory.
        /// </summary>
        public bool Writeable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this section is executable.
        /// </summary>
        public bool Executable { get; set; }

        /// <summary>
        /// Gets the content writer.
        /// </summary>
        public IBinaryWriter ContentWriter { get; }

        /// <summary>
        /// Gets the collection of symbols.
        /// </summary>
        public IReadOnlyCollection<Symbol> Symbols => new ReadOnlyCollection<Symbol>(this.symbols);

        /// <summary>
        /// Gets the collection of relocations.
        /// </summary>
        public IReadOnlyCollection<Relocation> Relocations => new ReadOnlyCollection<Relocation>(this.relocations);

        /// <inheritdoc/>
        internal override uint Align => 16;

        /// <inheritdoc/>
        internal override uint Size => (uint)this.content.Length;

        /// <inheritdoc/>
        internal override SectionFlags Flags =>
            SectionFlags.Alloc |
            (this.Executable ? SectionFlags.ExecInstr : SectionFlags.None) |
            (this.Writeable ? SectionFlags.Write : SectionFlags.None);

        /// <inheritdoc/>
        internal override SectionType Type => SectionType.ProgBits;

        /// <summary>
        /// Starts a new symbol definition.
        /// </summary>
        /// <param name="name">The symbol name.</param>
        /// <param name="type">The symbol type.</param>
        /// <param name="binding">The symbol binding.</param>
        /// <returns>A new instance of the <see cref="Symbol"/> class.</returns>
        public Symbol StartSymbol(
            string name,
            SymbolType type,
            SymbolBinding binding)
        {
            Symbol sym = new Symbol(
                name,
                this.content.Position,
                type,
                binding);
            this.symbols.Add(sym);
            return sym;
        }

        /// <summary>
        /// Ends a symbol definition.
        /// </summary>
        /// <param name="symbol">The symbol to end.</param>
        public void EndSymbol(Symbol symbol)
        {
            symbol.Complete((uint)(this.content.Position - symbol.Offset));
        }

        /// <summary>
        /// Defines a new relocation at the current position in the content stream.
        /// </summary>
        /// <param name="symbol">The symbol reference.</param>
        /// <param name="relative">True if PC relative; otherwise, false.</param>
        /// <param name="offset">The offset from the current position where the relocation starts.</param>
        /// <returns>A new instance of the <see cref="Relocation"/> class.</returns>
        public Relocation DefineRelocation(string symbol, bool relative, uint offset = 0)
        {
            Relocation rel = new Relocation((uint)this.content.Position + offset, relative, symbol);
            this.relocations.Add(rel);
            return rel;
        }

        /// <inheritdoc/>
        internal override void InternalSerialize(BinaryWriter writer)
        {
            writer.WriteBytes(this.content.ToArray());
        }
    }
}