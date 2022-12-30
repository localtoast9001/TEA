// <copyright file="CodeGeneratorTest.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>

namespace Tea.Compiler.UnitTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Tea.Language;

    /// <summary>
    /// Unit tests for the <see cref="CodeGenerator"/> class.
    /// </summary>
    [TestClass]
    public class CodeGeneratorTest
    {
        private const string Program1 = @"
namespace Test;
uses System;

type
    FloatTest = static class
        public
            static function Add(a, b : double) : double;
    end;

function FloatTest.Add(a, b : double) : double;
begin
    Add := a + b;
end;
        ";

        /// <summary>
        /// Validates the code generated for floating point assignments.
        /// </summary>
        [TestMethod]
        public void Generate_FloatingPointAssignmentTest()
        {
            MessageLog log = new MessageLog();
            CodeGenerator target = new CodeGenerator(log);
            CompilerContext context = new CompilerContext();
            ProgramUnit program = Parse(Program1, log);
            Assert.IsTrue(target.CreateTypes(context, program));
            Assert.IsTrue(target.CreateModule(context, program, out Module actual));

            MethodImpl? methodImpl = actual.CodeSegment.FirstOrDefault(e => e.Method!.Name!.Equals("Add"));
            Assert.IsNotNull(methodImpl);
            MethodInfo meth = methodImpl!.Method!;
            Assert.AreEqual(2, meth.Parameters.Count);
            foreach (ParameterInfo paramInfo in meth.Parameters)
            {
                Assert.IsNotNull(paramInfo.Type, paramInfo.Name);
                TypeDefinition typeDef = paramInfo.Type!;
                Assert.AreEqual(sizeof(double), typeDef.Size);
            }

            string[] expected = new[]
            {
                "push ebp",
                "mov ebp, esp",
                "sub esp, 8",
                "fld qword ptr _b$[ebp]",
                "sub esp, 8",
                "fstp qword ptr [esp]",
                "fld qword ptr _a$[ebp]",
                "fld qword ptr [esp]",
                "add esp, 8",
                "faddp",
                "sub esp, 8",
                "fstp qword ptr [esp]",
                "pop eax",
                "pop edx",
                "lea ecx, dword ptr _Add$[ebp]",
                "mov dword ptr [ecx], eax",
                "add ecx, 4",
                "mov dword ptr [ecx], edx",
                "fld qword ptr _Add$[ebp]",
                "mov esp, ebp",
                "pop ebp",
                "ret",
            };

            // Assert.AreEqual(expected.Length, methodImpl!.Statements.Count);
            for (int i = 0; i < expected.Length && i < methodImpl!.Statements.Count; i++)
            {
                Assert.AreEqual(expected[i], methodImpl!.Statements[i].Instruction!.ToString());
            }
        }

        private static ProgramUnit Parse(string program, MessageLog log)
        {
            ProgramUnit? result = null;
            using (TokenReader reader = new TokenReader(new StringReader(program), "test.tea", log))
            {
                Parser parser = new Parser(log);
                Assert.IsTrue(parser.TryParse(reader, out result));
            }

            return result!;
        }
    }
}