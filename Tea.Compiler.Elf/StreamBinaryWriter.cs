//-----------------------------------------------------------------------
// <copyright file="StreamBinaryWriter.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler.Elf
{
    using System.IO;

    /// <summary>
    /// Implementation of a <see cref="BinaryWriter"/> that writes to a stream.
    /// </summary>
    internal class StreamBinaryWriter : BinaryWriter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamBinaryWriter"/> class.
        /// </summary>
        /// <param name="stream">The underlying stream.</param>
        public StreamBinaryWriter(Stream stream)
        {
            this.Stream = stream ?? throw new ArgumentNullException(nameof(stream));
        }

        /// <summary>
        /// Gets the stream.
        /// </summary>
        internal Stream Stream { get; }

        /// <inheritdoc/>
        public override void WriteBytes(ReadOnlySpan<byte> data)
        {
            this.Stream.Write(data);
        }
    }
}