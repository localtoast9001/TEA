//-----------------------------------------------------------------------
// <copyright file="EnumDeclaration.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TEAC
{
    using System.Collections.Generic;

    /// <summary>
    /// Enum type declaration.
    /// </summary>
    internal class EnumDeclaration : TypeDeclaration
    {
        private readonly List<string> values = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumDeclaration"/> class.
        /// </summary>
        /// <param name="start">The first token in the parse node.</param>
        /// <param name="name">The name of the enum.</param>
        public EnumDeclaration(Token start, string name)
            : base(start, name)
        {
        }

        /// <summary>
        /// Gets the values in the enum.
        /// </summary>
        public IEnumerable<string> Values
        {
            get
            {
                return this.values;
            }
        }

        /// <summary>
        /// Adds a value to the enum.
        /// </summary>
        /// <param name="name">The enum value name.</param>
        public void AddValue(string name)
        {
            this.values.Add(name);
        }
    }
}
