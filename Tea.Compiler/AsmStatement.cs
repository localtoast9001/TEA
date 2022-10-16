//-----------------------------------------------------------------------
// <copyright file="AsmStatement.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler
{
    /// <summary>
    /// Assembly statement.
    /// </summary>
    public class AsmStatement
    {
        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        public string? Label { get; set; }

        /// <summary>
        /// Gets or sets the instruction.
        /// </summary>
        public string? Instruction { get; set; }
    }
}