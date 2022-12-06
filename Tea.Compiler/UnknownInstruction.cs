//-----------------------------------------------------------------------
// <copyright file="UnknownInstruction.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler
{
    using System;

    /// <summary>
    /// ASM only instruction that is not implemented in machine code yet.
    /// </summary>
    public class UnknownInstruction : Instruction
    {
        private readonly string text;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownInstruction"/> class.
        /// </summary>
        /// <param name="text">The ASM text.</param>
        public UnknownInstruction(string text)
        {
            this.text = text;
        }

        /// <inheritdoc/>
        public override byte[] ToArray()
        {
            return Array.Empty<byte>();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.text + "; TODO: encode.";
        }
    }
}