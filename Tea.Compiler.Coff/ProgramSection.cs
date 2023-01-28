// <copyright file="ProgramSection.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>

namespace Tea.Compiler.Coff
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Tea.Compiler.Binary;

    /// <summary>
    /// Code or data sections.
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
        public override IReadOnlyCollection<Symbol> Symbols => new ReadOnlyCollection<Symbol>(this.symbols);

        /// <summary>
        /// Gets the collection of relocations.
        /// </summary>
        public override IReadOnlyCollection<Relocation> Relocations => new ReadOnlyCollection<Relocation>(this.relocations);

        /// <inheritdoc/>
        internal override uint Length => (uint)this.content.Length;

        /// <inheritdoc/>
        internal override ImageSectionCharacteristicFlags Characteristics =>
            base.Characteristics |
            ImageSectionCharacteristicFlags.MemRead |
            (this.Executable ? (ImageSectionCharacteristicFlags.Code | ImageSectionCharacteristicFlags.MemExecute) : ImageSectionCharacteristicFlags.InitializedData) |
            (this.Writeable ? ImageSectionCharacteristicFlags.MemWrite : ImageSectionCharacteristicFlags.None);

        /// <inheritdoc/>
        internal override int Align => 16;

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

        /// <summary>
        /// Defines a symbol that points to the current location in the section.
        /// </summary>
        /// <param name="name">The symbol name.</param>
        /// <param name="global">True if the symbol is global; otherwise, false.</param>
        /// <returns>A new instance of the <see cref="Symbol"/> class.</returns>
        public Symbol DefineSymbol(string name, bool global)
        {
            Symbol symbol = new Symbol(name, this.content.Position, global);
            this.symbols.Add(symbol);
            return symbol;
        }

        /// <inheritdoc/>
        internal override void InternalSerialize(BinaryWriter writer)
        {
            writer.WriteBytes(this.content.ToArray());
        }
    }
}
