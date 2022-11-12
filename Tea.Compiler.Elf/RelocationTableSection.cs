//-----------------------------------------------------------------------
// <copyright file="RelocationTableSection.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler.Elf
{
    /// <summary>
    /// A Section of relocation table entries.
    /// </summary>
    internal class RelocationTableSection : Section
    {
        /// <summary>
        /// Gets the list of symbols.
        /// </summary>
        public IList<Rel32> Relocations { get; } = new List<Rel32>();

        /// <inheritdoc/>
        internal override SectionType Type => SectionType.Rel;

        /// <inheritdoc/>
        internal override SectionFlags Flags => SectionFlags.InfoLink;

        /// <inheritdoc/>
        internal override uint Size => (uint)(this.Relocations.Count + 1) * Rel32.BinarySize;

        /// <inheritdoc/>
        internal override uint EntrySize => Rel32.BinarySize;

        /// <inheritdoc/>
        internal override void InternalSerialize(BinaryWriter writer)
        {
            writer.Skip(Rel32.BinarySize);
            foreach (Rel32 rel in this.Relocations)
            {
                rel.Serialize(writer);
            }
        }
    }
}