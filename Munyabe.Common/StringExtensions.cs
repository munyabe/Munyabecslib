using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Munyabe.Common
{
    /// <summary>
    /// <see cref="string"/>に関するユーティリティクラスです。
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 文字列から大文字のみを抽出します。
        /// </summary>
        /// <param name="source">値を抽出する文字列</param>
        /// <returns>抽出した大文字の文字列</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>が<see langword="null"/>です。</exception>
        public static string GetUpperCases(this string source)
        {
            Guard.ArgumentNotNull(source, "value");

            if (string.IsNullOrWhiteSpace(source))
            {
                return string.Empty;
            }

            var chars = source.Where(char.IsUpper);
            return string.Concat(chars);
        }

        /// <summary>
        /// 文字列の先頭および末尾から指定された文字列を削除します。
        /// </summary>
        /// <param name="source">削除対象の文字列</param>
        /// <param name="removeValue">削除する文字列</param>
        /// <returns>指定された文字列を削除した後に残った文字列</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>が<see langword="null"/>です。</exception>
        public static string Trim(this string source, string removeValue)
        {
            return TrimHelper(source, removeValue, "^({0})+|({0})+$");
        }

        /// <summary>
        /// 文字列の末尾から指定された文字列を削除します。
        /// </summary>
        /// <param name="source">削除対象の文字列</param>
        /// <param name="removeValue">削除する文字列</param>
        /// <returns>指定された文字列を削除した後に残った文字列</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>が<see langword="null"/>です。</exception>
        public static string TrimEnd(this string source, string removeValue)
        {
            return TrimHelper(source, removeValue, "({0})+$");
        }

        /// <summary>
        /// 文字列の先頭から指定された文字列を削除します。
        /// </summary>
        /// <param name="source">削除対象の文字列</param>
        /// <param name="removeValue">削除する文字列</param>
        /// <returns>指定された文字列を削除した後に残った文字列</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>が<see langword="null"/>です。</exception>
        public static string TrimStart(this string source, string removeValue)
        {
            return TrimHelper(source, removeValue, "^({0})+");
        }

        /// <summary>
        /// 文字列から指定されたパターンに一致する文字列を削除するヘルパーメソッドです。
        /// </summary>
        private static string TrimHelper(string source, string removeValue, string patternFormat)
        {
            Guard.ArgumentNotNull(source, "value");

            var pattern = string.Format(patternFormat, Regex.Escape(removeValue));
            return Regex.Replace(source, pattern, string.Empty);
        }
    }
}