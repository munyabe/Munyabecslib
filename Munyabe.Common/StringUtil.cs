using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Munyabe.Common
{
    /// <summary>
    /// <see cref="string"/>に関するユーティリティクラスです。
    /// </summary>
    public static class StringUtil
    {
        /// <summary>
        /// 指定した文字列の書式項目を、指定した文字列に置換します。
        /// </summary>
        /// <param name="format">複合書式指定文字列</param>
        /// <param name="from">置換対象を指定する文字列</param>
        /// <param name="to">出現するすべての対象を置換する文字列</param>
        /// <returns>書式項目が<paramref name="to"/>の対応する文字列に置換された<paramref name="format"/>の文字列</returns>
        public static string Format(string format, string from, string to)
        {
            if (string.IsNullOrWhiteSpace(format) || string.IsNullOrWhiteSpace(from))
            {
                return string.Empty;
            }

            return format.Replace(string.Format("{{{0}}}", from), to);
        }

        /// <summary>
        /// 指定した文字列の書式項目を、指定した文字列に置換します。
        /// </summary>
        /// <param name="format">複合書式指定文字列</param>
        /// <param name="newValues">出現するすべての対象を置換する文字列</param>
        /// <returns>書式項目が<paramref name="newValues"/>の対応する文字列に置換された<paramref name="format"/>の文字列</returns>
        public static string Format(string format, IDictionary<string, string> newValues)
        {
            Guard.ArgumentNotNull(newValues, "newValues");

            if (string.IsNullOrWhiteSpace(format))
            {
                return string.Empty;
            }

            var regex = new Regex("{[^${}]*}");
            return regex.Replace(format, match =>
            {
                var from = match.Value.Substring(1, match.Length - 2);
                string value;
                return newValues.TryGetValue(from, out value) ? value : match.Value;
            });
        }
    }
}