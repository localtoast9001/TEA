//-----------------------------------------------------------------------
// <copyright file="Symbol.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler.Elf
{
    /// <summary>
    /// A symbol that references a location in a <see cref="ProgramSection" />.
    /// </summary>
    public class Symbol
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Symbol"/> class.
        /// </summary>
        /// <param name="name">Name of the symbol.</param>
        /// <param name="offset">The symbol offset.</param>
        /// <param name="type">The symbol type.</param>
        /// <param name="binding">The symbol binding.</param>
        internal Symbol(string name, long offset, SymbolType type, SymbolBinding binding)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.Offset = offset;
            this.Type = type;
            this.Binding = binding;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the offset of the symbol in the section.
        /// </summary>
        public long Offset { get; }

        /// <summary>
        /// Gets the size of the symbol in the section.
        /// </summary>
        public uint Size { get; private set; }

        /// <summary>
        /// Gets the symbol type.
        /// </summary>
        public SymbolType Type { get; }

        /// <summary>
        /// Gets the symbol binding.
        /// </summary>
        public SymbolBinding Binding { get; }

        /// <summary>
        /// Completes the symbol definition.
        /// </summary>
        /// <param name="size">The size of the symbol.</param>
        internal void Complete(uint size)
        {
            this.Size = size;
        }
    }
}