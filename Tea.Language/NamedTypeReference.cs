//-----------------------------------------------------------------------
// <copyright file="NamedTypeReference.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Language
{
    /// <summary>
    /// Type reference parse node that uses an identifier to reference a type.
    /// </summary>
    public class NamedTypeReference : TypeReference
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NamedTypeReference" /> class.
        /// </summary>
        /// <param name="start">The first token in the parse node.</param>
        /// <param name="typeName">The name of the type referenced.</param>
        public NamedTypeReference(Token start, string typeName)
            : base(start)
        {
            this.TypeName = typeName;
        }

        /// <summary>
        /// Gets the name of the type being referenced.
        /// </summary>
        public string TypeName { get; }
    }
}