using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Munyabe.Common;

namespace Test.Munyabe.Common
{
    /// <summary>
    /// <see cref="ConverterUtil"/> のテストクラスです。
    /// </summary>
    [TestClass]
    public class ConverterUtilTest
    {
        [TestMethod]
        public void ConvertValueTest()
        {
            Assert.AreEqual(11, ConverterUtil.ConvertValue<int>("11"));
            Assert.AreEqual(11, ConverterUtil.ConvertValue<int>(11));
            Assert.AreEqual(true, ConverterUtil.ConvertValue<bool>("true"));
            Assert.AreEqual(true, ConverterUtil.ConvertValue<bool>(true));
            Assert.AreEqual(1.1, ConverterUtil.ConvertValue<double>("1.1"));
            Assert.AreEqual(new DateTime(2010, 1, 2), ConverterUtil.ConvertValue<DateTime>("2010/1/2"));
        }

        [TestMethod]
        public void ConvertValueNonGenericTest()
        {
            Assert.AreEqual(11, ConverterUtil.ConvertValue("11", typeof(int)));
            Assert.AreEqual(11, ConverterUtil.ConvertValue(11, typeof(int)));
            Assert.AreEqual(true, ConverterUtil.ConvertValue(true, typeof(bool)));
            Assert.AreEqual(new DateTime(2010, 1, 2), ConverterUtil.ConvertValue("2010/1/2", typeof(DateTime)));
            Assert.IsNull(ConverterUtil.ConvertValue(null, typeof(int)));
        }

        [TestMethod]
        public void TryConvertValueTest()
        {
            {
                object result;
                Assert.IsTrue(ConverterUtil.TryConvertValue("true", typeof(Nullable<bool>), out result));
                Assert.AreEqual(true, result);
            }
            {
                object result;
                Assert.IsTrue(ConverterUtil.TryConvertValue(true, typeof(Nullable<bool>), out result));
                Assert.AreEqual(true, result);
            }
            {
                object result;
                Assert.IsTrue(ConverterUtil.TryConvertValue("1", typeof(int), out result));
                Assert.AreEqual(1, result);
            }
            {
                object result;
                Assert.IsTrue(ConverterUtil.TryConvertValue("1", typeof(Nullable<int>), out result));
                Assert.AreEqual(1, result);
            }
            {
                object result;
                Assert.IsTrue(ConverterUtil.TryConvertValue(2, typeof(Nullable<int>), out result));
                Assert.AreEqual(2, result);
            }
            {
                object result;
                Assert.IsFalse(ConverterUtil.TryConvertValue("aaa", typeof(int), out result));
                Assert.AreEqual(0, result);
            }
        }
    }
}