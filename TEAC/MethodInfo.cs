//-----------------------------------------------------------------------
// <copyright file="MethodInfo.cs" company="Jon Rowlett">
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
    /// Information about a method used in code generation.
    /// </summary>
    internal class MethodInfo
    {
        private readonly List<ParameterInfo> parameters = new List<ParameterInfo>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodInfo"/> class.
        /// </summary>
        /// <param name="type">The type on which the method is defined.</param>
        public MethodInfo(TypeDefinition? type)
        {
            this.Type = type;
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public TypeDefinition? Type { get; }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        public List<ParameterInfo> Parameters
        {
            get { return this.parameters; }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the method is static.
        /// </summary>
        public bool IsStatic { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the method is virtual.
        /// </summary>
        public bool IsVirtual { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the method is abstract.
        /// </summary>
        public bool IsAbstract { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the method is public.
        /// </summary>
        public bool IsPublic { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the method is protected.
        /// </summary>
        public bool IsProtected { get; set; }

        /// <summary>
        /// Gets or sets a the return type.
        /// </summary>
        public TypeDefinition? ReturnType { get; set; }

        /// <summary>
        /// Gets or sets the method's vtable index.
        /// </summary>
        public int VTableIndex { get; set; }

        /// <summary>
        /// Gets the mangled name.
        /// </summary>
        public string MangledName
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(this.Type?.MangledName ?? string.Empty);
                sb.AppendFormat("{0}{1}E", this.Name?.Length, this.Name ?? string.Empty);
                if (this.Parameters.Count > 0)
                {
                    foreach (var parameter in this.Parameters)
                    {
                        sb.Append(parameter.Type?.MangledName ?? string.Empty);
                    }
                }
                else
                {
                    sb.Append("v");
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// Assigns the Vtable index.
        /// </summary>
        public void AssignVTableIndex()
        {
            if (!this.IsVirtual)
            {
                return;
            }

            TypeDefinition? type = this.Type?.BaseClass;
            int maxIndex = -1;
            while (type != null)
            {
                foreach (MethodInfo method in type.Methods)
                {
                    if (!method.IsVirtual)
                    {
                        continue;
                    }

                    maxIndex = maxIndex > method.VTableIndex ? maxIndex : method.VTableIndex;
                    if (string.CompareOrdinal(method.Name, this.Name) == 0)
                    {
                        bool match = true;
                        if (method.Parameters.Count != this.Parameters.Count)
                        {
                            continue;
                        }

                        for (int i = 0; i < method.Parameters.Count; i++)
                        {
                            if (string.CompareOrdinal(method.Parameters[i].Type?.FullName, this.Parameters[i].Type?.FullName) != 0)
                            {
                                match = false;
                                break;
                            }
                        }

                        if (match)
                        {
                            this.VTableIndex = method.VTableIndex;
                            return;
                        }
                    }
                }

                type = type.BaseClass;
            }

            foreach (MethodInfo method in this.Type?.Methods ?? new List<MethodInfo>())
            {
                if (!method.IsVirtual)
                {
                    continue;
                }

                if (string.CompareOrdinal(method.MangledName, this.MangledName) == 0)
                {
                    continue;
                }

                maxIndex = maxIndex > method.VTableIndex ? maxIndex : method.VTableIndex;
            }

            this.VTableIndex = maxIndex + 1;
        }
    }
}
