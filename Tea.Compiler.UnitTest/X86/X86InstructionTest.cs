//-----------------------------------------------------------------------
// <copyright file="X86InstructionTest.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler.UnitTest.X86
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Tea.Compiler.X86;

    /// <summary>
    /// Unit tests for the <see cref="X86Instruction"/> class.
    /// </summary>
    [TestClass]
    public class X86InstructionTest
    {
        /// <summary>
        /// Unit test for the <see cref="X86Instruction.Push(Register)"/> method.
        /// </summary>
        [TestMethod]
        public void PushTest()
        {
            X86Instruction target = X86Instruction.Push(Register.EAX);
            Assert.IsNotNull(target);
            byte[] actual = target.ToArray();
            CollectionAssert.AreEqual(new byte[] { 0x50 }, actual);
        }

        /// <summary>
        /// Unit test for the <see cref="X86Instruction.Lea"/> method.
        /// </summary>
        [TestMethod]
        public void LeaTest()
        {
            X86Instruction target = X86Instruction.Lea(Register.EAX, RM.Address("label1"));
            Assert.IsNotNull(target);
            RelocationEntry[] actualRels = target.RelocationEntries.ToArray();
            Assert.AreEqual(1, actualRels.Length);
            RelocationEntry actualRel = actualRels[0];
            Assert.AreEqual("label1", actualRel.Symbol);
        }

        /// <summary>
        /// Unit test for the <see cref="X86Instruction.ToString()"/> method.
        /// </summary>
        [TestMethod]
        public void ToStringTest()
        {
            Tuple<X86Instruction, string>[] testCases = new Tuple<X86Instruction, string>[]
            {
                new Tuple<X86Instruction, string>(X86Instruction.Add(Register.AL, 1), "add al, 1"),
                new Tuple<X86Instruction, string>(X86Instruction.Add(Register.CL, 1), "add cl, 1"),
                new Tuple<X86Instruction, string>(X86Instruction.Add(Register.EAX, 1), "add eax, 1"),
                new Tuple<X86Instruction, string>(X86Instruction.Add(Register.EDX, 2), "add edx, 2"),
                new Tuple<X86Instruction, string>(X86Instruction.Add(Register.AL, Register.CL), "add al, cl"),
                new Tuple<X86Instruction, string>(X86Instruction.Add(Register.EAX, Register.ECX), "add eax, ecx"),
                new Tuple<X86Instruction, string>(X86Instruction.And(Register.AL, (byte)1), "and al, 1"),
                new Tuple<X86Instruction, string>(X86Instruction.And(Register.CL, (byte)2), "and cl, 2"),
                new Tuple<X86Instruction, string>(X86Instruction.And(Register.EAX, 1U), "and eax, 1"),
                new Tuple<X86Instruction, string>(X86Instruction.And(Register.EDX, 2U), "and edx, 2"),
                new Tuple<X86Instruction, string>(X86Instruction.And(Register.AL, Register.CL), "and al, cl"),
                new Tuple<X86Instruction, string>(X86Instruction.And(Register.EAX, Register.ECX), "and eax, ecx"),
                new Tuple<X86Instruction, string>(X86Instruction.Call("abc"), "call abc"),
                new Tuple<X86Instruction, string>(X86Instruction.Call(RM.Address("ptr")), "call [ptr]"),
                new Tuple<X86Instruction, string>(X86Instruction.Call(RM.Address(Register.EDX, 4, "label1")), "call label1[edx]"),
                new Tuple<X86Instruction, string>(X86Instruction.Cld(), "cld"),
                new Tuple<X86Instruction, string>(X86Instruction.Cmp(Register.AL, Register.CL), "cmp al, cl"),
                new Tuple<X86Instruction, string>(X86Instruction.Cmp(Register.EAX, Register.ECX), "cmp eax, ecx"),
                new Tuple<X86Instruction, string>(X86Instruction.Fnstsw(), "fnstsw ax"),
                new Tuple<X86Instruction, string>(X86Instruction.Imul(RM.FromRegister(Register.CL)), "imul al, cl"),
                new Tuple<X86Instruction, string>(X86Instruction.Imul(RM.FromRegister(Register.CX)), "imul ax, cx"),
                new Tuple<X86Instruction, string>(X86Instruction.Imul(RM.FromRegister(Register.ECX)), "imul eax, ecx"),
                new Tuple<X86Instruction, string>(X86Instruction.Imul(Register.ECX, RM.Address(Register.EDI)), "imul ecx, [edi]"),
                new Tuple<X86Instruction, string>(X86Instruction.Jmp("label1"), "jmp label1"),
                new Tuple<X86Instruction, string>(X86Instruction.Jz("label1"), "jz label1"),
                new Tuple<X86Instruction, string>(X86Instruction.Lea(Register.ECX, RM.Address(Register.EAX)), "lea ecx, [eax]"),
                new Tuple<X86Instruction, string>(X86Instruction.Lea(Register.EAX, RM.Address("label1")), "lea eax, [label1]"),
                new Tuple<X86Instruction, string>(X86Instruction.Lea(Register.EAX, RM.Address(Register.EBP, -4, "label1")), "lea eax, label1[ebp]"),
                new Tuple<X86Instruction, string>(X86Instruction.Mov(Register.AL, Register.CL), "mov al, cl"),
                new Tuple<X86Instruction, string>(X86Instruction.Mov(Register.EDI, Register.EAX), "mov edi, eax"),
                new Tuple<X86Instruction, string>(X86Instruction.Mov(Register.EAX, 1), "mov eax, 1"),
                new Tuple<X86Instruction, string>(X86Instruction.Mov(Register.EAX, 0, "funcptr"), "mov eax, funcptr"),
                new Tuple<X86Instruction, string>(X86Instruction.Mov(Register.AL, RM.Address(Register.ECX, sizeof(byte))), "mov al, byte ptr [ecx]"),
                new Tuple<X86Instruction, string>(X86Instruction.Mov(Register.EAX, RM.Address(Register.ECX)), "mov eax, [ecx]"),
                new Tuple<X86Instruction, string>(X86Instruction.Mov(Register.AX, RM.Address(Register.ECX, sizeof(ushort))), "mov ax, word ptr [ecx]"),
                new Tuple<X86Instruction, string>(X86Instruction.Mov(Register.EAX, RM.Address(Register.ECX, 4, "_x$")), "mov eax, _x$[ecx]"),
                new Tuple<X86Instruction, string>(X86Instruction.Mov(Register.AX, RM.Address(Register.ECX, 4, "_x$", sizeof(ushort))), "mov ax, word ptr _x$[ecx]"),
                new Tuple<X86Instruction, string>(X86Instruction.Mov(RM.Address(Register.EBP, 4, "_x$"), Register.EAX), "mov _x$[ebp], eax"),
                new Tuple<X86Instruction, string>(X86Instruction.Mov(RM.Address(Register.EBP, 4, "_x$", sizeof(byte)), Register.AL), "mov byte ptr _x$[ebp], al"),
                new Tuple<X86Instruction, string>(X86Instruction.Mov(RM.Address(Register.EBP, 4, "_x$", sizeof(ushort)), Register.AX), "mov word ptr _x$[ebp], ax"),
                new Tuple<X86Instruction, string>(X86Instruction.Movsb().Rep(), "rep movsb"),
                new Tuple<X86Instruction, string>(X86Instruction.Pop(Register.EAX), "pop eax"),
                new Tuple<X86Instruction, string>(X86Instruction.Push(Register.EAX), "push eax"),
                new Tuple<X86Instruction, string>(X86Instruction.Sahf(), "sahf"),
                new Tuple<X86Instruction, string>(X86Instruction.Seta(RM.FromRegister(Register.AL)), "seta al"),
                new Tuple<X86Instruction, string>(X86Instruction.Setae(RM.FromRegister(Register.AL)), "setae al"),
                new Tuple<X86Instruction, string>(X86Instruction.Setb(RM.FromRegister(Register.AL)), "setb al"),
                new Tuple<X86Instruction, string>(X86Instruction.Setbe(RM.FromRegister(Register.AL)), "setbe al"),
                new Tuple<X86Instruction, string>(X86Instruction.Sete(RM.FromRegister(Register.AL)), "sete al"),
                new Tuple<X86Instruction, string>(X86Instruction.Setg(RM.FromRegister(Register.AL)), "setg al"),
                new Tuple<X86Instruction, string>(X86Instruction.Setge(RM.FromRegister(Register.AL)), "setge al"),
                new Tuple<X86Instruction, string>(X86Instruction.Setl(RM.FromRegister(Register.AL)), "setl al"),
                new Tuple<X86Instruction, string>(X86Instruction.Setle(RM.FromRegister(Register.AL)), "setle al"),
                new Tuple<X86Instruction, string>(X86Instruction.Setne(RM.FromRegister(Register.AL)), "setne al"),
                new Tuple<X86Instruction, string>(X86Instruction.Shl(RM.FromRegister(Register.AL)), "shl al, 1"),
                new Tuple<X86Instruction, string>(X86Instruction.Shl(RM.FromRegister(Register.AX)), "shl ax, 1"),
                new Tuple<X86Instruction, string>(X86Instruction.Shl(RM.FromRegister(Register.EAX)), "shl eax, 1"),
                new Tuple<X86Instruction, string>(X86Instruction.Shl(RM.FromRegister(Register.AL), 2), "shl al, 2"),
                new Tuple<X86Instruction, string>(X86Instruction.Shl(RM.FromRegister(Register.AX), 2), "shl ax, 2"),
                new Tuple<X86Instruction, string>(X86Instruction.Shl(RM.FromRegister(Register.EAX), 2), "shl eax, 2"),
                new Tuple<X86Instruction, string>(X86Instruction.Sub(Register.AL, (byte)1), "sub al, 1"),
                new Tuple<X86Instruction, string>(X86Instruction.Sub(Register.CL, (byte)2), "sub cl, 2"),
                new Tuple<X86Instruction, string>(X86Instruction.Sub(Register.EAX, 1U), "sub eax, 1"),
                new Tuple<X86Instruction, string>(X86Instruction.Sub(Register.EDX, 2U), "sub edx, 2"),
                new Tuple<X86Instruction, string>(X86Instruction.Sub(Register.AL, Register.CL), "sub al, cl"),
                new Tuple<X86Instruction, string>(X86Instruction.Sub(Register.EAX, Register.ECX), "sub eax, ecx"),
                new Tuple<X86Instruction, string>(X86Instruction.Test(Register.AL, Register.CL), "test al, cl"),
                new Tuple<X86Instruction, string>(X86Instruction.Test(Register.EAX, Register.ECX), "test eax, ecx"),
                new Tuple<X86Instruction, string>(X86Instruction.Xor(Register.AL, Register.CL), "xor al, cl"),
                new Tuple<X86Instruction, string>(X86Instruction.Xor(Register.EAX, Register.ECX), "xor eax, ecx"),
            };

            foreach (var testCase in testCases)
            {
                string actual = string.Empty;
                try
                {
                    actual = testCase.Item1.ToString();
                }
                catch (Exception ex)
                {
                    Assert.Fail($"Expected [{testCase.Item2}]. Actual [{ex.ToString()}].");
                }

                Assert.AreEqual(testCase.Item2, actual);
            }
        }
    }
}