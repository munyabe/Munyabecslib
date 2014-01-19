using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Munyabe.Windows;

namespace Test.Munyabe.Windows
{
    [TestClass]
    public class RectExtensionsTest
    {
        [TestMethod]
        public void CenterTest()
        {
            var rect = new Rect(200, 100, 400, 300);
            var center = rect.Center();

            Assert.AreEqual(400, center.X);
            Assert.AreEqual(250, center.Y);
        }
    }
}
