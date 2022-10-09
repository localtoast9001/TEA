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
    using System.Threading.Tasks;

    internal class MethodInfo
    {
        private List<ParameterInfo> parameters = new List<ParameterInfo>();

        public MethodInfo(TypeDefinition? type)
        {
            this.Type = type;
        }

        public TypeDefinition? Type { get; }

        public List<ParameterInfo> Parameters
        {
            get { return this.parameters; }
        }

        public string? Name { get; set; }
        public bool IsStatic { get; set; }
        public bool IsVirtual { get; set; }
        public bool IsAbstract { get; set; }
        public bool IsPublic { get; set; }
        public bool IsProtected { get; set; }
        public TypeDefinition? ReturnType { get; set; }
        public int VTableIndex { get; set; }

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
    }
}
