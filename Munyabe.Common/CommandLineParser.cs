using System.Collections.Generic;
using System.Linq;

namespace Munyabe.Common
{
    /// <summary>
    /// コマンドライン引数を解析するクラスです。
    /// </summary>
    public static class CommandLineParser
    {
        /// <summary>
        /// 引数のキーを指定するプレフィックスです。
        /// </summary>
        private static readonly string[] _defaultPrefixes = new[] { "-", "/" };

        /// <summary>
        /// コマンドライン引数を解析します。
        /// </summary>
        /// <param name="args">コマンドライン引数の配列</param>
        /// <param name="prefixes">引数のキーを指定するプレフィックス</param>
        /// <returns>引数の種別をキーに値を保持するディクショナリー</returns>
        public static IDictionary<string, string> Parse(string[] args, params string[] prefixes)
        {
            var result = new Dictionary<string, string>();
            var switchPrefixes = prefixes.Any() ? prefixes : _defaultPrefixes;

            var currentKey = string.Empty;
            foreach (var arg in args)
            {
                if (switchPrefixes.Any(prefix => arg.StartsWith(prefix) && prefix.Length < arg.Length))
                {
                    var prefix = switchPrefixes.First(arg.StartsWith);
                    currentKey = arg.Substring(prefix.Length);
                    result.Add(currentKey, string.Empty);
                }
                else if (!string.IsNullOrEmpty(currentKey))
                {
                    result[currentKey] = arg;
                    currentKey = string.Empty;
                }
            }
            return result;
        }
    }
}
