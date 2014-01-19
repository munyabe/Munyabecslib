using Microsoft.VisualStudio.TestTools.UnitTesting;
using Munyabe.Common;

namespace Test.Munyabe.Common
{
    /// <summary>
    /// <see cref="ConsoleUtil"/>のテストクラスです。
    /// </summary>
    [TestClass]
    public class ConsoleUtilTest
    {
        [TestMethod]
        public void ParseTest()
        {
            var args = "-a hoge -b -cc fuga".Split(' ');
            var result = ConsoleUtil.ParseArgs(args);

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("hoge", result["a"]);
            Assert.AreEqual(string.Empty, result["b"]);
            Assert.AreEqual("fuga", result["cc"]);
        }

        [TestMethod]
        public void ParseTest2()
        {
            var args = "-a hoge _b +cc fuga".Split(' ');
            var result = ConsoleUtil.ParseArgs(args);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("hoge", result["a"]);
        }

        [TestMethod]
        public void ParseTest3()
        {
            var args = "-a hoge -".Split(' ');
            var result = ConsoleUtil.ParseArgs(args);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("hoge", result["a"]);
        }

        [TestMethod]
        public void ParseTest4()
        {
            var args = "--a hoge -b --c".Split(' ');
            var result = ConsoleUtil.ParseArgs(args, "--");

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("hoge", result["a"]);
            Assert.AreEqual(string.Empty, result["c"]);
        }
    }
}
