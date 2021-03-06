﻿namespace TEAC
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
        private Dictionary<string, int> enumValues = new Dictionary<string, int>();
        private List<TypeDefinition> methodParamTypes = new List<TypeDefinition>();
        private List<KeyValuePair<TypeDefinition, int>> interfaces = new List<KeyValuePair<TypeDefinition, int>>();

        public List<FieldInfo> Fields { get { return this.fields; } }
        public List<MethodInfo> Methods { get { return this.methods; } }
        public IDictionary<string, int> EnumValues { get { return this.enumValues; } }
        public List<TypeDefinition> MethodParamTypes { get { return this.methodParamTypes; } }
        public List<KeyValuePair<TypeDefinition, int>> Interfaces { get { return this.interfaces; } }

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
        public bool IsEnum { get; set; }
        public bool IsMethod { get; set; }
        public TypeDefinition InnerType { get; set; }
        public TypeDefinition BaseClass { get; set; }
        public TypeDefinition MethodReturnType { get; set; }
        public TypeDefinition MethodImplicitArgType { get; set; }
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

        public IEnumerable<KeyValuePair<TypeDefinition, int>> GetAllInterfaces()
        {
            Dictionary<string, KeyValuePair<TypeDefinition, int>> allInterfaces = 
                new Dictionary<string, KeyValuePair<TypeDefinition, int>>();
            TypeDefinition classType = this;
            while (classType != null)
            {
                foreach (var pair in classType.Interfaces)
                {
                    if (!allInterfaces.ContainsKey(pair.Key.MangledName))
                    {
                        allInterfaces.Add(pair.Key.MangledName, pair);
                    }
                }

                classType = classType.BaseClass;
            }

            return allInterfaces.Values;
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

        public MethodInfo FindMethod(string name)
        {
            return FindMethod(name, null, false);
        }

        public MethodInfo FindMethod(string name, IList<TypeDefinition> argTypes)
        {
            return FindMethod(name, argTypes, true);
        }

        private MethodInfo FindMethod(string name, IList<TypeDefinition> argTypes, bool matchArgs)
        {
            TypeDefinition type = this;
            while (type != null)
            {
                foreach (MethodInfo method in type.Methods)
                {
                    if (string.CompareOrdinal(name, method.Name) == 0)
                    {
                        if (!matchArgs || MatchArgs(method, argTypes))
                        {
                            return method;
                        }
                    }
                }

                type = type.BaseClass;
            }

            return null;
        }

        public MethodInfo FindConstructor(IList<TypeDefinition> argTypes)
        {
            foreach (MethodInfo method in this.Methods)
            {
                if (string.CompareOrdinal("constructor", method.Name) == 0)
                {
                    if (MatchArgs(method, argTypes))
                    {
                        return method;
                    }
                }
            }

            return null;
        }

        public MethodInfo CreateMethodInfoForMethodType()
        {
            MethodInfo meth = new MethodInfo(this.MethodImplicitArgType);
            meth.IsStatic = (this.MethodImplicitArgType == null);

            meth.ReturnType = this.MethodReturnType;
            meth.Name = "???";
            string argName = "_";
            foreach (var argType in this.methodParamTypes)
            {
                meth.Parameters.Add(new ParameterInfo { Name = argName, Type = argType });
                argName = argName + "_";
            }

            return meth;
        }

        private static bool MatchArgs(MethodInfo method, IList<TypeDefinition> argTypes)
        {
            if (method.Parameters.Count != argTypes.Count)
            {
                return false;
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

            return match;
        }

        public MethodInfo GetCopyConstructor(CompilerContext context)
        {
            List<TypeDefinition> argTypes = new List<TypeDefinition>();
            argTypes.Add(context.GetPointerType(this));
            return FindConstructor(argTypes);
        }

        public void AddInterface(TypeDefinition interfaceType, int offset)
        {
            this.interfaces.Add(new KeyValuePair<TypeDefinition,int>(interfaceType, offset));
        }

        public FieldInfo GetInterfaceTablePointer(TypeDefinition interfaceType)
        {
            return GetTablePointer(VTablePointerFieldName + interfaceType.MangledName);
        }

        public FieldInfo GetVTablePointer()
        {
            return GetTablePointer(VTablePointerFieldName);
        }

        private FieldInfo GetTablePointer(string fieldName)
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
                        fieldName) == 0)
                    {
                        return field;
                    }
                }
            }

            return null;
        }

        public FieldInfo AddInterfaceTablePointer(CompilerContext context, int offset, TypeDefinition interfaceType)
        {
            return AddTablePointer(
                context,
                offset,
                VTablePointerFieldName + interfaceType.MangledName);
        }

        public FieldInfo AddVTablePointer(CompilerContext context, int offset)
        {
            return AddTablePointer(context, offset, VTablePointerFieldName);
        }

        private FieldInfo AddTablePointer(CompilerContext context, int offset, string name)
        {
            TypeDefinition ptrType = null;
            context.TryFindTypeByName("^", out ptrType);
            FieldInfo field = new FieldInfo
            {
                Name = name, 
                IsPublic = true,
                Offset = offset,
                Type = context.GetArrayType(ptrType, 0)
            };

            this.fields.Add(field);
            return field;
        }
    }
}
