using System;

namespace Munyabe.VisualStudio
{
    /// <summary>
    /// ファイルの種別を表すフラグです。
    /// </summary>
    [Flags]
    public enum FileTypeFlags
    {
        /// <summary>
        /// ファイルを指定しません。
        /// </summary>
        None = 0,

        /// <summary>
        /// C# ファイルです。
        /// </summary>
        CSharp = 1,

        /// <summary>
        /// XAML ファイルです。
        /// </summary>
        Xaml = 2,

        /// <summary>
        /// リソースファイルです。
        /// </summary>
        ResX = 4
    }
}
