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

    internal class Scope
    {
        private Dictionary<string, SymbolEntry> symbols = new Dictionary<string, SymbolEntry>();
        private int parameterOffset = 8;
        private int localOffset;
        private int tempVariableIndex;

        public int LocalOffset { get { return this.localOffset; } }

        public int ParameterOffset { get { return this.ParameterOffset; } }

        public LocalVariable ReturnVariable { get; set; }

        public LocalVariable DefineTempVariable(TypeDefinition type)
        {
            string name = "$temp" + (this.tempVariableIndex++).ToString();
            this.DefineLocalVariable(name, type);
            return (LocalVariable)symbols[name];
        }

        public LocalVariable DefineLocalVariable(string name, TypeDefinition type)
        {
            int alignFix = localOffset % type.Size;
            if (alignFix > 0)
            {
                localOffset += type.Size - alignFix;
            }

            localOffset += type.Size;
            LocalVariable l = new LocalVariable(name)
            {
                Offset = this.localOffset,
                Type = type
            };

            symbols.Add(name, l);
            return l;
        }

        public void DefineParameter(string name, TypeDefinition type)
        {
            ParameterVariable p = new ParameterVariable(name)
            {
                Offset = this.parameterOffset,
                Type = type
            };

            parameterOffset += p.Type.Size;
            symbols.Add(name, p);
        }

        public bool TryLookup(string symbol, out SymbolEntry value)
        {
            return this.symbols.TryGetValue(symbol, out value);
        }

        public void SaveSymbols(IDictionary<string, int> dictionary)
        {
            foreach (string symbolName in this.symbols.Keys)
            {
                var symbol = symbols[symbolName];
                LocalVariable localVar = symbol as LocalVariable;
                if (localVar != null)
                {
                    dictionary.Add("_" + symbolName + "$", -localVar.Offset);
                }
                else
                {
                    ParameterVariable parVar = (ParameterVariable)symbol;
                    dictionary.Add("_" + symbolName + "$", parVar.Offset);
                }
            }
        }
    }

    abstract class SymbolEntry
    {
        protected SymbolEntry(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }
        public TypeDefinition Type { get; set; }
    }

    class LocalVariable : SymbolEntry
    {
        public LocalVariable(string name)
            : base(name)
        {
        }

        public int Offset { get; set; }
    }

    class ParameterVariable : SymbolEntry
    {
        public ParameterVariable(string name)
            : base(name)
        {
        }

        public int Offset { get; set; }
    }

    internal class TypeDefinition
    {
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
    }

    internal class MethodInfo
    {
        private List<ParameterInfo> parameters = new List<ParameterInfo>();

        public MethodInfo(TypeDefinition type)
        {
            this.Type = type;
        }

        public TypeDefinition Type { get; private set; }

        public List<ParameterInfo> Parameters
        {
            get { return this.parameters; }
        }

        public string Name { get; set; }
        public bool IsStatic { get; set; }
        public bool IsVirtual { get; set; }
        public bool IsPublic { get; set; }
        public bool IsProtected { get; set; }
        public TypeDefinition ReturnType { get; set; }

        public string MangledName
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(this.Type.MangledName);
                sb.AppendFormat("{0}{1}E", this.Name.Length, this.Name);
                if (this.Parameters.Count > 0)
                {
                    foreach (var parameter in this.Parameters)
                    {
                        sb.Append(parameter.Type.MangledName);
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

    internal class FieldInfo
    {
        public string Name { get; set; }
        public TypeDefinition Type { get; set; }
        public bool IsStatic { get; set; }
        public bool IsPublic { get; set; }
        public bool IsProtected { get; set; }
        public int Offset { get; set; }
    }

    internal class ParameterInfo
    {
        public string Name { get; set; }
        public TypeDefinition Type { get; set; }
    }
}
