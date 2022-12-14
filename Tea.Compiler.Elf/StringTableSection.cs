//-----------------------------------------------------------------------
// <copyright file="StringTableSection.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler.Elf
{
    /// <summary>
    /// String table section.
    /// </summary>
    public class StringTableSection : Section
    {
        private readonly Dictionary<string, uint> stringOffsets = new Dictionary<string, uint>(StringComparer.Ordinal);

        private readonly List<string> stringTable = new List<string>();

        private uint size;

        /// <inheritdoc/>
        internal override uint Align => sizeof(int);

        /// <inheritdoc/>
        internal override SectionFlags Flags => SectionFlags.Strings;

        /// <inheritdoc/>
        internal override uint Size => this.size;

        /// <inheritdoc/>
        internal override SectionType Type => SectionType.StrTab;

        /// <summary>
        /// Defines a string.
        /// </summary>
        /// <param name="name">The string.</param>
        /// <returns>The index into the section for the string.</returns>
        public uint DefineString(string name)
        {
            if (!this.stringOffsets.TryGetValue(name, out uint result))
            {
                result = this.size;
                this.stringTable.Add(name);
                this.stringOffsets.Add(name, result);
                this.size += (uint)name.Length + 1;
            }

            return result;
        }

        /// <inheritdoc/>
        internal override void InternalSerialize(BinaryWriter writer)
        {
            foreach (string str in this.stringTable)
            {
                writer.WriteBytes(System.Text.Encoding.ASCII.GetBytes(str));
                writer.WriteByte(0);
            }
        }
    }
}