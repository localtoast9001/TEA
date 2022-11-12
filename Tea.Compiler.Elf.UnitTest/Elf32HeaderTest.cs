//-----------------------------------------------------------------------
// <copyright file="Elf32HeaderTest.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler.Elf.UnitTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for the <see cref="Elf32Header"/> class.
    /// </summary>
    [TestClass]
    public class Elf32HeaderTest
    {
        /// <summary>
        /// Tests the binary size matches.
        /// </summary>
        [TestMethod]
        public void BinarySizeTest()
        {
            Elf32Header target = new Elf32Header();
            using (MemoryStream ms = new MemoryStream())
            {
                StreamBinaryWriter writer = new StreamBinaryWriter(ms);
                target.Serialize(writer);
                Assert.AreEqual(Elf32Header.BinarySize, ms.Length);
            }
        }
    }
}