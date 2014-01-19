using System;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace Munyabe.Windows.Drawing
{
    /// <summary>
    /// <see cref="System.Drawing"/>名前空間のクラスに関する拡張メソッドを定義するクラスです。
    /// </summary>
    public static class DrawingExtensions
    {
        /// <summary>
        /// <see cref="Drawing.Image"/>を<see cref="BitmapSource"/>に変換します。
        /// </summary>
        /// <param name="source"><see cref="Drawing.Image"/>インスタンス</param>
        /// <returns>変換された<see cref="BitmapSource"/>インスタンス</returns>
        public static BitmapSource ToBitmapSource(this Image source)
        {
            using (var bitmap = new Bitmap(source))
            {
                return bitmap.ToBitmapSource();
            }
        }

        /// <summary>
        /// <see cref="Drawing.Bitmap"/>を<see cref="BitmapSource"/>に変換します。
        /// </summary>
        /// <param name="source"><see cref="Drawing.Bitmap"/>インスタンス</param>
        /// <returns>変換された<see cref="BitmapSource"/>インスタンス</returns>
        public static BitmapSource ToBitmapSource(this Bitmap source)
        {
            var hBitmap = source.GetHbitmap();
            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                if (hBitmap != IntPtr.Zero)
                    NativeMethods.DeleteObject(hBitmap);
            }
        }

        /// <summary>
        /// <see cref="Drawing.Icon"/>を<see cref="BitmapSource"/>に変換します。
        /// </summary>
        /// <param name="source"><see cref="Drawing.Icon"/>インスタンス</param>
        /// <returns>変換された<see cref="BitmapSource"/>インスタンス</returns>
        public static BitmapSource ToBitmapSource(this Icon source)
        {
            return Imaging.CreateBitmapSourceFromHIcon(
                source.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }
    }
}