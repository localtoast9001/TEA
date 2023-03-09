// <copyright file="Symbol.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>

namespace Tea.Compiler.Coff
{
    /// <summary>
    /// A symbol that references a location in a <see cref="ProgramSection" />.
    /// </summary>
    public class Symbol
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Symbol" /> class.
        /// </summary>
        /// <param name="name">The symbol name.</param>
        /// <param name="offset">The offset of the symbol in the section.</param>
        /// <param name="storage">The storage class of the symbol.</param>
        /// <param name="type">The symbol type.</param>
        internal Symbol(
            string name,
            long offset,
            StorageClass storage,
            SymbolType type)
        {
            this.Name = name;
            this.Offset = offset;
            this.Storage = storage;
            this.Type = type;
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
        /// Gets the storage class for the symbol.
        /// </summary>
        public StorageClass Storage { get; }

        /// <summary>
        /// Gets the symbol type.
        /// </summary>
        public SymbolType Type { get; }
    }
}
