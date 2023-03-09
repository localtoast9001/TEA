// <copyright file="StorageClass.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>

namespace Tea.Compiler.Coff
{
    /// <summary>
    /// The storage class for a symbol.
    /// </summary>
    /// <remarks>
    /// Information was sourced from http://www.delorie.com/djgpp/doc/coff/symtab.html.
    /// </remarks>
    public enum StorageClass : byte
    {
        /// <summary>
        /// No entry.
        /// </summary>
        Null = 0,

        /// <summary>
        /// Automatic variable.
        /// </summary>
        Auto = 1,

        /// <summary>
        /// External (public) symbol - this covers globals and externs.
        /// </summary>
        External = 2,

        /// <summary>
        /// Static (private) symbol.
        /// </summary>
        Static = 3,

        /// <summary>
        /// Register variable.
        /// </summary>
        Register = 4,

        /// <summary>
        /// External definition.
        /// </summary>
        ExternalDef = 5,

        /// <summary>
        /// Label.
        /// </summary>
        Label = 6,

        /// <summary>
        /// Undefined label.
        /// </summary>
        UndefinedLabel = 7,

        /// <summary>
        /// Member of structure.
        /// </summary>
        MemberOfStructure = 8,

        /// <summary>
        /// Function argument.
        /// </summary>
        Argument = 9,

        /// <summary>
        /// Structure tag.
        /// </summary>
        StructureTag = 10,

        /// <summary>
        /// Member of union.
        /// </summary>
        MemberOfUnion = 11,

        /// <summary>
        /// Union tag.
        /// </summary>
        UnionTag = 12,

        /// <summary>
        /// Type definition.
        /// </summary>
        TypeDefinition = 13,

        /// <summary>
        /// Undefined static.
        /// </summary>
        UndefinedStatic = 14,

        /// <summary>
        /// Enum tag.
        /// </summary>
        EnumTag = 15,

        /// <summary>
        /// Member of enum.
        /// </summary>
        MemberOfEnum = 16,

        /// <summary>
        /// Register parameter.
        /// </summary>
        RegisterParameter = 17,

        /// <summary>
        /// Bit field.
        /// </summary>
        BitField = 18,

        /// <summary>
        /// Auto argument.
        /// </summary>
        AutoArgument = 19,

        /// <summary>
        /// Dummy entry (end of block).
        /// </summary>
        LastEntry = 20,

        /// <summary>
        /// &quot;.bb&quot; or &quot;.eb&quot; - beginning or end of block
        /// </summary>
        Block = 100,

        /// <summary>
        /// &quot;.bf&quot; or &quot;.ef&quot; - beginning or end of function
        /// </summary>
        Function = 101,

        /// <summary>
        /// End of structure.
        /// </summary>
        EndOfStructure = 102,

        /// <summary>
        /// File name.
        /// </summary>
        File = 103,

        /// <summary>
        /// Line number, reformatted as symbol.
        /// </summary>
        Line = 104,

        /// <summary>
        /// Duplicate tag.
        /// </summary>
        Alias = 105,

        /// <summary>
        /// ext symbol in &quot;dmert public lib&quot;.
        /// </summary>
        Hidden = 106,

        /// <summary>
        /// Physical end of function.
        /// </summary>
        EndOfFunction = 255,
    }
}
