using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Munyabe.Common;

namespace Test.Munyabe.Common
{
    /// <summary>
    /// <see cref="StringExtensions"/>のテストクラスです。
    /// </summary>
    [TestClass]
    public class StringExtensionsTest
    {
        [TestMethod]
        public void GetUpperCasesTest()
        {
            Assert.AreEqual("SET", "StringExtensionsTest".GetUpperCases());
            Assert.AreEqual("ET", "stringExtensionsTest".GetUpperCases());
            Assert.AreEqual("EXT", "stringEXtensionsTest".GetUpperCases());
            Assert.AreEqual(string.Empty, "string".GetUpperCases());
        }

        [TestMethod]
        public void RemoveNewLineTest()
        {
            Assert.AreEqual("ab", "a\r\nb".RemoveNewLine());
            Assert.AreEqual("ab", "a\rb".RemoveNewLine());
            Assert.AreEqual("ab", "a\nb".RemoveNewLine());
            Assert.AreEqual("ab", "\r\na\rb\n".RemoveNewLine());

            var newLineString = "a" + Environment.NewLine + "b";
            Assert.AreEqual("ab", newLineString.RemoveNewLine());
            Assert.AreEqual("ab", @"a
b".RemoveNewLine());

            Assert.AreEqual("abcdefg", "abcdefg".RemoveNewLine());
        }

        [TestMethod]
        public void TrimTest()
        {
            Assert.AreEqual("Value", "TypeValue".Trim("Type"));
            Assert.AreEqual("Value", "TypeTypeValue".Trim("Type"));
            Assert.AreEqual("Value", "{0}Value".Trim("{0}"));

            Assert.AreEqual("Type", "TypeValue".Trim("Value"));
            Assert.AreEqual("Type", "TypeValueValue".Trim("Value"));
            Assert.AreEqual("Type", "Type{0}".Trim("{0}"));

            Assert.AreEqual("Type", "Type".Trim("Value"));
            Assert.AreEqual("Type", "Type".Trim(string.Empty));
            Assert.AreEqual(string.Empty, "Value".Trim("Value"));
            Assert.AreEqual(string.Empty, string.Empty.Trim("Value"));

            Assert.AreEqual("Type", "ValueTypeValue".Trim("Value"));
            Assert.AreEqual("Type", "{0}Type{0}".Trim("{0}"));
        }

        [TestMethod]
        public void TrimEndTest()
        {
            Assert.AreEqual("Type", "TypeValue".TrimEnd("Value"));
            Assert.AreEqual("Type", "TypeValueValue".TrimEnd("Value"));
            Assert.AreEqual("Type", "Type".TrimEnd("Value"));
            Assert.AreEqual("Type", "Type".TrimEnd(string.Empty));
            Assert.AreEqual(string.Empty, "Value".TrimEnd("Value"));
            Assert.AreEqual(string.Empty, string.Empty.TrimEnd("Value"));

            Assert.AreEqual("Type", "Type{0}".TrimEnd("{0}"));
        }

        [TestMethod]
        public void TrimStartTest()
        {
            Assert.AreEqual("Value", "TypeValue".TrimStart("Type"));
            Assert.AreEqual("Value", "TypeTypeValue".TrimStart("Type"));
            Assert.AreEqual("Type", "Type".TrimStart("Value"));
            Assert.AreEqual("Type", "Type".TrimStart(string.Empty));
            Assert.AreEqual(string.Empty, "Value".TrimStart("Value"));
            Assert.AreEqual(string.Empty, string.Empty.TrimStart("Type"));

            Assert.AreEqual("Value", "{0}Value".TrimStart("{0}"));
        }
    }
}