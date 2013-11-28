using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEAC
{
    internal class CompilerContext
    {
        private List<string> uses = new List<string>();
        private List<string> includes = new List<string>();
        private Dictionary<string, TypeDefinition> types = new Dictionary<string, TypeDefinition>();

        public CompilerContext()
        {
            TypeDefinition intType = new TypeDefinition
            {
                FullName = "integer",
                Size = 4,
                SpecialMangledName = "i"
            };
            
            TypeDefinition charType = new TypeDefinition
            {
                FullName = "character",
                Size = 2,
                SpecialMangledName = "c"
            };

            TypeDefinition boolType = new TypeDefinition
            {
                FullName = "boolean",
                Size = 1,
                SpecialMangledName = "f"
            };

            TypeDefinition byteType = new TypeDefinition
            {
                FullName = "byte",
                Size = 1,
                SpecialMangledName = "b"
            };

            TypeDefinition singleType = new TypeDefinition
            {
                FullName = "single",
                Size = 4,
                SpecialMangledName = "s",
                IsFloatingPoint = true
            };
            
            TypeDefinition doubleType = new TypeDefinition
            {
                FullName = "double",
                Size = 8,
                SpecialMangledName = "d",
                IsFloatingPoint = true
            };

            TypeDefinition genericPointerType = new TypeDefinition
            {
                FullName = "^",
                Size = 4,
                SpecialMangledName = "p",
                IsPointer = true
            };

            types.Add(intType.FullName, intType);
            types.Add(charType.FullName, charType);
            types.Add(singleType.FullName, singleType);
            types.Add(doubleType.FullName, doubleType);
            types.Add(boolType.FullName, boolType);
            types.Add(byteType.FullName, byteType);
            types.Add(genericPointerType.FullName, genericPointerType);
        }

        public string Namespace { get; set; }
        
        public IEnumerable<string> Uses { get { return this.uses; } }

        public IEnumerable<string> Includes { get { return this.includes; } }

        public void AddIncludePaths(IEnumerable<string> paths)
        {
            this.includes.AddRange(paths);
        }

        public void AddUses(string ns)
        {
            uses.Add(ns);
        }

        public bool TryFindTypeByName(string nameRef, out TypeDefinition type)
        {
            if (types.TryGetValue(nameRef, out type))
            {
                return true;
            }

            if (types.TryGetValue(this.Namespace + "." + nameRef, out type))
            {
                return true;
            }

            foreach (string ns in this.uses)
            {
                if (types.TryGetValue(ns + "." + nameRef, out type))
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
            out MethodInfo methodInfo)
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
                    if (string.CompareOrdinal(constructor.Parameters[i].Type.FullName, parameterTypes[i].FullName) != 0)
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
            out TypeDefinition type, 
            out MethodInfo methodInfo)
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
            if (!TryFindTypeByName(typeName, out type))
            {
                return false;
            }

            foreach (var test in type.Methods)
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
                        if (string.CompareOrdinal(test.Parameters[i].Type.FullName, parameterTypes[i].FullName) != 0)
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
            TypeDefinition result = null;
            string fullName = "^" + elementType.FullName;
            if (!this.types.TryGetValue(fullName, out result))
            {
                result = new TypeDefinition
                {
                    FullName = fullName, IsPointer = true, Size = 4, InnerType = elementType
                };

                this.types.Add(fullName, result);
            }

            return result;
        }

        public TypeDefinition GetArrayType(TypeDefinition elementType, int elementCount)
        {
            TypeDefinition result = null;
            string fullName = "#" + elementCount.ToString() + elementType.FullName;
            if (!this.types.TryGetValue(fullName, out result))
            {
                result = new TypeDefinition
                {
                    FullName = fullName,
                    IsArray = true,
                    Size = elementCount > 0 ? elementType.Size * elementCount : 4,
                    ArrayElementCount = elementCount,
                    InnerType = elementType
                };

                this.types.Add(fullName, result);
            }

            return result;
        }

        public bool TryDeclareType(string name, out TypeDefinition typeDef)
        {
            string fullName = this.Namespace + "." + name;
            TypeDefinition existing = null;
            if (this.types.TryGetValue(fullName, out existing))
            {
                typeDef = existing;
                return existing.Size == 0;
            }

            typeDef = new TypeDefinition
            {
                FullName = fullName
            };

            this.types.Add(fullName, typeDef);
            return true;
        }

        public Scope BeginScope(MethodInfo method)
        {
            Scope scope = new Scope();
            if (method.ReturnType != null)
            {
                LocalVariable returnVar = scope.DefineLocalVariable(method.Name, method.ReturnType);
                scope.ReturnVariable = returnVar;
            }

            if (!method.IsStatic)
            {
                scope.DefineParameter("this", this.GetPointerType(method.Type));
            }

            foreach (ParameterInfo parameterInfo in method.Parameters)
            {
                scope.DefineParameter(parameterInfo.Name, parameterInfo.Type);
            }

            return scope;
        }

        internal void ClearUses()
        {
            this.uses.Clear();
        }
    }
}
