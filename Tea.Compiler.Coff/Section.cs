// <copyright file="Section.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>

namespace Tea.Compiler.Coff
{
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using Tea.Compiler.Binary;

    /// <summary>
    /// Base class for sections in a COFF file.
    /// </summary>
    public abstract class Section : ISerializable
    {
        /// <summary>
        /// Gets or sets the name of the section.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets the collection of symbols.
        /// </summary>
        public virtual IReadOnlyCollection<Symbol> Symbols => new ReadOnlyCollection<Symbol>(Array.Empty<Symbol>());

        /// <summary>
        /// Gets the collection of relocations.
        /// </summary>
        public virtual IReadOnlyCollection<Relocation> Relocations => new ReadOnlyCollection<Relocation>(Array.Empty<Relocation>());

        /// <summary>
        /// Gets the length of the raw data in bytes.
        /// </summary>
        internal virtual uint Length => 0;

        /// <summary>
        /// Gets characteristics flags.
        /// </summary>
        internal virtual ImageSectionCharacteristicFlags Characteristics => this.Align > 0 ?
            (ImageSectionCharacteristicFlags)(GetShift(this.Align) << 20) :
            ImageSectionCharacteristicFlags.None;

        /// <summary>
        /// Gets the alignment for the section (in bytes).
        /// </summary>
        internal virtual int Align => 0;

        /// <summary>
        /// Serializes the section.
        /// </summary>
        /// <param name="writer">The writer to use.</param>
        void ISerializable.Serialize(BinaryWriter writer)
        {
            this.InternalSerialize(writer);
        }

        /// <summary>
        /// Serializes the section.
        /// </summary>
        /// <param name="writer">The writer to use.</param>
        /// <remarks>Workaround for compiler bug where an interface implementation cannot be internal abstract.</remarks>
        internal abstract void InternalSerialize(BinaryWriter writer);

        private static int GetShift(int value)
        {
            int shift = 0;
            while (value > 0)
            {
                value >>= 1;
                shift++;
            }

            return shift;
        }
    }
}
