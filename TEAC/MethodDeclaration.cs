//-----------------------------------------------------------------------
// <copyright file="MethodDeclaration.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TEAC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal class MethodDeclaration : ParseNode
    {
        private List<ParameterDeclaration> parameters = new List<ParameterDeclaration>();

        public MethodDeclaration(
            Token start,
            string name,
            bool isStatic,
            bool isVirtual,
            bool isAbstract)
            : base(start)
        {
            this.MethodName = name;
            this.IsStatic = isStatic;
            this.IsVirtual = isVirtual;
            this.IsAbstract = isAbstract;
        }

        public TypeReference? ReturnType { get; set; }

        public string MethodName { get; private set; }

        public bool IsStatic { get; private set; }

        public bool IsVirtual { get; private set; }

        public bool IsAbstract { get; private set; }

        public IEnumerable<ParameterDeclaration> Parameters { get { return this.parameters; } }

        public void AddParameter(ParameterDeclaration parameterDecl)
        {
            this.parameters.Add(parameterDecl);
        }
    }
}
