using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Munyabe.Common;

namespace Test.Munyabe.Common
{
    /// <summary>
    /// <see cref="IDictionaryExtensions"/>のテストクラスです。
    /// </summary>
    [TestClass]
    public class IDictionaryExtensionsTest
    {
        private Dictionary<string, int> intValues = new Dictionary<string, int>();
        private Dictionary<string, string> stringValues = new Dictionary<string, string>();

        [TestInitialize]
        public void Initialize()
        {
            intValues["int1"] = 100;
            stringValues["string1"] = "Hoge";
        }

        [TestMethod]
        public void EqualsPairTest()
        {
            var first = new Dictionary<int, string>();
            first.Add(1, "First");
            first.Add(2, "Second");

            Assert.IsFalse(first.EqualsPair(null));

            var sameDic = new Dictionary<int, string>();
            sameDic.Add(1, "First");
            sameDic.Add(2, "Second");

            Assert.IsTrue(first.EqualsPair(sameDic));
            Assert.IsTrue(sameDic.EqualsPair(first));

            var differentDic = new Dictionary<int, string>();
            differentDic.Add(1, "1st");
            differentDic.Add(2, "2nd");

            Assert.IsFalse(first.EqualsPair(differentDic));
            Assert.IsFalse(differentDic.EqualsPair(first));

            var nullValueDic = new Dictionary<int, string>();
            nullValueDic.Add(1, null);
            nullValueDic.Add(2, null);

            Assert.IsFalse(first.EqualsPair(nullValueDic));
            Assert.IsFalse(nullValueDic.EqualsPair(first));

            var nullValueDic2 = new Dictionary<int, string>();
            nullValueDic2.Add(1, null);
            nullValueDic2.Add(2, null);

            Assert.IsTrue(nullValueDic.EqualsPair(nullValueDic2));
        }

        [TestMethod]
        public void GetOrAddTest()
        {
            Assert.AreEqual(100, intValues.GetOrAdd("int1", 500));
            Assert.AreEqual(100, intValues["int1"]);

            Assert.AreEqual(500, intValues.GetOrAdd("int2", 500));
            Assert.AreEqual(500, intValues["int2"]);
        }

        [TestMethod]
        public void GetOrAddByFactoryTest()
        {
            Assert.AreEqual("Hoge", stringValues.GetOrAdd("string1", key => "Fuga"));
            Assert.AreEqual("Hoge", stringValues["string1"]);

            Assert.AreEqual("string2_Hoge", stringValues.GetOrAdd("string2", key => key + "_Hoge"));
            Assert.AreEqual("string2_Hoge", stringValues["string2"]);
        }

        [TestMethod]
        public void GetOrDefaultTest()
        {
            Assert.AreEqual(100, intValues.GetOrDefault("int1"));
            Assert.AreEqual(0, intValues.GetOrDefault("int2"));

            Assert.AreEqual("Hoge", stringValues.GetOrDefault("string1"));
            Assert.AreEqual(null, stringValues.GetOrDefault("string2"));
        }

        [TestMethod]
        public void GetOrDefaultBySetValueTest()
        {
            Assert.AreEqual(200, intValues.GetOrDefault("int3", 200));
            Assert.AreEqual(string.Empty, stringValues.GetOrDefault("string3", string.Empty));
        }

        [TestMethod]
        public void GetOrDefaultByFactoryTest()
        {
            Assert.AreEqual(200, intValues.GetOrDefault("int3", key => 200));
            Assert.AreEqual(string.Empty, stringValues.GetOrDefault("string3", key => string.Empty));
        }
    }
}