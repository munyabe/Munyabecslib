using System.Linq;

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
        public static string GetUpperCases(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            var chars = value.Where(char.IsUpper);
            return string.Concat(chars);
        }
    }
}