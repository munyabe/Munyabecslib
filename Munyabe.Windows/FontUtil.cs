using System.Linq;
using System.Windows.Media;

namespace Munyabe.Windows
{
    /// <summary>
    /// フォントに関するユーティリティクラスです。
    /// </summary>
    public static class FontUtil
    {
        /// <summary>
        /// システムにインストールされているフォントかどうかを判定します。
        /// </summary>
        /// <param name="fontName">フォント名</param>
        /// <returns>インストールされている場合は<see langword="true"/></returns>
        public static bool IsInstalledFont(string fontName)
        {
            return Fonts.SystemFontFamilies.Any(family => family.FamilyNames.Values.Contains(fontName));
        }
    }
}