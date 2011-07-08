using System;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Munyabe.Common.Xml;

namespace Test.Munyabe.Common.Xml
{
    /// <summary>
    /// <see cref="XElementExtensions"/>のテストクラスです。
    /// </summary>
    [TestClass]
    public class XElementExtensionsTest
    {
        private const string TEST_XML = @"<Person Name=""Ichiro"" Age=""37"" />";
        private XElement _element = XElement.Parse(TEST_XML);

        [TestMethod]
        public void GetAttributeValueTest()
        {
            Assert.AreEqual("Ichiro", _element.GetAttributeValue("Name"));
            Assert.AreEqual("37", _element.GetAttributeValue("Age"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetAttributeValueErrorTest()
        {
            Assert.AreEqual("Error", _element.GetAttributeValue("Hoge"));
        }

        [TestMethod]
        public void GetAttributeValueOrDefaultTest()
        {
            Assert.AreEqual("Ichiro", _element.GetAttributeValueOrDefault("Name"));
            Assert.AreEqual("37", _element.GetAttributeValueOrDefault("Age"));
            Assert.AreEqual(string.Empty, _element.GetAttributeValueOrDefault("Hoge"));

            Assert.AreEqual("Default", _element.GetAttributeValueOrDefault("Hoge", "Default"));
        }
    }
}