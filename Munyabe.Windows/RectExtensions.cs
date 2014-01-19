using System.Windows;

namespace Munyabe.Windows
{
    /// <summary>
    /// <see cref="Rect"/>の拡張メソッドを定義するクラスです。
    /// </summary>
    public static class RectExtensions
    {
        /// <summary>
        /// 四角形の中心座標を取得します。
        /// </summary>
        /// <param name="rect">四角形</param>
        /// <returns>中心座標</returns>
        public static Point Center(this Rect rect)
        {
            return new Point((rect.Left + rect.Right) / 2, (rect.Top + rect.Bottom) / 2);
        }
    }
}
