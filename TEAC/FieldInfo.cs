//-----------------------------------------------------------------------
// <copyright file="FieldInfo.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TEAC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Information about a field created as part of code generation.
    /// </summary>
    internal class FieldInfo
    {
        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the type of the field.
        /// </summary>
        public TypeDefinition? Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the field is static.
        /// </summary>
        public bool IsStatic { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the field is public.
        /// </summary>
        public bool IsPublic { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the field is protected.
        /// </summary>
        public bool IsProtected { get; set; }

        /// <summary>
        /// Gets or sets the offset of the field inside the type.
        /// </summary>
        public int Offset { get; set; }
    }
}
