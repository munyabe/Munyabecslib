using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Munyabe.Common;

namespace Test.Munyabe.Common
{
    /// <summary>
    /// <see cref="TypeExtensions"/>のテストクラスです。
    /// </summary>
    [TestClass]
    public class TypeExtensionsTest
    {
        [TestMethod]
        public void CanSetNullTest()
        {
            Assert.IsTrue(typeof(string).CanSetNull());
            Assert.IsTrue(typeof(int?).CanSetNull());

            Assert.IsFalse(typeof(int).CanSetNull());
            Assert.IsFalse(typeof(DateTime).CanSetNull());
        }

        [TestMethod]
        public void GetBaseTypesTest()
        {
            Assert.IsTrue(
                typeof(TestClassAttribute)
                    .GetBaseTypes()
                    .SequenceEqual(new Type[] { typeof(Attribute), typeof(object) }));
        }

        [TestMethod]
        public void GetBaseTypesContainsSelfTest()
        {
            Assert.IsTrue(
                typeof(TestClassAttribute)
                    .GetBaseTypes(true)
                    .SequenceEqual(new Type[] { typeof(TestClassAttribute), typeof(Attribute), typeof(object) }));
        }

        [TestMethod]
        public void GetDafaultValueTest()
        {
            Assert.AreEqual(0, typeof(int).GetDafaultValue());
            Assert.AreEqual(false, typeof(bool).GetDafaultValue());
            Assert.AreEqual(null, typeof(string).GetDafaultValue());
            Assert.AreEqual(new DateTime(), typeof(DateTime).GetDafaultValue());
            Assert.AreEqual(null, typeof(Nullable<int>).GetDafaultValue());
        }

        [TestMethod]
        public void ToNullableTypeTest()
        {
            Assert.AreEqual(typeof(Nullable<int>), typeof(int).ToNullableType());
            Assert.AreEqual(typeof(Nullable<DateTime>), typeof(DateTime).ToNullableType());
            Assert.AreEqual(typeof(string), typeof(string).ToNullableType());
        }
    }
}