//-----------------------------------------------------------------------
// <copyright file="RelocationTableSection.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler.Elf
{
    using Tea.Compiler.Binary;

    /// <summary>
    /// A Section of relocation table entries.
    /// </summary>
    internal class RelocationTableSection : Section
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RelocationTableSection"/> class.
        /// </summary>
        /// <param name="target">The target that will have the relocations.</param>
        public RelocationTableSection(ProgramSection target)
        {
            this.Target = target;
        }

        /// <summary>
        /// Gets the list of symbols.
        /// </summary>
        public IList<Rel32> Relocations { get; } = new List<Rel32>();

        /// <summary>
        /// Gets the section to which the relocation applies.
        /// </summary>
        public ProgramSection Target { get; }

        /// <inheritdoc/>
        internal override SectionType Type => SectionType.Rel;

        /// <inheritdoc/>
        internal override SectionFlags Flags => SectionFlags.InfoLink;

        /// <inheritdoc/>
        internal override uint Size => (uint)this.Relocations.Count * Rel32.BinarySize;

        /// <inheritdoc/>
        internal override uint EntrySize => Rel32.BinarySize;

        /// <inheritdoc/>
        internal override void InternalSerialize(BinaryWriter writer)
        {
            // writer.Skip(Rel32.BinarySize);
            foreach (Rel32 rel in this.Relocations)
            {
                rel.Serialize(writer);
            }
        }
    }
}