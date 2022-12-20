//-----------------------------------------------------------------------
// <copyright file="CompilerContext.cs" company="Jon Rowlett">
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
    /// Modifiable state used by the compiler as it goes through parsing and code generation phases.
    /// </summary>
    public class CompilerContext
    {
        private readonly List<string> uses = new List<string>();
        private readonly List<string> includes = new List<string>();
        private readonly Dictionary<string, TypeDefinition> types = new Dictionary<string, TypeDefinition>();
        private readonly HashSet<string> alreadyUsed = new HashSet<string>();

        private readonly TypeDefinition intType = new TypeDefinition
        {
            FullName = "integer",
            Size = 4,
            SpecialMangledName = "i",
        };

        private readonly TypeDefinition shortType = new TypeDefinition
        {
            FullName = "short",
            Size = 2,
            SpecialMangledName = "i2",
        };

        private readonly TypeDefinition longType = new TypeDefinition
        {
            FullName = "long",
            Size = 8,
            SpecialMangledName = "i8",
        };

        private readonly TypeDefinition charType = new TypeDefinition
        {
            FullName = "character",
            Size = 1,
            SpecialMangledName = "c",
        };

        private readonly TypeDefinition boolType = new TypeDefinition
        {
            FullName = "boolean",
            Size = 1,
            SpecialMangledName = "f",
        };

        private readonly TypeDefinition byteType = new TypeDefinition
        {
            FullName = "byte",
            Size = 1,
            SpecialMangledName = "b",
        };

        private readonly TypeDefinition singleType = new TypeDefinition
        {
            FullName = "single",
            Size = 4,
            SpecialMangledName = "s",
            IsFloatingPoint = true,
        };

        private readonly TypeDefinition doubleType = new TypeDefinition
        {
            FullName = "double",
            Size = 8,
            SpecialMangledName = "d",
            IsFloatingPoint = true,
        };

        private readonly TypeDefinition extendedType = new TypeDefinition
        {
            FullName = "extended",
            Size = 10,
            SpecialMangledName = "e",
            IsFloatingPoint = true,
        };

        private readonly TypeDefinition genericPointerType = new TypeDefinition
        {
            FullName = "^",
            Size = 4,
            SpecialMangledName = "p",
            IsPointer = true,
        };

        private readonly TypeDefinition charArrayType;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompilerContext"/> class.
        /// </summary>
        public CompilerContext()
        {
            TypeDefinition[] builtinTypes = new[]
            {
                this.intType,
                this.shortType,
                this.longType,
                this.charType,
                this.singleType,
                this.doubleType,
                this.extendedType,
                this.boolType,
                this.byteType,
                this.genericPointerType,
            };

            foreach (TypeDefinition t in builtinTypes)
            {
                this.types.Add(t.FullName!, t);
            }

            this.charArrayType = this.GetArrayType(this.charType, 0);
        }

        /// <summary>
        /// Gets or sets the namespace.
        /// </summary>
        public string? Namespace { get; set; }

        /// <summary>
        /// Gets or sets the source file name.
        /// </summary>
        public string? SourceFileName { get; set; }

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
        /// Gets the integer type.
        /// </summary>
        public TypeDefinition IntegerType => this.intType;

        /// <summary>
        /// Gets the char type.
        /// </summary>
        public TypeDefinition CharType => this.charType;

        /// <summary>
        /// Gets the boolean type.
        /// </summary>
        public TypeDefinition BooleanType => this.boolType;

        /// <summary>
        /// Gets the generic pointer type.
        /// </summary>
        public TypeDefinition GenericPointerType => this.genericPointerType;

        /// <summary>
        /// Gets the single precision floating point type.
        /// </summary>
        public TypeDefinition SingleType => this.singleType;

        /// <summary>
        /// Gets the double precision floating point type.
        /// </summary>
        public TypeDefinition DoubleType => this.doubleType;

        /// <summary>
        /// Gets the type for an array of chars of indefinite length.
        /// </summary>
        public TypeDefinition CharArrayType => this.charArrayType;

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
        /// <returns>True if used; otherwise, false.</returns>
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

        /// <summary>
        /// Tries to find the matching constructor.
        /// </summary>
        /// <param name="objectType">The object type.</param>
        /// <param name="parameterTypes">The parameter types.</param>
        /// <param name="methodInfo">On success, receives the method info.</param>
        /// <returns>True if a match is found; otherwise, false.</returns>
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

        /// <summary>
        /// Tries to find the method and matching type.
        /// </summary>
        /// <param name="methodNameRef">The method name.</param>
        /// <param name="parameterTypes">The parameter types.</param>
        /// <param name="type">On success, receives the reference to the type.</param>
        /// <param name="methodInfo">On success, receives the method info.</param>
        /// <returns>True if a match could be found; otherwise, false.</returns>
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

        /// <summary>
        /// Gets a pointer type for the given element type.
        /// </summary>
        /// <param name="elementType">The element type.</param>
        /// <returns>The pointer type to the given element type.</returns>
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

        /// <summary>
        /// Gets an array type given an element type and count.
        /// </summary>
        /// <param name="elementType">The element type.</param>
        /// <param name="elementCount">The element count.</param>
        /// <returns>The array type definition.</returns>
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

        /// <summary>
        /// Tries to declare a type.
        /// </summary>
        /// <param name="name">The name of the type.</param>
        /// <param name="typeDef">On success, receives the type definition.</param>
        /// <returns>True if the type could be declared; otherwise, false.</returns>
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

        /// <summary>
        /// Begins a new scope.
        /// </summary>
        /// <param name="method">The method for which to create a scope.</param>
        /// <returns>The new scope.</returns>
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

        /// <summary>
        /// Gets the method type.
        /// </summary>
        /// <param name="calleeMethod">The callee method.</param>
        /// <returns>The method type for the calle method.</returns>
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

        /// <summary>
        /// Clears the uses set.
        /// </summary>
        internal void ClearUses()
        {
            this.uses.Clear();
        }
    }
}
