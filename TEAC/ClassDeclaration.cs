//-----------------------------------------------------------------------
// <copyright file="ClassDeclaration.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TEAC
{
    /// <summary>
    /// Type declaration for a class.
    /// </summary>
    internal class ClassDeclaration : TypeDeclaration
    {
        private readonly List<MethodDeclaration> publicMethods = new List<MethodDeclaration>();
        private readonly List<MethodDeclaration> protectedMethods = new List<MethodDeclaration>();
        private readonly List<MethodDeclaration> privateMethods = new List<MethodDeclaration>();
        private readonly List<string> interfaces = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassDeclaration"/> class.
        /// </summary>
        /// <param name="start">The first token in the parse node.</param>
        /// <param name="name">The name of the type.</param>
        /// <param name="baseType">Optional base type.</param>
        /// <param name="isStatic">True if the class is static; otherwise, false.</param>
        public ClassDeclaration(
            Token start,
            string name,
            string? baseType,
            bool isStatic)
            : base(start, name)
        {
            this.IsStatic = isStatic;
            this.BaseType = baseType;
        }

        /// <summary>
        /// Gets a value indicating whether the class is static.
        /// </summary>
        public bool IsStatic { get; }

        /// <summary>
        /// Gets the base type.
        /// </summary>
        public string? BaseType { get; }

        /// <summary>
        /// Gets the class fields.
        /// </summary>
        public VarBlock? Fields { get; set; }

        public IEnumerable<MethodDeclaration> PublicMethods
        {
            get { return this.publicMethods; }
        }

        /// <summary>
        /// Gets the set of protected methods.
        /// </summary>
        public IEnumerable<MethodDeclaration> ProtectedMethods { get { return this.protectedMethods; } }

        /// <summary>
        /// Gets the set of private methods.
        /// </summary>
        public IEnumerable<MethodDeclaration> PrivateMethods { get { return this.privateMethods; } }

        /// <summary>
        /// Gets the set of interfaces.
        /// </summary>
        public IEnumerable<string> Interfaces { get { return this.interfaces; } }

        /// <summary>
        /// Adds a public method.
        /// </summary>
        /// <param name="method">The method to add.</param>
        public void AddPublicMethod(MethodDeclaration method)
        {
            this.publicMethods.Add(method);
        }

        /// <summary>
        /// Adds a protected method.
        /// </summary>
        /// <param name="method">The method to add.</param>
        public void AddProtectedMethod(MethodDeclaration method)
        {
            this.protectedMethods.Add(method);
        }

        /// <summary>
        /// Adds a private method.
        /// </summary>
        /// <param name="method">The private method to add.</param>
        public void AddPrivateMethod(MethodDeclaration method)
        {
            this.privateMethods.Add(method);
        }

        /// <summary>
        /// Adds an interface.
        /// </summary>
        /// <param name="interfaceName">The name of the interface.</param>
        public void AddInterface(string interfaceName)
        {
            this.interfaces.Add(interfaceName);
        }
    }
}
