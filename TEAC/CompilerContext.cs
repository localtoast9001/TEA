﻿//-----------------------------------------------------------------------
// <copyright file="CompilerContext.cs" company="Jon Rowlett">
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
    /// Modifiable state used by the compiler as it goes through parsing and code generation phases.
    /// </summary>
    internal class CompilerContext
    {
        private readonly List<string> uses = new List<string>();
        private readonly List<string> includes = new List<string>();
        private readonly Dictionary<string, TypeDefinition> types = new Dictionary<string, TypeDefinition>();
        private readonly HashSet<string> alreadyUsed = new HashSet<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CompilerContext"/> class.
        /// </summary>
        public CompilerContext()
        {
            TypeDefinition intType = new TypeDefinition
            {
                FullName = "integer",
                Size = 4,
                SpecialMangledName = "i",
            };

            TypeDefinition shortType = new TypeDefinition
            {
                FullName = "short",
                Size = 2,
                SpecialMangledName = "i2",
            };

            TypeDefinition longType = new TypeDefinition
            {
                FullName = "long",
                Size = 8,
                SpecialMangledName = "i8",
            };

            TypeDefinition charType = new TypeDefinition
            {
                FullName = "character",
                Size = 2,
                SpecialMangledName = "c",
            };

            TypeDefinition boolType = new TypeDefinition
            {
                FullName = "boolean",
                Size = 1,
                SpecialMangledName = "f",
            };

            TypeDefinition byteType = new TypeDefinition
            {
                FullName = "byte",
                Size = 1,
                SpecialMangledName = "b",
            };

            TypeDefinition singleType = new TypeDefinition
            {
                FullName = "single",
                Size = 4,
                SpecialMangledName = "s",
                IsFloatingPoint = true,
            };

            TypeDefinition doubleType = new TypeDefinition
            {
                FullName = "double",
                Size = 8,
                SpecialMangledName = "d",
                IsFloatingPoint = true,
            };

            TypeDefinition extendedType = new TypeDefinition
            {
                FullName = "extended",
                Size = 10,
                SpecialMangledName = "e",
                IsFloatingPoint = true,
            };

            TypeDefinition genericPointerType = new TypeDefinition
            {
                FullName = "^",
                Size = 4,
                SpecialMangledName = "p",
                IsPointer = true,
            };

            this.types.Add(intType.FullName, intType);
            this.types.Add(shortType.FullName, shortType);
            this.types.Add(longType.FullName, longType);
            this.types.Add(charType.FullName, charType);
            this.types.Add(singleType.FullName, singleType);
            this.types.Add(doubleType.FullName, doubleType);
            this.types.Add(extendedType.FullName, extendedType);
            this.types.Add(boolType.FullName, boolType);
            this.types.Add(byteType.FullName, byteType);
            this.types.Add(genericPointerType.FullName, genericPointerType);
        }

        /// <summary>
        /// Gets or sets the namespace.
        /// </summary>
        public string? Namespace { get; set; }

        /// <summary>
        /// Gets the uses namespaces.
        /// </summary>
        public IEnumerable<string> Uses
        {
            get { return this.uses; }
        }

        /// <summary>
        /// Gets the include paths.
        /// </summary>
        public IEnumerable<string> Includes
        {
            get { return this.includes; }
        }

        /// <summary>
        /// Adds include search paths to the context.
        /// </summary>
        /// <param name="paths">The include search paths.</param>
        public void AddIncludePaths(IEnumerable<string> paths)
        {
            this.includes.AddRange(paths);
        }

        /// <summary>
        /// Tests if a namespace is already used.
        /// </summary>
        /// <param name="ns">The namespace.</param>
        public bool AlreadyUsed(string ns)
        {
            return this.alreadyUsed.Contains(ns);
        }

        /// <summary>
        /// Adds a namespace to the already used set.
        /// </summary>
        /// <param name="ns">The namespace.</param>
        public void AddAlreadyUsed(string ns)
        {
            this.alreadyUsed.Add(ns);
        }

        /// <summary>
        /// Adds a namespace to the uses set.
        /// </summary>
        /// <param name="ns">The namespace.</param>
        public void AddUses(string ns)
        {
            this.uses.Add(ns);
        }

        /// <summary>
        /// Tries to find a type definition by name.
        /// </summary>
        /// <param name="nameRef">The name to search.</param>
        /// <param name="type">On success, receives a reference to the matching type definition.</param>
        /// <returns>True if the type is found; otherwise, false.</returns>
        public bool TryFindTypeByName(string nameRef, out TypeDefinition? type)
        {
            if (this.types.TryGetValue(nameRef, out type))
            {
                return true;
            }

            if (this.types.TryGetValue(this.Namespace + "." + nameRef, out type))
            {
                return true;
            }

            foreach (string ns in this.uses)
            {
                if (this.types.TryGetValue(ns + "." + nameRef, out type))
                {
                    return true;
                }
            }

            type = null;
            return false;
        }

        public bool TryFindConstructor(
            TypeDefinition objectType,
            IList<TypeDefinition> parameterTypes,
            out MethodInfo? methodInfo)
        {
            methodInfo = null;
            foreach (MethodInfo constructor in objectType.Methods.Where(e => string.CompareOrdinal("constructor", e.Name) == 0))
            {
                if (constructor.Parameters.Count != parameterTypes.Count)
                {
                    continue;
                }

                bool match = true;
                for (int i = 0; i < parameterTypes.Count; i++)
                {
                    if (string.CompareOrdinal(constructor.Parameters[i].Type?.FullName, parameterTypes[i].FullName) != 0)
                    {
                        match = false;
                        break;
                    }
                }

                if (match)
                {
                    methodInfo = constructor;
                    return true;
                }
            }

            return false;
        }

        public bool TryFindMethodAndType(
            string methodNameRef,
            IList<TypeDefinition> parameterTypes,
            out TypeDefinition? type,
            out MethodInfo? methodInfo)
        {
            type = null;
            methodInfo = null;
            int methodStart = methodNameRef.LastIndexOf('.');
            if (methodStart < 0)
            {
                return false;
            }

            string methodName = methodNameRef.Substring(methodStart + 1);
            string typeName = methodNameRef.Substring(0, methodStart);
            if (!this.TryFindTypeByName(typeName, out type))
            {
                return false;
            }

            foreach (var test in type?.Methods ?? new List<MethodInfo>())
            {
                if (string.CompareOrdinal(test.Name, methodName) == 0)
                {
                    bool match = true;
                    if (test.Parameters.Count != parameterTypes.Count)
                    {
                        continue;
                    }

                    for (int i = 0; i < test.Parameters.Count; i++)
                    {
                        if (string.CompareOrdinal(test.Parameters[i].Type?.FullName, parameterTypes[i].FullName) != 0)
                        {
                            match = false;
                            break;
                        }
                    }

                    if (!match)
                    {
                        continue;
                    }

                    methodInfo = test;
                    return true;
                }
            }

            return false;
        }

        public TypeDefinition GetPointerType(TypeDefinition elementType)
        {
            TypeDefinition? result = null;
            string fullName = "^" + elementType.FullName;
            if (!this.types.TryGetValue(fullName, out result))
            {
                result = new TypeDefinition
                {
                    FullName = fullName,
                    IsPointer = true,
                    Size = 4,
                    InnerType = elementType,
                };

                this.types.Add(fullName, result);
            }

            return result;
        }

        public TypeDefinition GetArrayType(TypeDefinition elementType, int elementCount)
        {
            TypeDefinition? result = null;
            string fullName = "#" + elementCount.ToString() + elementType.FullName;
            if (!this.types.TryGetValue(fullName, out result))
            {
                result = new TypeDefinition
                {
                    FullName = fullName,
                    IsArray = true,
                    Size = elementCount > 0 ? elementType.Size * elementCount : 4,
                    ArrayElementCount = elementCount,
                    InnerType = elementType,
                };

                this.types.Add(fullName, result);
            }

            return result;
        }

        public bool TryDeclareType(string name, out TypeDefinition typeDef)
        {
            string fullName = this.Namespace + "." + name;
            TypeDefinition? existing = null;
            if (this.types.TryGetValue(fullName, out existing))
            {
                typeDef = existing;
                return existing.Size == 0;
            }

            typeDef = new TypeDefinition
            {
                FullName = fullName,
            };

            this.types.Add(fullName, typeDef);
            return true;
        }

        public Scope BeginScope(MethodInfo method)
        {
            Scope scope = new Scope();
            if (method.ReturnType != null)
            {
                LocalVariable returnVar = scope.DefineLocalVariable(method.Name!, method.ReturnType!);
                scope.ReturnVariable = returnVar;
                if ((method.ReturnType.Size > 8 && !method.ReturnType.IsFloatingPoint) || method.ReturnType.IsClass)
                {
                    scope.LargeReturnVariable = scope.DefineParameter(
                        "$result",
                        this.GetPointerType(method.ReturnType));
                }
            }

            if (!method.IsStatic)
            {
                scope.DefineParameter("this", this.GetPointerType(method.Type!));
            }

            foreach (ParameterInfo parameterInfo in method.Parameters)
            {
                scope.DefineParameter(parameterInfo.Name!, parameterInfo.Type!);
            }

            return scope;
        }

        public TypeDefinition GetMethodType(MethodInfo calleeMethod)
        {
            StringBuilder sb = new StringBuilder();
            if (calleeMethod.ReturnType != null)
            {
                sb.Append(calleeMethod.ReturnType.FullName);
            }

            sb.Append("(");
            bool firstArg = true;
            if (!calleeMethod.IsStatic)
            {
                sb.Append(calleeMethod.Type?.FullName);
                firstArg = false;
            }

            if (calleeMethod.Parameters.Count > 0)
            {
                foreach (var parameter in calleeMethod.Parameters)
                {
                    if (!firstArg)
                    {
                        sb.Append(",");
                    }

                    sb.Append(parameter.Type!.FullName!);

                    firstArg = false;
                }
            }

            sb.Append(")");

            string fullName = sb.ToString();
            TypeDefinition? methodType = null;
            if (!this.types.TryGetValue(fullName, out methodType))
            {
                methodType = new TypeDefinition();
                methodType.IsMethod = true;
                methodType.Size = 4;
                methodType.MethodReturnType = calleeMethod.ReturnType;
                methodType.FullName = fullName;
                if (!calleeMethod.IsStatic)
                {
                    methodType.MethodImplicitArgType = calleeMethod.Type;
                }

                foreach (var p in calleeMethod.Parameters)
                {
                    methodType.MethodParamTypes.Add(p.Type!);
                }

                this.types.Add(methodType.FullName, methodType);
            }

            return methodType;
        }

        internal void ClearUses()
        {
            this.uses.Clear();
        }
    }
}
