using System;
using System.Runtime.InteropServices;

namespace Munyabe.Windows
{
    /// <summary>
    /// WIN32 API呼出しを定義するための内部クラスです。
    /// </summary>
    internal static class NativeMethods
    {
        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject(IntPtr hObject);
    }
}