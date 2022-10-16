//-----------------------------------------------------------------------
// <copyright file="DataEntry.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler
{
    /// <summary>
    /// Data segment entry for module output.
    /// </summary>
    public class DataEntry
    {
        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        public string? Label { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
#pragma warning disable SA1011 // False positive.
        public object[]? Value { get; set; }
#pragma warning restore
    }
}