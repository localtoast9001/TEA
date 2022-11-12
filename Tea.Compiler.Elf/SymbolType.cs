//-----------------------------------------------------------------------
// <copyright file="SymbolType.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler.Elf
{
    /// <summary>
    /// Symbol type values.
    /// </summary>
    public enum SymbolType
    {
        /// <summary>
        /// No type.
        /// </summary>
        None = 0,

        /// <summary>
        /// Variables, arrays, etc.
        /// </summary>
        Object = 1,

        /// <summary>
        /// Methods or functions.
        /// </summary>
        Func = 2,

        /// <summary>
        /// This symbol is associated with a section.
        /// </summary>
        Section = 3,

        /// <summary>
        /// File symbol.
        /// </summary>
        File = 4,

        /// <summary>
        /// This symbol labels an uninitialized common block.
        /// </summary>
        Common = 5,

        /// <summary>
        /// Thread-local storage symbol.
        /// </summary>
        Tls = 6,
    }
}
