using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Munyabe.Common;

namespace Test.Munyabe.Common
{
    /// <summary>
    /// <see cref="StringUtil"/>のテストクラスです。
    /// </summary>
    [TestClass]
    public class StringUtilTest
    {
        [TestMethod]
        public void FormatTest()
        {
            Assert.AreEqual("this is a {bad} test", StringUtil.Format("this is a {bad} test", "name", "Format"));
            Assert.AreEqual("this is a Format test", StringUtil.Format("this is a {name} test", "name", "Format"));
            Assert.AreEqual("this is a Format test : Format", StringUtil.Format("this is a {name} test : {name}", "name", "Format"));
        }

        [TestMethod]
        public void FormatFromDictionaryTest()
        {
            var values = new Dictionary<string, string>();
            values["name"] = "Format";
            values["class"] = "StringUtil";

            Assert.AreEqual("this is a {bad} test", StringUtil.Format("this is a {bad} test", values));
            Assert.AreEqual("this is a Format test", StringUtil.Format("this is a {name} test", values));
            Assert.AreEqual("this is a Format test : Format", StringUtil.Format("this is a {name} test : {name}", values));

            Assert.AreEqual("this is a Format {bad} test", StringUtil.Format("this is a {name} {bad} test", values));
            Assert.AreEqual("this is a StringUtil.Format test", StringUtil.Format("this is a {class}.{name} test", values));
            Assert.AreEqual("this is a {Name} {bad} test", StringUtil.Format("this is a {Name} {bad} test", values));
        }
    }
}