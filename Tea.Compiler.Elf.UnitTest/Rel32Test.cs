//-----------------------------------------------------------------------
// <copyright file="Rel32Test.cs" company="Jon Rowlett">
//     Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Tea.Compiler.Elf.UnitTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Tea.Compiler.Binary;

    /// <summary>
    /// Unit test for the <see cref="Rel32"/> struct.
    /// </summary>
    [TestClass]
    public class Rel32Test
    {
        /// <summary>
        /// Tests the binary size matches.
        /// </summary>
        [TestMethod]
        public void BinarySizeTest()
        {
            Rel32 target = default;
            using (MemoryStream ms = new MemoryStream())
            {
                StreamBinaryWriter writer = new StreamBinaryWriter(ms);
                target.Serialize(writer);
                Assert.AreEqual(Rel32.BinarySize, ms.Length);
            }
        }
    }
}