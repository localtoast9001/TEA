//-----------------------------------------------------------------------
// <copyright file="TypeDefinition.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Type definition intermediate structure.
    /// </summary>
    public class TypeDefinition
    {
        private const string VTablePointerFieldName = "?vtblptr";

        private readonly List<MethodInfo> methods = new List<MethodInfo>();
        private readonly List<FieldInfo> fields = new List<FieldInfo>();
        private readonly Dictionary<string, int> enumValues = new Dictionary<string, int>();
        private readonly List<TypeDefinition> methodParamTypes = new List<TypeDefinition>();
        private readonly List<KeyValuePair<TypeDefinition, int>> interfaces = new List<KeyValuePair<TypeDefinition, int>>();

        /// <summary>
        /// Gets the fields.
        /// </summary>
        public List<FieldInfo> Fields
        {
            get { return this.fields; }
        }

        /// <summary>
        /// Gets the methods.
        /// </summary>
        public List<MethodInfo> Methods
        {
            get { return this.methods; }
        }

        /// <summary>
        /// Gets the enum values.
        /// </summary>
        public IDictionary<string, int> EnumValues
        {
            get { return this.enumValues; }
        }

        /// <summary>
        /// Gets the method parameter types for a method type.
        /// </summary>
        public List<TypeDefinition> MethodParamTypes
        {
            get { return this.methodParamTypes; }
        }

        /// <summary>
        /// Gets the interfaces.
        /// </summary>
        public List<KeyValuePair<TypeDefinition, int>> Interfaces
        {
            get { return this.interfaces; }
        }

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        public string? FullName { get; set; }

        /// <summary>
        /// Gets or sets the size of each instance of the type.
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Gets or sets the array element count.
        /// </summary>
        public int ArrayElementCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is a pointer type.
        /// </summary>
        public bool IsPointer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is an array type.
        /// </summary>
        public bool IsArray { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is a class type.
        /// </summary>
        public bool IsClass { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is a floating point type.
        /// </summary>
        public bool IsFloatingPoint { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this type is public.
        /// </summary>
        public bool IsPublic { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this type is a static class.
        /// </summary>
        public bool IsStaticClass { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this type is an abstract class.
        /// </summary>
        public bool IsAbstractClass { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this type is an interface.
        /// </summary>
        public bool IsInterface { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this type in an enum.
        /// </summary>
        public bool IsEnum { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is a method type.
        /// </summary>
        public bool IsMethod { get; set; }

        /// <summary>
        /// Gets or sets the inner type.
        /// </summary>
        public TypeDefinition? InnerType { get; set; }

        /// <summary>
        /// Gets or sets the base class.
        /// </summary>
        public TypeDefinition? BaseClass { get; set; }

        /// <summary>
        /// Gets or sets the method return type.
        /// </summary>
        public TypeDefinition? MethodReturnType { get; set; }

        /// <summary>
        /// Gets or sets the method impplicit type.
        /// </summary>
        public TypeDefinition? MethodImplicitArgType { get; set; }

        /// <summary>
        /// Gets or sets the special mangled name.
        /// </summary>
        public string? SpecialMangledName { get; set; }

        /// <summary>
        /// Gets the mangled name.
        /// </summary>
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
                    return "P" + this.InnerType?.MangledName ?? string.Empty;
                }

                if (this.IsArray)
                {
                    return "A" + this.InnerType?.MangledName ?? string.Empty;
                }

                StringBuilder sb = new StringBuilder();
                sb.Append("_ZN");
                foreach (string part in this.FullName?.Split('.') ?? new string[0])
                {
                    sb.AppendFormat("{0}{1}", part.Length, part);
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// Gets all interfaces the type implements.
        /// </summary>
        /// <returns>A set of interfaces with interface offsets in the type.</returns>
        public IEnumerable<KeyValuePair<TypeDefinition, int>> GetAllInterfaces()
        {
            Dictionary<string, KeyValuePair<TypeDefinition, int>> allInterfaces =
                new Dictionary<string, KeyValuePair<TypeDefinition, int>>();
            TypeDefinition? classType = this;
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

        /// <summary>
        /// Gets the destructor.
        /// </summary>
        /// <returns>The method info for the destructor or null if the type does not have one.</returns>
        public MethodInfo? GetDestructor()
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

        /// <summary>
        /// Gets the default constructor.
        /// </summary>
        /// <returns>The default constructor or null.</returns>
        public MethodInfo? GetDefaultConstructor()
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

        /// <summary>
        /// Finds a method with the given name.
        /// </summary>
        /// <param name="name">The method name.</param>
        /// <returns>The matching method or null.</returns>
        public MethodInfo? FindMethod(string name)
        {
            return this.FindMethod(name, null, false);
        }

        /// <summary>
        /// Finds a method by name and list of argument types.
        /// </summary>
        /// <param name="name">The name of the method.</param>
        /// <param name="argTypes">The list of arg types to match.</param>
        /// <returns>The matching method or null.</returns>
        public MethodInfo? FindMethod(string name, IList<TypeDefinition>? argTypes)
        {
            return this.FindMethod(name, argTypes, true);
        }

        /// <summary>
        /// Finds a constructor.
        /// </summary>
        /// <param name="argTypes">The list of arg types.</param>
        /// <returns>The matching method or null.</returns>
        public MethodInfo? FindConstructor(IList<TypeDefinition> argTypes)
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

        /// <summary>
        /// Creates a method info for a given method type.
        /// </summary>
        /// <returns>The method info.</returns>
        public MethodInfo CreateMethodInfoForMethodType()
        {
            MethodInfo meth = new MethodInfo(this.MethodImplicitArgType);
            meth.IsStatic = this.MethodImplicitArgType == null;

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

        /// <summary>
        /// Gets the copy constructor for this type.
        /// </summary>
        /// <param name="context">The compiler context.</param>
        /// <returns>The copy constructor or null.</returns>
        public MethodInfo? GetCopyConstructor(CompilerContext context)
        {
            List<TypeDefinition> argTypes = new List<TypeDefinition>();
            argTypes.Add(context.GetPointerType(this));
            return this.FindConstructor(argTypes);
        }

        /// <summary>
        /// Adds an interface to the type.
        /// </summary>
        /// <param name="interfaceType">The interface.</param>
        /// <param name="offset">The field offset where to add the interface.</param>
        public void AddInterface(TypeDefinition interfaceType, int offset)
        {
            this.interfaces.Add(new KeyValuePair<TypeDefinition, int>(interfaceType, offset));
        }

        /// <summary>
        /// Gets the interface table pointer.
        /// </summary>
        /// <param name="interfaceType">The interface type.</param>
        /// <returns>The matching field info for the table pointer or null.</returns>
        public FieldInfo? GetInterfaceTablePointer(TypeDefinition interfaceType)
        {
            return this.GetTablePointer(VTablePointerFieldName + interfaceType.MangledName);
        }

        /// <summary>
        /// Gets the vtable pointer.
        /// </summary>
        /// <returns>The vtable pointer or null if the type does not have one.</returns>
        public FieldInfo? GetVTablePointer()
        {
            return this.GetTablePointer(VTablePointerFieldName);
        }

        /// <summary>
        /// Adds an interface table pointer.
        /// </summary>
        /// <param name="context">The compiler context.</param>
        /// <param name="offset">The offset of the field.</param>
        /// <param name="interfaceType">The interface type.</param>
        /// <returns>The field that was created.</returns>
        public FieldInfo AddInterfaceTablePointer(CompilerContext context, int offset, TypeDefinition interfaceType)
        {
            return this.AddTablePointer(
                context,
                offset,
                VTablePointerFieldName + interfaceType.MangledName);
        }

        /// <summary>
        /// Adds a vtable pointer.
        /// </summary>
        /// <param name="context">The compiler context.</param>
        /// <param name="offset">The offset for the table.</param>
        /// <returns>The vtable field.</returns>
        public FieldInfo AddVTablePointer(CompilerContext context, int offset)
        {
            return this.AddTablePointer(context, offset, VTablePointerFieldName);
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
                if (string.CompareOrdinal(method.Parameters[i].Type?.FullName, argTypes[i].FullName) != 0)
                {
                    match = false;
                    break;
                }
            }

            return match;
        }

        private MethodInfo? FindMethod(string name, IList<TypeDefinition>? argTypes, bool matchArgs)
        {
            TypeDefinition? type = this;
            while (type != null)
            {
                foreach (MethodInfo method in type?.Methods ?? new List<MethodInfo>())
                {
                    if (string.CompareOrdinal(name, method.Name) == 0)
                    {
                        if (!matchArgs || MatchArgs(method, argTypes!))
                        {
                            return method;
                        }
                    }
                }

                type = type!.BaseClass;
            }

            return null;
        }

        private FieldInfo? GetTablePointer(string fieldName)
        {
            Stack<TypeDefinition> typeHierarchy = new Stack<TypeDefinition>();
            TypeDefinition? type = this;
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

        private FieldInfo AddTablePointer(CompilerContext context, int offset, string name)
        {
            TypeDefinition? ptrType = null;
            context.TryFindTypeByName("^", out ptrType);
            FieldInfo field = new FieldInfo
            {
                Name = name,
                IsPublic = true,
                Offset = offset,
                Type = context.GetArrayType(ptrType!, 0),
            };

            this.fields.Add(field);
            return field;
        }
    }
}
