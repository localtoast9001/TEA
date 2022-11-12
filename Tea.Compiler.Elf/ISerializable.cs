//-----------------------------------------------------------------------
// <copyright file="ISerializable.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler.Elf
{
    /// <summary>
    /// Mandatory interface for serializable structs.
    /// </summary>
    internal interface ISerializable
    {
        /// <summary>
        /// Serialize to a <see cref="BinaryWriter"/>.
        /// </summary>
        /// <param name="writer">The writer.</param>
        void Serialize(BinaryWriter writer);
    }
}