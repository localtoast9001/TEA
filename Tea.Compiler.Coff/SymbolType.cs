// <copyright file="SymbolType.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>

namespace Tea.Compiler.Coff
{
    /// <summary>
    /// Lower 4 bits of the symbol type.
    /// </summary>
    internal enum BaseSymbolType : byte
    {
        /// <summary>
        /// No type specified.
        /// </summary>
        Null = 0,

        /// <summary>
        /// Void. Not used.
        /// </summary>
        Void,

        /// <summary>
        /// Character type.
        /// </summary>
        Char,

        /// <summary>
        /// Short type.
        /// </summary>
        Short,

        /// <summary>
        /// Integer type.
        /// </summary>
        Int,

        /// <summary>
        /// Long type.
        /// </summary>
        Long,

        /// <summary>
        /// 32-bit Float type.
        /// </summary>
        Float,

        /// <summary>
        /// 64-bit floating point.
        /// </summary>
        Double,

        /// <summary>
        /// structure type.
        /// </summary>
        Struct,

        /// <summary>
        /// Union type.
        /// </summary>
        Union,

        /// <summary>
        /// Enum type.
        /// </summary>
        Enum,

        /// <summary>
        /// Member of enumeration.
        /// </summary>
        MemberOfEnumeration,

        /// <summary>
        /// Unsigned char type.
        /// </summary>
        UChar,

        /// <summary>
        /// Unsigned short type.
        /// </summary>
        UShort,

        /// <summary>
        /// Unsigned integer type.
        /// </summary>
        UInt,

        /// <summary>
        /// Unsigned long type.
        /// </summary>
        ULong,
    }

    /// <summary>
    /// Upper 4 bits of the symbol type.
    /// </summary>
    internal enum DerivedSymbolType : byte
    {
        /// <summary>
        /// No derivation.
        /// </summary>
        None = 0,

        /// <summary>
        /// Pointer to the type specified in the base.
        /// </summary>
        Pointer = 0x10,

        /// <summary>
        /// Pointer to a function returning the type specified in the base.
        /// </summary>
        Func = 0x20,

        /// <summary>
        /// Array of the type specified in the base.
        /// </summary>
        Array = 0x30,
    }

    /// <summary>
    /// Type field in <see cref="SymbolEntry"/>.
    /// </summary>
    /// <remarks>
    /// Information was sourced from http://www.delorie.com/djgpp/doc/coff/symtab.html.
    /// </remarks>
    internal struct SymbolType
    {

        /// <summary>
        /// A symbol type for a function.
        /// </summary>
        public static readonly SymbolType Function = new SymbolType(BaseSymbolType.Null, DerivedSymbolType.Func);

        private readonly byte value;

        private SymbolType(BaseSymbolType b, DerivedSymbolType d)
        {
            this.value = (byte)((byte)b | (byte)d);
        }

        public byte Value => this.value;
    }
}
