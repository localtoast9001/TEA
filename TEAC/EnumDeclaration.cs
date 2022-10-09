//-----------------------------------------------------------------------
// <copyright file="EnumDeclaration.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TEAC
{
    using System.Collections.Generic;

    internal class EnumDeclaration : TypeDeclaration
    {
        private readonly List<string> values = new List<string>();

        public EnumDeclaration(Token start, string name)
            : base(start, name)
        {
        }

        public IEnumerable<string> Values
        {
            get
            {
                return this.values;
            }
        }

        public void AddValue(string name)
        {
            this.values.Add(name);
        }
    }
}
