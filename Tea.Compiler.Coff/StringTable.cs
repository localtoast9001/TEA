// <copyright file="StringTable.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>

namespace Tea.Compiler.Coff
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Tea.Compiler.Binary;

    /// <summary>
    /// String table that is part of the COFF binary.
    /// </summary>
    internal class StringTable : ISerializable
    {
        private readonly List<string> strings = new List<string>();
        private readonly Dictionary<string, uint> indexes = new Dictionary<string, uint>(StringComparer.Ordinal);
        private uint offset;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringTable"/> class.
        /// </summary>
        public StringTable()
        {
            _ = this.DefineString(string.Empty);
        }

        /// <summary>
        /// Defines a new string or returns the index to an existing string if it is already defined.
        /// </summary>
        /// <param name="value">The string to define.</param>
        /// <returns>The index of the new string or the index of the existing string.</returns>
        public uint DefineString(string value)
        {
            if (this.indexes.TryGetValue(value, out uint index))
            {
                return index;
            }

            int size = Encoding.UTF8.GetByteCount(value) + 1;
            this.strings.Add(value);

            uint result = this.offset;
            this.indexes.Add(value, result);
            this.offset += (uint)size;
            return result;
        }

        /// <inheritdoc/>
        public void Serialize(BinaryWriter writer)
        {
            // This is the size of the string table.
            writer.WriteUInt32(this.offset);

            foreach (string s in this.strings)
            {
                byte[] data = Encoding.UTF8.GetBytes(s);
                writer.WriteBytes(data);
                writer.WriteByte(0);
            }
        }
    }
}
