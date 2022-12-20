//-----------------------------------------------------------------------
// <copyright file="AsmModuleWriter.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Tea.Compiler;

    /// <summary>
    /// Writer that writes a module to a ASM source file.
    /// </summary>
    public class AsmModuleWriter : ModuleWriter
    {
        private readonly StreamWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsmModuleWriter"/> class.
        /// </summary>
        /// <param name="fileName">Path to the output file.</param>
        public AsmModuleWriter(string fileName)
        {
            this.writer = new StreamWriter(fileName);
        }

        /// <inheritdoc/>
        public override bool Write(Module module)
        {
            this.writer.WriteLine(".model flat,C");
            this.writer.WriteLine();
            foreach (MethodInfo method in module.ProtoList)
            {
                this.writer.Write(method.MangledName);
                this.writer.WriteLine(" PROTO C");
            }

            foreach (string externSymbol in module.ExternList)
            {
                this.writer.Write(externSymbol);
                this.writer.WriteLine(" PROTO C");
            }

            this.writer.WriteLine();
            this.writer.WriteLine(".data");
            foreach (DataEntry dataEntry in module.DataSegment)
            {
                if (!string.IsNullOrEmpty(dataEntry.Label))
                {
                    this.writer.Write(dataEntry.Label);
                }

                for (int i = 0; i < dataEntry.Value?.Length; i++)
                {
                    this.writer.Write("\t");
                    object val = dataEntry.Value[i];
                    if (val is byte)
                    {
                        this.writer.Write("db");
                    }
                    else if (val is ushort)
                    {
                        this.writer.Write("dw");
                    }
                    else
                    {
                        this.writer.Write("dd");
                    }

                    this.writer.Write("\t");

                    this.writer.WriteLine(val);
                }

                this.writer.WriteLine();
            }

            this.writer.WriteLine();
            this.writer.WriteLine(".code");
            string? mainMethod = null;
            foreach (MethodImpl method in module.CodeSegment)
            {
                if ((method.Method?.IsStatic ?? false) &&
                   string.CompareOrdinal("Main", method.Method.Name) == 0 &&
                   method.Method.Parameters.Count == 2 &&
                   string.CompareOrdinal(method.Method?.Parameters[0].Type?.FullName, "integer") == 0 &&
                   string.CompareOrdinal(method.Method?.Parameters[1].Type?.FullName, "#0#0character") == 0)
                {
                    mainMethod = method.Method?.MangledName;
                }

                foreach (string symbol in method.Symbols.Keys)
                {
                    this.writer.WriteLine("{0}={1}", symbol, method.Symbols[symbol]);
                }

                this.writer.Write(method.Method?.MangledName);
                this.writer.Write(" PROC C");
                if (method.Method!.IsProtected || method.Method!.IsPublic)
                {
                    this.writer.Write(" EXPORT");
                }

                this.writer.WriteLine();

                foreach (var statement in method.Statements)
                {
                    if (!string.IsNullOrEmpty(statement.Label))
                    {
                        this.writer.Write("{0}:", statement.Label);
                    }

                    this.writer.Write("\t");
                    this.writer.WriteLine(statement.Instruction);
                }

                this.writer.Write(method.Method.MangledName);
                this.writer.WriteLine(" ENDP");
            }

            if (!string.IsNullOrEmpty(mainMethod))
            {
                string methodText = @"main PROC C EXPORT
    push ebp
    mov ebp,esp    
    push [ebp+12]
    push [ebp+8]
    call {0}
    add esp,8
    mov esp,ebp
    pop ebp
    ret
main ENDP";

                this.writer.WriteLine(methodText, mainMethod);
            }

            this.writer.WriteLine("END");
            return true;
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                this.writer.Dispose();
            }
        }
    }
}
