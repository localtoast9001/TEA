namespace TEAC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class TypeDefinition
    {
        private const string VTablePointerFieldName = "?vtblptr";

        private List<MethodInfo> methods = new List<MethodInfo>();
        private List<FieldInfo> fields = new List<FieldInfo>();

        public List<FieldInfo> Fields { get { return this.fields; } }
        public List<MethodInfo> Methods { get { return this.methods; } }

        public string FullName { get; set; }
        public int Size { get; set; }
        public int ArrayElementCount { get; set; }
        public bool IsPointer { get; set; }
        public bool IsArray { get; set; }
        public bool IsClass { get; set; }
        public bool IsFloatingPoint { get; set; }
        public bool IsPublic { get; set; }
        public bool IsStaticClass { get; set; }
        public bool IsAbstractClass { get; set; }
        public bool IsInterface { get; set; }
        public TypeDefinition InnerType { get; set; }
        public TypeDefinition BaseClass { get; set; }
        public string SpecialMangledName { get; set; }
        public string MangledName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.SpecialMangledName))
                {
                    return this.SpecialMangledName;
                }

                if (this.IsPointer)
                {
                    return "P" + this.InnerType.MangledName;
                }

                if (this.IsArray)
                {
                    return "A" + this.InnerType.MangledName;
                }

                StringBuilder sb = new StringBuilder();
                sb.Append("_ZN");
                foreach (string part in this.FullName.Split('.'))
                {
                    sb.AppendFormat("{0}{1}", part.Length, part);
                }

                return sb.ToString();
            }
        }

        public MethodInfo GetDestructor()
        {
            foreach (MethodInfo method in this.Methods)
            {
                if (string.CompareOrdinal("destructor", method.Name) == 0)
                {
                    return method;
                }
            }

            return null;
        }

        public MethodInfo GetDefaultConstructor()
        {
            foreach (MethodInfo method in this.Methods)
            {
                if (method.Parameters.Count != 0)
                {
                    continue;
                }

                if (string.CompareOrdinal("constructor", method.Name) == 0)
                {
                    return method;
                }
            }

            return null;
        }

        public MethodInfo FindConstructor(IList<TypeDefinition> argTypes)
        {
            foreach (MethodInfo method in this.Methods)
            {
                if (string.CompareOrdinal("constructor", method.Name) == 0)
                {
                    if (method.Parameters.Count != argTypes.Count)
                    {
                        continue;
                    }

                    bool match = true;
                    for (int i = 0; i < method.Parameters.Count; i++)
                    {
                        if (string.CompareOrdinal(method.Parameters[i].Type.FullName, argTypes[i].FullName) != 0)
                        {
                            match = false;
                            break;
                        }
                    }

                    if (match)
                    {
                        return method;
                    }
                }
            }

            return null;
        }

        public MethodInfo GetCopyConstructor(CompilerContext context)
        {
            List<TypeDefinition> argTypes = new List<TypeDefinition>();
            argTypes.Add(context.GetPointerType(this));
            return FindConstructor(argTypes);
        }

        public FieldInfo GetVTablePointer()
        {
            Stack<TypeDefinition> typeHierarchy = new Stack<TypeDefinition>();
            TypeDefinition type = this;
            while (type != null)
            {
                typeHierarchy.Push(type);
                type = type.BaseClass;
            }

            while (typeHierarchy.Count > 0)
            {
                type = typeHierarchy.Pop();
                foreach (FieldInfo field in type.Fields)
                {
                    if (!field.IsStatic && string.CompareOrdinal(
                        field.Name,
                        VTablePointerFieldName) == 0)
                    {
                        return field;
                    }
                }
            }

            return null;
        }

        public FieldInfo AddVTablePointer(CompilerContext context, int offset)
        {
            TypeDefinition ptrType = null;
            context.TryFindTypeByName("^", out ptrType);
            FieldInfo field = new FieldInfo
            {
                Name = VTablePointerFieldName, 
                IsPublic = true,
                Offset = offset,
                Type = context.GetArrayType(ptrType, 0)
            };

            this.fields.Add(field);
            return field;
        }
    }
}
