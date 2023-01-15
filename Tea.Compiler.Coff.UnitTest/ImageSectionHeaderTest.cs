// <copyright file="ImageSectionHeaderTest.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>

namespace Tea.Compiler.Coff.UnitTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Tea.Compiler.Binary;

    /// <summary>
    /// Unit tests for the <see cref="ImageSectionHeader"/> class.
    /// </summary>
    [TestClass]
    public class ImageSectionHeaderTest
    {
        /// <summary>
        /// Unit test to check the serialized size is the same as the constant.
        /// </summary>
        [TestMethod]
        public void BinarySizeTest()
        {
            ImageSectionHeader target = new ImageSectionHeader();
            MemoryStream ms = new MemoryStream();
            StreamBinaryWriter writer = new StreamBinaryWriter(ms);
            target.Serialize(writer);
            Assert.AreEqual(ImageSectionHeader.BinarySize, ms.Length);
        }
    }
}
