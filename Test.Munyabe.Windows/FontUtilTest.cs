using System.Linq;
using System.Windows.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Munyabe.Windows;

namespace Test.Munyabe.Windows
{
    /// <summary>
    /// <see cref="FontUtil"/>のテストクラスです。
    /// </summary>
    [TestClass]
    public class FontUtilTest
    {
        [TestMethod]
        public void IsInstalledFontTest()
        {
            Assert.IsTrue(FontUtil.IsInstalledFont("Arial"));
            Assert.IsTrue(FontUtil.IsInstalledFont("Meiryo"));

            var meiryo = new FontFamily("Meiryo");
            Assert.IsTrue(meiryo.FamilyNames.Values.All(FontUtil.IsInstalledFont));

            Assert.IsFalse(FontUtil.IsInstalledFont("Hoge"));
            Assert.IsFalse(FontUtil.IsInstalledFont("ほげ"));

            Assert.IsFalse(FontUtil.IsInstalledFont(string.Empty));
            Assert.IsFalse(FontUtil.IsInstalledFont(null));
        }
    }
}
