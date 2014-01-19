using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Munyabe.Common;

namespace Test.Munyabe.Common
{
    /// <summary>
    /// <see cref="DateTimeExtensions"/>のテストクラスです。
    /// </summary>
    [TestClass]
    public class DateTimeExtensionsTest
    {
        [TestMethod]
        public void GetFirstDateOfMonthTest()
        {
            var julyDate = new DateTime(2011, 7, 10, 12, 30, 40);
            Assert.AreEqual(new DateTime(2011, 7, 1), julyDate.GetFirstDateOfMonth());
        }

        [TestMethod]
        public void GetLastDateOfMonthTest()
        {
            var februaryDate = new DateTime(2011, 2, 10);
            Assert.AreEqual(new DateTime(2011, 2, 28), februaryDate.GetLastDateOfMonth());

            var julyDate = new DateTime(2011, 7, 10, 12, 30, 40);
            Assert.AreEqual(new DateTime(2011, 7, 31), julyDate.GetLastDateOfMonth());
        }
    }
}