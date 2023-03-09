// <copyright file="ImageFileHeaderTest.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>

namespace Tea.Compiler.Coff.UnitTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Tea.Compiler.Binary;

    /// <summary>
    /// Unit tests for the <see cref="ImageFileHeader"/> class.
    /// </summary>
    [TestClass]
    public class ImageFileHeaderTest
    {
        /// <summary>
        /// Unit test to check the serialized size is the same as the constant.
        /// </summary>
        [TestMethod]
        public void BinarySizeTest()
        {
            ImageFileHeader target = new ImageFileHeader();
            MemoryStream ms = new MemoryStream();
            StreamBinaryWriter writer = new StreamBinaryWriter(ms);
            target.Serialize(writer);
            Assert.AreEqual(ImageFileHeader.BinarySize, ms.Length);
        }
    }
}
