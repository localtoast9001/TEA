//-----------------------------------------------------------------------
// <copyright file="ParameterInfo.cs" company="Jon Rowlett">
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
    /// Defines a parameter that is used inside <see cref="MethodInfo"/>.
    /// </summary>
    internal class ParameterInfo
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public TypeDefinition? Type { get; set; }
    }
}
