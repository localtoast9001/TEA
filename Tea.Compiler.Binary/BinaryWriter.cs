//-----------------------------------------------------------------------
// <copyright file="BinaryWriter.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler.Binary
{
    /// <summary>
    /// Abstract binary writer.
    /// </summary>
    public abstract class BinaryWriter : IBinaryWriter
    {
        /// <summary>
        /// Writes the data to the underlying stream.
        /// </summary>
        /// <param name="data">The data to write.</param>
        public abstract void WriteBytes(ReadOnlySpan<byte> data);

        /// <summary>
        /// Skips or pads the output by a number of bytes.
        /// </summary>
        /// <param name="count">The number of bytes to skip.</param>
        public virtual void Skip(uint count)
        {
            if (count > 0)
            {
                this.WriteBytes(new byte[count]);
            }
        }

        /// <summary>
        /// Writes a byte to the stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void WriteByte(byte value)
        {
            this.WriteBytes(new byte[] { value });
        }

        /// <summary>
        /// Writes an unsigned short to the stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public virtual void WriteUInt16(ushort value)
        {
            this.WriteBytes(new byte[] { (byte)value, (byte)(value >> 8) });
        }

        /// <summary>
        /// Writes an unsigned int to the stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public virtual void WriteUInt32(uint value)
        {
            this.WriteBytes(new byte[] { (byte)value, (byte)(value >> 8), (byte)(value >> 16), (byte)(value >> 24) });
        }

        /// <summary>
        /// Writes a signed short to the stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public virtual void WriteInt16(short value)
        {
            this.WriteUInt16((ushort)value);
        }

        /// <summary>
        /// Writes a signed int to the stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public virtual void WriteInt32(int value)
        {
            this.WriteUInt32((uint)value);
        }
    }
}