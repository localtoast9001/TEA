//-----------------------------------------------------------------------
// <copyright file="InterfaceDeclaration.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TEAC
{
    using System;
    using System.Collections.Generic;

    internal class InterfaceDeclaration : TypeDeclaration
    {
        private List<MethodDeclaration> methods = new List<MethodDeclaration>();

        public InterfaceDeclaration(
            Token start,
            string name)
            : base(start, name)
        {
        }

        public string? BaseInterfaceType { get; set; }
        public IList<MethodDeclaration> Methods { get { return this.methods; } }
    }
}
