// <copyright file="TypeDefinitionTest.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>

namespace Tea.Compiler.UnitTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for the <see cref="TypeDefinition"/> class.
    /// </summary>
    [TestClass]
    public class TypeDefinitionTest
    {
        /// <summary>
        /// Unit test for the <see cref="TypeDefinition.Equals(TypeDefinition?)"/> method.
        /// </summary>
        [TestMethod]
        public void EqualsTest()
        {
            CompilerContext context = new CompilerContext();
            Tuple<TypeDefinition, TypeDefinition?, bool>[] testCases = new Tuple<TypeDefinition, TypeDefinition?, bool>[]
            {
                new Tuple<TypeDefinition, TypeDefinition?, bool>(context.BooleanType, context.IntegerType, false),
                new Tuple<TypeDefinition, TypeDefinition?, bool>(context.BooleanType, context.BooleanType, true),
                new Tuple<TypeDefinition, TypeDefinition?, bool>(context.SingleType, context.DoubleType, false),
                new Tuple<TypeDefinition, TypeDefinition?, bool>(context.SingleType, null, false),
            };

            foreach (var testCase in testCases)
            {
                TypeDefinition target = testCase.Item1;
                TypeDefinition? other = testCase.Item2;
                bool expected = testCase.Item3;
                int targetHashCode = target.GetHashCode();

                bool actual = false;
                int otherHashCode = 0;
                try
                {
                    actual = target.Equals(other);
                    if (other != null)
                    {
                        otherHashCode = other!.GetHashCode();
                    }
                }
                catch (Exception ex)
                {
                    Assert.Fail($"Unexpected exception for test case [{target.FullName},{other?.FullName}]. Exception: [{ex}].");
                }

                Assert.AreEqual(expected, actual, $"[{target.FullName}].Equals([{other?.FullName}])");
                if (expected)
                {
                    Assert.AreEqual(targetHashCode, otherHashCode, $"Hash code do not match for case [{target.FullName}].");
                }
            }
        }
    }
}
