using System;
using System.Collections.Generic;
using System.Globalization;
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
        /// <param name="oldValue">置換される文字列</param>
        /// <param name="newValue">出現するすべての対象を置換する文字列</param>
        /// <returns>書式項目が<paramref name="newValue"/>の対応する文字列に置換された<paramref name="format"/>の文字列</returns>
        /// <exception cref="ArgumentNullException"><paramref name="format"/>が<see langword="null"/>です。</exception>
        /// <exception cref="ArgumentNullException"><paramref name="oldValue"/>が<see langword="null"/>です。</exception>
        public static string Format(string format, string oldValue, string newValue)
        {
            Guard.ArgumentNotNull(format, "format");
            Guard.ArgumentNotNull(oldValue, "oldValue");

            return format.Replace(string.Format(CultureInfo.InvariantCulture, "{{{0}}}", oldValue), newValue);
        }

        /// <summary>
        /// 指定した文字列の書式項目を、指定した文字列に置換します。
        /// </summary>
        /// <param name="format">複合書式指定文字列</param>
        /// <param name="newValues">出現するすべての対象を置換する文字列</param>
        /// <returns>書式項目が<paramref name="newValues"/>の対応する文字列に置換された<paramref name="format"/>の文字列</returns>
        /// <exception cref="ArgumentNullException"><paramref name="format"/>が<see langword="null"/>です。</exception>
        /// <exception cref="ArgumentNullException"><paramref name="newValues"/>が<see langword="null"/>です。</exception>
        public static string Format(string format, IDictionary<string, string> newValues)
        {
            Guard.ArgumentNotNull(format, "format");
            Guard.ArgumentNotNull(newValues, "newValues");

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