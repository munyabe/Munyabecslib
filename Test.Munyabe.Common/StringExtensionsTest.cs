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
    }
}