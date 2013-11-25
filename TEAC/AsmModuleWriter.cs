using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEAC
{
    class AsmModuleWriter : ModuleWriter
    {
        private StreamWriter writer;

        public AsmModuleWriter(string fileName)
        {
            writer = new StreamWriter(fileName);
        }

        public override bool Write(Module module)
        {
            writer.WriteLine(".model flat,C");
            writer.WriteLine();
            foreach (MethodInfo method in module.ProtoList)
            {
                writer.Write(method.MangledName);
                writer.WriteLine(" PROTO C");
            }

            foreach (string externSymbol in module.ExternList)
            {
                writer.Write(externSymbol);
                writer.WriteLine(" PROTO C");
            }

            writer.WriteLine();
            writer.WriteLine(".data");
            foreach (DataEntry dataEntry in module.DataSegment)
            {
                if (!string.IsNullOrEmpty(dataEntry.Label))
                {
                    writer.Write(dataEntry.Label);
                }

                writer.Write("\t");
                object val = dataEntry.Value[0];
                if (val is byte)
                {
                    writer.Write("db");
                }
                else if (val is ushort)
                {
                    writer.Write("dw");
                }
                else
                {
                    writer.Write("dd");
                }

                writer.Write("\t");
                for (int i = 0; i < dataEntry.Value.Length; i++)
                {
                    if (i > 0)
                    {
                        writer.Write(",");
                    }

                    writer.Write(dataEntry.Value[i]);
                }

                writer.WriteLine();
            }

            writer.WriteLine();
            writer.WriteLine(".code");
            string mainMethod = null;
            foreach (MethodImpl method in module.CodeSegment)
            {
                if (method.Method.IsStatic &&
                   string.CompareOrdinal("Main", method.Method.Name) == 0 &&
                   method.Method.Parameters.Count == 2 &&
                   string.CompareOrdinal(method.Method.Parameters[0].Type.FullName, "integer") == 0 &&
                   string.CompareOrdinal(method.Method.Parameters[1].Type.FullName, "#0#0character") == 0)
                {
                    mainMethod = method.Method.MangledName;
                }

                writer.Write(method.Method.MangledName);
                writer.Write(" PROC C");
                if (method.Method.IsProtected || method.Method.IsPublic)
                {
                    writer.Write(" EXPORT");
                }

                writer.WriteLine();

                foreach (var statement in method.Statements)
                {
                    if (!string.IsNullOrEmpty(statement.Label))
                    {
                        writer.Write("{0}:", statement.Label);
                    }

                    writer.Write("\t");
                    writer.WriteLine(statement.Instruction);
                }

                writer.Write(method.Method.MangledName);
                writer.WriteLine(" ENDP");
            }

            if (!string.IsNullOrEmpty(mainMethod))
            {
                string methodText = @"wmain PROC C EXPORT
	push ebp
	mov ebp,esp    
    push [ebp+12]
    push [ebp+8]
    call {0}
    add esp,8
	mov esp,ebp
	pop ebp
    ret
wmain ENDP";

                writer.WriteLine(methodText, mainMethod);
            }

            writer.WriteLine("END");
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            writer.Dispose();
        }
    }
}
