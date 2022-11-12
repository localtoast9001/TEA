//-----------------------------------------------------------------------
// <copyright file="SymbolBinding.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler.Elf
{
    /// <summary>
    /// Symbol binding values.
    /// </summary>
    public enum SymbolBinding
    {
        /// <summary>
        /// Local scope.
        /// </summary>
        Local = 0,

        /// <summary>
        /// Global scope.
        /// </summary>
        Global = 1,

        /// <summary>
        /// Weak, (ie. __attribute__((weak))).
        /// </summary>
        Weak = 2,
    }
}