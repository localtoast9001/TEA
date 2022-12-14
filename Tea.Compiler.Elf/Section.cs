//-----------------------------------------------------------------------
// <copyright file="Section.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler.Elf
{
    /// <summary>
    /// Abstract section in an ELF file.
    /// </summary>
    public abstract class Section : ISerializable
    {
        /// <summary>
        /// Gets or sets the name of the section.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets the size of the section in bytes.
        /// </summary>
        internal virtual uint Size => 0;

        /// <summary>
        /// Gets the alignment of the section. Must be a power of 2.
        /// </summary>
        internal virtual uint Align => sizeof(int);

        /// <summary>
        /// Gets the section type.
        /// </summary>
        internal abstract SectionType Type { get; }

        /// <summary>
        /// Gets the section flags.
        /// </summary>
        internal abstract SectionFlags Flags { get; }

        /// <summary>
        /// Gets the size of each entry in the section for sections with fixed size entries.
        /// </summary>
        internal virtual uint EntrySize => 0;

        /// <summary>
        /// Gets or sets the linked section.
        /// </summary>
        internal Section? Link { get; set; }

        /// <summary>
        /// Gets the info field.
        /// </summary>
        internal virtual uint Info { get; }

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
    }
}