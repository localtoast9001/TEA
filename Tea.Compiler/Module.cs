//-----------------------------------------------------------------------
// <copyright file="Module.cs" company="Jon Rowlett">
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
    /// Module that is output from a code generator.
    /// </summary>
    public class Module
    {
        private readonly List<DataEntry> dataSeg = new List<DataEntry>();
        private readonly List<MethodInfo> protoList = new List<MethodInfo>();
        private readonly List<string> externList = new List<string>();
        private readonly List<MethodImpl> codeSeg = new List<MethodImpl>();
        private int literalStringIndex;
        private int literalDoubleIndex;
        private int jumpLabelIndex;

        /// <summary>
        /// Gets or sets the source file name.
        /// </summary>
        public string? SourceFileName { get; set; }

        /// <summary>
        /// Gets the data segment.
        /// </summary>
        public List<DataEntry> DataSegment
        {
            get { return this.dataSeg; }
        }

        /// <summary>
        /// Gets the code segment.
        /// </summary>
        public List<MethodImpl> CodeSegment
        {
            get { return this.codeSeg; }
        }

        /// <summary>
        /// Gets the prototype list.
        /// </summary>
        public List<MethodInfo> ProtoList
        {
            get { return this.protoList; }
        }

        /// <summary>
        /// Gets the list of externs.
        /// </summary>
        public List<string> ExternList
        {
            get { return this.externList; }
        }

        /// <summary>
        /// Adds a method prototype.
        /// </summary>
        /// <param name="proto">The method to add.</param>
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

        /// <summary>
        /// Adds an extern symbol.
        /// </summary>
        /// <param name="externSymbol">The symbol to add.</param>
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

        /// <summary>
        /// Gets the next jump symbol.
        /// </summary>
        /// <returns>The next jump signal.</returns>
        public string GetNextJumpLabel()
        {
            return "$Label_" + (this.jumpLabelIndex++).ToString();
        }

        /// <summary>
        /// Defines a constant.
        /// </summary>
        /// <param name="value">The constant value.</param>
        /// <returns>The new symbol name.</returns>
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

        /// <summary>
        /// Defines a literal string.
        /// </summary>
        /// <param name="value">The literal value.</param>
        /// <returns>The symbol name.</returns>
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
                scrub[i] = rawData[i + 2];
            }

            string symbolName = "$String_" + this.literalStringIndex.ToString();
            this.literalStringIndex++;
            this.DataSegment.Add(new DataEntry { Label = symbolName, Value = scrub });
            return symbolName;
        }

        /// <summary>
        /// Defines interfaces tables from the given type.
        /// </summary>
        /// <param name="type">The type for which to define the interface tables.</param>
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
                            jumpMethodImpl.Statements.Add(new AsmStatement { Instruction = new UnknownInstruction("mov eax,_this$[esp]") });
                            jumpMethodImpl.Statements.Add(new AsmStatement { Instruction = new UnknownInstruction(string.Format("sub eax,{0}", pair.Value)) });
                            jumpMethodImpl.Statements.Add(new AsmStatement { Instruction = new UnknownInstruction("mov _this$[esp],eax") });
                            jumpMethodImpl.Statements.Add(new AsmStatement { Instruction = new UnknownInstruction("jmp " + implMethod.MangledName) });
                        }
                    }
                }

                this.DataSegment.Add(new DataEntry { Label = label, Value = entries.ToArray() });
            }
        }

        /// <summary>
        /// Defines the vtable for the given type.
        /// </summary>
        /// <param name="type">The type for which to define the vtable.</param>
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
}
