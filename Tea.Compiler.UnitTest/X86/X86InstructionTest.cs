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
        /// Unit test for the <see cref="X86Instruction.Imul(Register, RM, int)"/> method.
        /// </summary>
        [TestMethod]
        public void ImulImmediateInt()
        {
            X86Instruction target = X86Instruction.Imul(Register.AX, RM.FromRegister(Register.CX), -2000);
            Assert.IsNotNull(target);
            byte[] actual = target.ToArray();
            Assert.AreEqual(5, actual.Length);
            Assert.AreEqual(0x66, actual[0]);
            Assert.AreEqual(0x69, actual[1]);
            Assert.AreEqual(0xc1, actual[2]);
            Assert.AreEqual(0x30, actual[3]);
            Assert.AreEqual(0xf8, actual[4]);
        }

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
                new Tuple<X86Instruction, string>(X86Instruction.Call(RM.Address("ptr")), "call dword ptr [ptr]"),
                new Tuple<X86Instruction, string>(X86Instruction.Call(RM.Address(Register.EDX, 4, "label1")), "call dword ptr label1[edx]"),
                new Tuple<X86Instruction, string>(X86Instruction.Cld(), "cld"),
                new Tuple<X86Instruction, string>(X86Instruction.Cmp(Register.AL, Register.CL), "cmp al, cl"),
                new Tuple<X86Instruction, string>(X86Instruction.Cmp(Register.EAX, Register.ECX), "cmp eax, ecx"),
                new Tuple<X86Instruction, string>(X86Instruction.Faddp(), "faddp"),
                new Tuple<X86Instruction, string>(X86Instruction.Fcompp(), "fcompp"),
                new Tuple<X86Instruction, string>(X86Instruction.Fdivp(), "fdivp"),
                new Tuple<X86Instruction, string>(X86Instruction.Fild(RM.Address(Register.ESP)), "fild dword ptr [esp]"),
                new Tuple<X86Instruction, string>(X86Instruction.Fild(RM.Address(Register.ESP, sizeof(ushort))), "fild word ptr [esp]"),
                new Tuple<X86Instruction, string>(X86Instruction.Fild(RM.Address(Register.ESP, sizeof(ulong))), "fild qword ptr [esp]"),
                new Tuple<X86Instruction, string>(X86Instruction.Fld(RM.Address(Register.EAX)), "fld dword ptr [eax]"),
                new Tuple<X86Instruction, string>(X86Instruction.Fld(RM.Address(Register.EAX, sizeof(double))), "fld qword ptr [eax]"),
                new Tuple<X86Instruction, string>(X86Instruction.Fld(RM.Address(Register.EAX, RM.TWordSize)), "fld tword ptr [eax]"),
                new Tuple<X86Instruction, string>(X86Instruction.Fld1(), "fld1"),
                new Tuple<X86Instruction, string>(X86Instruction.Fldz(), "fldz"),
                new Tuple<X86Instruction, string>(X86Instruction.Fmulp(), "fmulp"),
                new Tuple<X86Instruction, string>(X86Instruction.Fsubp(), "fsubp"),
                new Tuple<X86Instruction, string>(X86Instruction.Fstp(RM.Address(Register.EAX)), "fstp dword ptr [eax]"),
                new Tuple<X86Instruction, string>(X86Instruction.Fstp(RM.Address(Register.EAX, sizeof(double))), "fstp qword ptr [eax]"),
                new Tuple<X86Instruction, string>(X86Instruction.Fstp(RM.Address(Register.EAX, RM.TWordSize)), "fstp tword ptr [eax]"),
                new Tuple<X86Instruction, string>(X86Instruction.Fchs(), "fchs"),
                new Tuple<X86Instruction, string>(X86Instruction.Fnstsw(), "fnstsw ax"),
                new Tuple<X86Instruction, string>(X86Instruction.Idiv(RM.FromRegister(Register.CL)), "idiv cl"),
                new Tuple<X86Instruction, string>(X86Instruction.Idiv(RM.FromRegister(Register.CX)), "idiv cx"),
                new Tuple<X86Instruction, string>(X86Instruction.Idiv(RM.FromRegister(Register.ECX)), "idiv ecx"),
                new Tuple<X86Instruction, string>(X86Instruction.Imul(RM.FromRegister(Register.CL)), "imul al, cl"),
                new Tuple<X86Instruction, string>(X86Instruction.Imul(RM.FromRegister(Register.CX)), "imul ax, cx"),
                new Tuple<X86Instruction, string>(X86Instruction.Imul(RM.FromRegister(Register.ECX)), "imul eax, ecx"),
                new Tuple<X86Instruction, string>(X86Instruction.Imul(Register.ECX, RM.Address(Register.EDI)), "imul ecx, dword ptr [edi]"),
                new Tuple<X86Instruction, string>(X86Instruction.Imul(Register.AX, RM.FromRegister(Register.CX), 5), "imul ax, cx, 5"),
                new Tuple<X86Instruction, string>(X86Instruction.Imul(Register.EAX, RM.FromRegister(Register.ECX), 50), "imul eax, ecx, 50"),
                new Tuple<X86Instruction, string>(X86Instruction.Imul(Register.AX, RM.FromRegister(Register.CX), -2000), "imul ax, cx, -2000"),
                new Tuple<X86Instruction, string>(X86Instruction.Imul(Register.EAX, RM.FromRegister(Register.ECX), -2000), "imul eax, ecx, -2000"),
                new Tuple<X86Instruction, string>(X86Instruction.Jmp("label1"), "jmp label1"),
                new Tuple<X86Instruction, string>(X86Instruction.Jz("label1"), "jz label1"),
                new Tuple<X86Instruction, string>(X86Instruction.Lea(Register.ECX, RM.Address(Register.EAX)), "lea ecx, dword ptr [eax]"),
                new Tuple<X86Instruction, string>(X86Instruction.Lea(Register.EAX, RM.Address("label1")), "lea eax, dword ptr [label1]"),
                new Tuple<X86Instruction, string>(X86Instruction.Lea(Register.EAX, RM.Address(Register.EBP, -4, "label1")), "lea eax, dword ptr label1[ebp]"),
                new Tuple<X86Instruction, string>(X86Instruction.Mov(Register.AL, Register.CL), "mov al, cl"),
                new Tuple<X86Instruction, string>(X86Instruction.Mov(Register.EDI, Register.EAX), "mov edi, eax"),
                new Tuple<X86Instruction, string>(X86Instruction.Mov(Register.EAX, 1), "mov eax, 1"),
                new Tuple<X86Instruction, string>(X86Instruction.Mov(Register.EAX, 0, "funcptr"), "mov eax, funcptr"),
                new Tuple<X86Instruction, string>(X86Instruction.Mov(Register.AL, RM.Address(Register.ECX, sizeof(byte))), "mov al, byte ptr [ecx]"),
                new Tuple<X86Instruction, string>(X86Instruction.Mov(Register.EAX, RM.Address(Register.ECX)), "mov eax, dword ptr [ecx]"),
                new Tuple<X86Instruction, string>(X86Instruction.Mov(Register.AX, RM.Address(Register.ECX, sizeof(ushort))), "mov ax, word ptr [ecx]"),
                new Tuple<X86Instruction, string>(X86Instruction.Mov(Register.EAX, RM.Address(Register.ECX, 4, "_x$")), "mov eax, dword ptr _x$[ecx]"),
                new Tuple<X86Instruction, string>(X86Instruction.Mov(Register.AX, RM.Address(Register.ECX, 4, "_x$", sizeof(ushort))), "mov ax, word ptr _x$[ecx]"),
                new Tuple<X86Instruction, string>(X86Instruction.Mov(RM.Address(Register.EBP, 4, "_x$"), Register.EAX), "mov dword ptr _x$[ebp], eax"),
                new Tuple<X86Instruction, string>(X86Instruction.Mov(RM.Address(Register.EBP, 4, "_x$", sizeof(byte)), Register.AL), "mov byte ptr _x$[ebp], al"),
                new Tuple<X86Instruction, string>(X86Instruction.Mov(RM.Address(Register.EBP, 4, "_x$", sizeof(ushort)), Register.AX), "mov word ptr _x$[ebp], ax"),
                new Tuple<X86Instruction, string>(X86Instruction.Movsb().Rep(), "rep movsb"),
                new Tuple<X86Instruction, string>(X86Instruction.Or(Register.AL, Register.DL), "or al, dl"),
                new Tuple<X86Instruction, string>(X86Instruction.Or(Register.AX, Register.DX), "or ax, dx"),
                new Tuple<X86Instruction, string>(X86Instruction.Or(Register.EAX, Register.ECX), "or eax, ecx"),
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