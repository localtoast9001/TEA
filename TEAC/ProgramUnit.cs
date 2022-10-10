//-----------------------------------------------------------------------
// <copyright file="ProgramUnit.cs" company="Jon Rowlett">
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
    /// Program unit parse node.
    /// </summary>
    internal class ProgramUnit : ParseNode
    {
        private readonly Dictionary<string, TypeDeclaration> types = new Dictionary<string, TypeDeclaration>();
        private readonly List<TypeDeclaration> typeList = new List<TypeDeclaration>();
        private readonly List<MethodDefinition> methods = new List<MethodDefinition>();
        private string nameSpace = string.Empty;
        private readonly List<string> usesReferences = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgramUnit"/> class.
        /// </summary>
        /// <param name="start">First token in the parse node.</param>
        public ProgramUnit(Token start)
            : base(start)
        {
        }

        /// <summary>
        /// Gets or sets the namespace.
        /// </summary>
        public string Namespace
        {
            get
            {
                return this.nameSpace;
            }

            set
            {
                this.nameSpace = value;
            }
        }

        /// <summary>
        /// Gets the uses references.
        /// </summary>
        public IEnumerable<string> Uses
        {
            get { return this.usesReferences; }
        }

        /// <summary>
        /// Gets the types declared.
        /// </summary>
        public IEnumerable<TypeDeclaration> Types
        {
            get { return this.typeList; }
        }

        /// <summary>
        /// Gets the variables.
        /// </summary>
        public VarBlock? Variables { get; set; }

        /// <summary>
        /// Gets the methods.
        /// </summary>
        public IEnumerable<MethodDefinition> Methods { get { return this.methods; } }

        /// <summary>
        /// Adds a uses reference.
        /// </summary>
        /// <param name="value">The uses namespace value.</param>
        public void AddUses(string value)
        {
            this.usesReferences.Add(value);
        }

        /// <summary>
        /// Adds a type declaration.
        /// </summary>
        /// <param name="type">The typee declaration to add.</param>
        public void AddType(TypeDeclaration type)
        {
            this.types.Add(type.Name, type);
            this.typeList.Add(type);
        }

        /// <summary>
        /// Adds a method definition.
        /// </summary>
        /// <param name="method">The method definition to add.</param>
        public void AddMethod(MethodDefinition method)
        {
            this.methods.Add(method);
        }
    }
}
