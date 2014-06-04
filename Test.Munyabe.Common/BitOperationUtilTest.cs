using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Munyabe.Common;

namespace Test.Munyabe.Common
{
    /// <summary>
    /// <see cref="BitOperationUtil"/>のテストクラスです。
    /// </summary>
    [TestClass]
    public class BitOperationUtilTest
    {
        [TestMethod]
        public void DivideToBitsTest()
        {
            CollectionAssert.AreEqual(new[] { 1 }, BitOperationUtil.DivideToBits(1).ToArray());
            CollectionAssert.AreEqual(new[] { 2, 4 }, BitOperationUtil.DivideToBits(6).ToArray());
            CollectionAssert.AreEqual(new[] { 8 }, BitOperationUtil.DivideToBits(8).ToArray());
            CollectionAssert.AreEqual(new[] { 1, 4, 8 }, BitOperationUtil.DivideToBits(13).ToArray());

            Assert.IsFalse(BitOperationUtil.DivideToBits(0).Any());
        }

        [TestMethod]
        public void IsSingleBitTest()
        {
            Assert.IsTrue(BitOperationUtil.IsSingleBit(1));
            Assert.IsTrue(BitOperationUtil.IsSingleBit(8));
            Assert.IsTrue(BitOperationUtil.IsSingleBit(256));

            Assert.IsFalse(BitOperationUtil.IsSingleBit(0));
            Assert.IsFalse(BitOperationUtil.IsSingleBit(7));
            Assert.IsFalse(BitOperationUtil.IsSingleBit(192));
        }
    }
}
