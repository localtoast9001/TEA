using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEAC
{
    internal class Module
    {
        private List<DataEntry> dataSeg = new List<DataEntry>();
        private List<MethodInfo> protoList = new List<MethodInfo>();
        private List<string> externList = new List<string>();
        private List<MethodImpl> codeSeg = new List<MethodImpl>();
        private int literalStringIndex;
        private int literalDoubleIndex;
        private int jumpLabelIndex;

        public List<DataEntry> DataSegment { get { return this.dataSeg; } }
        public List<MethodImpl> CodeSegment { get { return this.codeSeg; } }
        public List<MethodInfo> ProtoList { get { return this.protoList; } }
        public List<string> ExternList { get { return this.externList; } }

        public void AddProto(MethodInfo proto)
        {
            string mangledName = proto.MangledName;
            foreach (var existing in this.protoList)
            {
                if (string.CompareOrdinal(existing.MangledName, mangledName) == 0)
                {
                    return;
                }
            }

            this.protoList.Add(proto);
        }

        public void AddExtern(string externSymbol)
        {
            foreach (var existing in this.externList)
            {
                if (string.CompareOrdinal(externSymbol, existing) == 0)
                {
                    return;
                }
            }

            this.externList.Add(externSymbol);
        }

        public string GetNextJumpLabel()
        {
            return "$Label_" + (this.jumpLabelIndex++).ToString();
        }

        public string DefineConstant(double value)
        {
            byte[] rawData = BitConverter.GetBytes(value);
            string symbolName = "$Double_" + this.literalDoubleIndex.ToString();
            this.literalDoubleIndex++;
            object[] scrub = new object[rawData.Length];
            for (int i = 0; i < scrub.Length; i++)
            {
                scrub[i] = rawData[i];
            }

            this.DataSegment.Add(new DataEntry { Label = symbolName, Value = scrub });
            return symbolName;
        }

        public string DefineLiteralString(string value)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(ms, Encoding.Unicode))
            {
                writer.Write(value);
                writer.Write('\0');
            }

            byte[] rawData = ms.ToArray();
            object[] scrub = new object[rawData.Length - 2]; // strip out byte order
            for (int i = 0; i < scrub.Length; i++)
            {
                scrub[i] = rawData[i+2];
            }

            string symbolName = "$String_" + this.literalStringIndex.ToString();
            this.literalStringIndex++;
            this.DataSegment.Add(new DataEntry { Label = symbolName, Value = scrub });
            return symbolName;
        }

        public void DefineInterfaceTables(TypeDefinition type)
        {
            IEnumerable<KeyValuePair<TypeDefinition, int>> allInterfaces = type.GetAllInterfaces();
            foreach (var pair in allInterfaces)
            {
                string label = "$Vtbl_" + pair.Key.MangledName + "_" + type.MangledName;
                if (this.DataSegment.Any(e => string.CompareOrdinal(e.Label, label) == 0))
                {
                    continue;
                }

                List<string> entries = new List<string>();
                Stack<TypeDefinition> typeStack = new Stack<TypeDefinition>();
                TypeDefinition? t = pair.Key;
                while (t != null)
                {
                    typeStack.Push(t);
                    t = t.BaseClass;
                }

                while (typeStack.Count > 0)
                {
                    t = typeStack.Pop();
                    foreach (MethodInfo m in t.Methods)
                    {
                        MethodInfo? implMethod = type.FindMethod(
                            m.Name!,
                            m.Parameters.Select(e => e.Type!).ToList());
                        if (implMethod == null)
                        {
                            throw new NotSupportedException();
                        }

                        if (entries.Count <= m.VTableIndex)
                        {
                            for (int i = entries.Count; i <= implMethod.VTableIndex; i++)
                            {
                                entries.Add("0");
                            }
                        }

                        MethodInfo jumpMethod = new MethodInfo(type);
                        jumpMethod.Parameters.AddRange(m.Parameters);
                        jumpMethod.Name = t.FullName!.Replace('.', '_') + "_" + m.Name;

                        this.AddProto(implMethod);
                        this.AddProto(jumpMethod);
                        entries[m.VTableIndex] = jumpMethod.MangledName;

                        if (!this.CodeSegment.Any(e => string.CompareOrdinal(e.Method!.MangledName, jumpMethod.MangledName) == 0))
                        {
                            MethodImpl jumpMethodImpl = new MethodImpl(this);
                            jumpMethodImpl.Method = jumpMethod;
                            this.CodeSegment.Add(jumpMethodImpl);
                            jumpMethodImpl.Symbols.Add(new KeyValuePair<string, int>("_this$", 4));
                            jumpMethodImpl.Statements.Add(new AsmStatement { Instruction = "mov eax,_this$[esp]" });
                            jumpMethodImpl.Statements.Add(new AsmStatement { Instruction = string.Format("sub eax,{0}", pair.Value) });
                            jumpMethodImpl.Statements.Add(new AsmStatement { Instruction = "mov _this$[esp],eax" });
                            jumpMethodImpl.Statements.Add(new AsmStatement { Instruction = "jmp " + implMethod.MangledName });
                        }
                    }
                }

                this.DataSegment.Add(new DataEntry { Label = label, Value = entries.ToArray() });
            }
        }

        public void DefineVTable(TypeDefinition type)
        {
            string label = "$Vtbl_" + type.MangledName;
            if (this.DataSegment.Any(e => string.CompareOrdinal(e.Label, label) == 0))
            {
                return;
            }

            string[] entries = new string[0];
            Stack<TypeDefinition> typeStack = new Stack<TypeDefinition>();
            TypeDefinition? t = type;
            while (t != null)
            {
                typeStack.Push(t!);
                t = t.BaseClass;
            }

            while (typeStack.Count > 0)
            {
                t = typeStack.Pop();
                foreach (MethodInfo m in t!.Methods)
                {
                    if (m.IsVirtual)
                    {
                        if (entries.Length <= m.VTableIndex)
                        {
                            string[] newEntries = new string[m.VTableIndex + 1];
                            Array.Copy(entries, newEntries, entries.Length);
                            entries = newEntries;
                        }

                        if (!m.IsAbstract)
                        {
                            this.AddProto(m);
                            entries[m.VTableIndex] = m.MangledName;
                        }
                        else
                        {
                            entries[m.VTableIndex] = "0";
                        }
                    }
                }
            }

            this.DataSegment.Add(new DataEntry { Label = label, Value = entries });
        }
    }

    internal class DataEntry
    {
        public string? Label { get; set; }
        public object[]? Value { get; set; }
    }

    internal class AsmStatement
    {
        public string? Label { get; set; }
        public string? Instruction { get; set; }
    }

    internal class MethodImpl
    {
        private List<AsmStatement> statements = new List<AsmStatement>();
        private Dictionary<string, int> symbols = new Dictionary<string, int>();

        public MethodImpl(Module module)
        {
            this.Module = module;
        }

        public List<AsmStatement> Statements { get { return this.statements; } }
        public IDictionary<string, int> Symbols { get { return this.symbols; } }
        public MethodInfo? Method { get; set; }
        public Module? Module { get; private set; }
    }
}
