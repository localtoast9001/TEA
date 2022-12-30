//-----------------------------------------------------------------------
// <copyright file="RMTest.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler.UnitTest.X86
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Tea.Compiler.X86;

    /// <summary>
    /// Unit tests for the <See cref="RM"/> class.
    /// </summary>
    [TestClass]
    public class RMTest
    {
        /// <summary>
        /// Unit test for the <see cref="RM.ToString"/> method.
        /// </summary>
        [TestMethod]
        public void ToStringTest()
        {
            Tuple<RM, string>[] testCases = new Tuple<RM, string>[]
            {
                new Tuple<RM, string>(RM.FromRegister(Register.EAX), "eax"),
                new Tuple<RM, string>(RM.Address(Register.EAX), "dword ptr [eax]"),
                new Tuple<RM, string>(RM.Address(Register.EAX, 8), "qword ptr [eax]"),
                new Tuple<RM, string>(RM.Address(Register.EAX, 10), "tword ptr [eax]"),
                new Tuple<RM, string>(RM.Address(Register.ESP), "dword ptr [esp]"),
                new Tuple<RM, string>(RM.Address("label1"), "dword ptr [label1]"),
                new Tuple<RM, string>(RM.Address(Register.EBP, -4, "_x$"), "dword ptr _x$[ebp]"),
                new Tuple<RM, string>(RM.Address(Register.EBP, -4, null), "dword ptr -4[ebp]"),
                new Tuple<RM, string>(RM.Address(Register.ESP, 4, null), "dword ptr 4[esp]"),
                new Tuple<RM, string>(RM.Address(Register.EBP, -256, null), "dword ptr -256[ebp]"),
                new Tuple<RM, string>(RM.Address(Register.EBP, 320, null), "dword ptr 320[ebp]"),
            };

            foreach (var testCase in testCases)
            {
                string expected = testCase.Item2;
                string actual = string.Empty;
                try
                {
                    actual = testCase.Item1.ToString();
                }
                catch (Exception ex)
                {
                    Assert.Fail($"Unexpected exception. Expected: [{expected}]. Actual exception: [{ex.ToString()}].");
                }

                Assert.AreEqual(expected, actual);
            }
        }
    }
}