//-----------------------------------------------------------------------
// <copyright file="IBinaryWriter.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler.Binary
{
    /// <summary>
    /// Binary writer interface.
    /// </summary>
    public interface IBinaryWriter
    {
        /// <summary>
        /// Writes the data to the underlying stream.
        /// </summary>
        /// <param name="data">The data to write.</param>
        void WriteBytes(ReadOnlySpan<byte> data);

        /// <summary>
        /// Skips or pads the output by a number of bytes.
        /// </summary>
        /// <param name="count">The number of bytes to skip.</param>
        void Skip(uint count);

        /// <summary>
        /// Writes a byte to the stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        void WriteByte(byte value);

        /// <summary>
        /// Writes an unsigned short to the stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        void WriteUInt16(ushort value);

        /// <summary>
        /// Writes an unsigned int to the stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        void WriteUInt32(uint value);

        /// <summary>
        /// Writes a signed short to the stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        void WriteInt16(short value);

        /// <summary>
        /// Writes a signed int to the stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        void WriteInt32(int value);
    }
}