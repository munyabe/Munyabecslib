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
        /// <param name="value">値を抽出する文字列</param>
        /// <returns>抽出した大文字の文字列</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/>が<see langword="null"/>です。</exception>
        public static string GetUpperCases(this string value)
        {
            Guard.ArgumentNotNull(value, "value");

            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            var chars = value.Where(char.IsUpper);
            return string.Concat(chars);
        }

        /// <summary>
        /// 文字列の先頭および末尾から指定された文字列を削除します。
        /// </summary>
        /// <param name="value">削除対象の文字列</param>
        /// <param name="trimString">削除する文字列</param>
        /// <returns>指定された文字列を削除した後に残った文字列</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/>が<see langword="null"/>です。</exception>
        public static string Trim(this string value, string trimString)
        {
            return TrimHelper(value, trimString, "^({0})+|({0})+$");
        }

        /// <summary>
        /// 文字列の末尾から指定された文字列を削除します。
        /// </summary>
        /// <param name="value">削除対象の文字列</param>
        /// <param name="trimString">削除する文字列</param>
        /// <returns>指定された文字列を削除した後に残った文字列</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/>が<see langword="null"/>です。</exception>
        public static string TrimEnd(this string value, string trimString)
        {
            return TrimHelper(value, trimString, "({0})+$");
        }

        /// <summary>
        /// 文字列の先頭から指定された文字列を削除します。
        /// </summary>
        /// <param name="value">削除対象の文字列</param>
        /// <param name="trimString">削除する文字列</param>
        /// <returns>指定された文字列を削除した後に残った文字列</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/>が<see langword="null"/>です。</exception>
        public static string TrimStart(this string value, string trimString)
        {
            return TrimHelper(value, trimString, "^({0})+");
        }

        /// <summary>
        /// 文字列から指定されたパターンに一致する文字列を削除するヘルパーメソッドです。
        /// </summary>
        private static string TrimHelper(string value, string trimString, string patternFormat)
        {
            Guard.ArgumentNotNull(value, "value");

            var pattern = string.Format(patternFormat, Regex.Escape(trimString));
            return Regex.Replace(value, pattern, string.Empty);
        }
    }
}