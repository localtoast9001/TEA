//-----------------------------------------------------------------------
// <copyright file="ExtensionsTest.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler.UnitTest.X86
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Tea.Compiler.X86;

    /// <summary>
    /// Unit tests for the <see cref="Extensions"/> class.
    /// </summary>
    [TestClass]
    public class ExtensionsTest
    {
        /// <summary>
        /// Unit test for the <see cref="Extensions.Size(Register)"/> method.
        /// </summary>
        [TestMethod]
        public void SizeTest()
        {
            Assert.AreEqual(sizeof(byte), Register.AL.Size());
            Assert.AreEqual(sizeof(byte), Register.AH.Size());
            Assert.AreEqual(sizeof(ushort), Register.AX.Size());
            Assert.AreEqual(sizeof(uint), Register.EAX.Size());
        }

        /// <summary>
        /// Unit test for the <see cref="Extensions.RegisterCode(Register)"/> method.
        /// </summary>
        [TestMethod]
        public void RegisterCodeTest()
        {
            Assert.AreEqual((byte)0U, Register.AL.RegisterCode());
            Assert.AreEqual((byte)0U, Register.AX.RegisterCode());
            Assert.AreEqual((byte)0U, Register.EAX.RegisterCode());
        }

        /// <summary>
        /// Unit test for the <see cref="Extensions.FromRegisterCode(byte, int)"/> method.
        /// </summary>
        [TestMethod]
        public void FromRegisterCodeTest()
        {
            Assert.AreEqual(Register.AL, ((byte)0).FromRegisterCode(sizeof(byte)));
            Assert.AreEqual(Register.AX, ((byte)0).FromRegisterCode(sizeof(ushort)));
            Assert.AreEqual(Register.EAX, ((byte)0).FromRegisterCode(sizeof(uint)));
        }

        /// <summary>
        /// Unit test for the <see cref="Extensions.ToUInt32(ReadOnlySpan{byte})"/> method.
        /// </summary>
        [TestMethod]
        public void ToUInt32Test()
        {
            byte[] data = new byte[] { 1, 2, 3, 4 };
            Assert.AreEqual(0x04030201U, Extensions.ToUInt32(new ReadOnlySpan<byte>(data)));
        }

        /// <summary>
        /// Unit test for the <see cref="Extensions.ToInt32(ReadOnlySpan{byte})"/> method.
        /// </summary>
        [TestMethod]
        public void ToInt32Test()
        {
            byte[] data = new byte[] { 0xfc, 0xff, 0xff, 0xff };
            Assert.AreEqual(-4, Extensions.ToInt32(new ReadOnlySpan<byte>(data)));
        }

        /// <summary>
        /// Unit test for the <see cref="Extensions.ToInt16(ReadOnlySpan{byte})"/> method.
        /// </summary>
        [TestMethod]
        public void ToUInt16Test()
        {
            byte[] data = new byte[] { 1, 2 };
            Assert.AreEqual(0x0201U, Extensions.ToUInt16(new ReadOnlySpan<byte>(data)));
        }

        /// <summary>
        /// Unit test for the <see cref="Extensions.ToInt16(ReadOnlySpan{byte})"/> method.
        /// </summary>
        [TestMethod]
        public void ToInt16Test()
        {
            byte[] data = new byte[] { 0xfc, 0xff };
            Assert.AreEqual(-4, Extensions.ToInt16(new ReadOnlySpan<byte>(data)));
        }
    }
}