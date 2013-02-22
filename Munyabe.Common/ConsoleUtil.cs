using System;
using System.Collections.Generic;
using System.Linq;

namespace Munyabe.Common
{
    /// <summary>
    /// コンソールアプリケーションに関するユーティリティクラスです。
    /// </summary>
    public static class ConsoleUtil
    {
        /// <summary>
        /// コマンドライン引数のキーを指定するプレフィックスです。
        /// </summary>
        private static readonly string[] _defaultPrefixes = new[] { "-", "/" };

        /// <summary>
        /// コマンドライン引数を解析します。
        /// </summary>
        /// <param name="args">コマンドライン引数の配列</param>
        /// <param name="prefixes">引数のキーを指定するプレフィックス</param>
        /// <returns>引数の種別をキーに値を保持するディクショナリー</returns>
        public static IDictionary<string, string> ParseArgs(string[] args, params string[] prefixes)
        {
            var result = new Dictionary<string, string>();
            var switchPrefixes = prefixes.Any() ? prefixes : _defaultPrefixes;

            var currentKey = string.Empty;
            foreach (var arg in args)
            {
                if (switchPrefixes.Any(prefix => arg.StartsWith(prefix, StringComparison.Ordinal) && prefix.Length < arg.Length))
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

        /// <summary>
        /// 警告をコンソールに出力します。
        /// </summary>
        /// <param name="message">出力するメッセージ</param>
        public static void WriteAlert(string message)
        {
            WriteLine(message, ConsoleColor.Yellow);
        }

        /// <summary>
        /// エラーをコンソールに出力します。
        /// </summary>
        /// <param name="message">出力するメッセージ</param>
        public static void WriteError(string message)
        {
            WriteLine(message, ConsoleColor.Red);
        }

        /// <summary>
        /// 指定の色でメッセージを出力します。
        /// </summary>
        /// <param name="message">出力するメッセージ</param>
        /// <param name="color">出力する色</param>
        public static void WriteLine(string message, ConsoleColor color)
        {
            Guard.ArgumentNotNullOrEmpty(message, "message");

            var defaultColor = Console.ForegroundColor;
            try
            {
                Console.ForegroundColor = color;
                Console.WriteLine(message);
            }
            finally
            {
                Console.ForegroundColor = defaultColor;
            }
        }
    }
}
