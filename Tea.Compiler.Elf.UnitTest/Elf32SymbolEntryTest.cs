//-----------------------------------------------------------------------
// <copyright file="Elf32SymbolEntryTest.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler.Elf.UnitTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Tea.Compiler.Binary;

    /// <summary>
    /// Unit tests for the <see cref="Elf32SymbolEntry"/> class.
    /// </summary>
    [TestClass]
    public class Elf32SymbolEntryTest
    {
        /// <summary>
        /// Tests the binary size matches.
        /// </summary>
        [TestMethod]
        public void BinarySizeTest()
        {
            Elf32SymbolEntry target = default;
            using (MemoryStream ms = new MemoryStream())
            {
                StreamBinaryWriter writer = new StreamBinaryWriter(ms);
                target.Serialize(writer);
                Assert.AreEqual(Elf32SymbolEntry.BinarySize, ms.Length);
            }
        }
    }
}