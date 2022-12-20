// <copyright file="ModuleTest.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>

namespace Tea.Compiler.UnitTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for the <see cref="Module"/> class.
    /// </summary>
    [TestClass]
    public class ModuleTest
    {
        /// <summary>
        /// Unit test for the <see cref="Module.DefineLiteralString(string)"/> method.
        /// </summary>
        [TestMethod]
        public void DefineLiteralStringTest()
        {
            Module target = new Module();
            string symbol = target.DefineLiteralString("Hello 🥓!");
            Assert.IsFalse(string.IsNullOrWhiteSpace(symbol));

            DataEntry? data = target.DataSegment.FirstOrDefault(e => e.Label != null && e.Label!.Equals(symbol, StringComparison.Ordinal));
            Assert.IsNotNull(data);
            Assert.IsNotNull(data.Value);
            Assert.IsTrue(data.Value.Length > 2);

            byte? firstChar = data.Value[0] as byte?;
            Assert.IsNotNull(firstChar);
            Assert.AreEqual((byte)'H', firstChar!);

            byte? nullChar = data.Value[^1] as byte?;
            Assert.IsNotNull(nullChar);
            Assert.AreEqual((byte)0, nullChar!);

            byte? lastChar = data.Value[^2] as byte?;
            Assert.IsNotNull(lastChar);
            Assert.AreNotEqual((byte)0, lastChar);
        }
    }
}
