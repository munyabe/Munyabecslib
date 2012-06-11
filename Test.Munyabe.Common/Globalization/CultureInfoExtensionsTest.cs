using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Munyabe.Common.Globalization;

namespace Test.Munyabe.Common.Globalization
{
    /// <summary>
    /// <see cref="CultureInfoExtensions"/>のテストクラスです。
    /// </summary>
    [TestClass]
    public class CultureInfoExtensionsTest
    {
        [TestMethod]
        public void IsMatchCultureTest()
        {
            Assert.IsTrue(new CultureInfo("ja-JP").IsMatchCulture(new CultureInfo("ja-JP")));
            Assert.IsTrue(new CultureInfo("ja").IsMatchCulture(new CultureInfo("ja-JP")));
            Assert.IsTrue(new CultureInfo("ja").IsMatchCulture(new CultureInfo("ja")));

            Assert.IsFalse(new CultureInfo("ja-JP").IsMatchCulture(new CultureInfo("ja")));
        }

        [TestMethod]
        public void IsTwoByteCultureTest()
        {
            Assert.IsTrue(new CultureInfo("ja").IsTwoByteCulture());
            Assert.IsTrue(new CultureInfo("ja-JP").IsTwoByteCulture());

            Assert.IsTrue(new CultureInfo("zh").IsTwoByteCulture());
            Assert.IsTrue(new CultureInfo("zh-CN").IsTwoByteCulture());
            Assert.IsTrue(new CultureInfo("zh-hk").IsTwoByteCulture());
            Assert.IsTrue(new CultureInfo("zh-sg").IsTwoByteCulture());
            Assert.IsTrue(new CultureInfo("zh-tw").IsTwoByteCulture());

            Assert.IsTrue(new CultureInfo("ko").IsTwoByteCulture());

            Assert.IsFalse(new CultureInfo("en").IsTwoByteCulture());
            Assert.IsFalse(new CultureInfo("en-US").IsTwoByteCulture());
        }

        [TestMethod]
        public void GetRootCultureTest()
        {
            Assert.AreEqual(new CultureInfo("ja"), new CultureInfo("ja-JP").GetRootCulture());
            Assert.AreEqual(new CultureInfo("ja"), new CultureInfo("ja").GetRootCulture());
            Assert.AreEqual(new CultureInfo("zh"), new CultureInfo("zh-CN").GetRootCulture());
            Assert.AreEqual(new CultureInfo("zh"), new CultureInfo("zh-CHT").GetRootCulture());
            Assert.AreEqual(new CultureInfo("zh"), new CultureInfo("zh-Hant").GetRootCulture());
            Assert.AreEqual(new CultureInfo("en"), new CultureInfo("en-US").GetRootCulture());
        }
    }
}
