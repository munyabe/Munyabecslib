using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Munyabe.Common;

namespace Test.Munyabe.Common
{
    /// <summary>
    /// <see cref="MemoryChecker"/>のテストクラスです。
    /// </summary>
    [TestClass]
    public class MemoryCheckerTest
    {
        [TestMethod]
        public void GetAliveObjectsTest()
        {
            var target1 = new LeakedTarget();
            MemoryChecker.RegisterCheckObject(target1);

            var target2 = new LeakedTarget();
            MemoryChecker.RegisterCheckObject(target2);

            var results = MemoryChecker.GetAliveObjects().ToList();
            Assert.AreEqual(2, results.Count);
            Assert.IsTrue(results.Contains(target1));
            Assert.IsTrue(results.Contains(target2));

            target1 = null;
            results = null;

            results = MemoryChecker.GetAliveObjects().ToList();
            Assert.AreEqual(1, results.Count);
            Assert.IsTrue(results.Contains(target2));

            target2 = null;
            results = null;

            results = MemoryChecker.GetAliveObjects().ToList();
            Assert.AreEqual(0, results.Count);
        }

        private sealed class LeakedTarget
        { }
    }
}
