using System;
using System.IO;
using System.Text;

namespace Munyabe.Common
{
    /// <summary>
    /// ロギングに関する機能を提供するクラスです。
    /// </summary>
    public static class LoggerUtil
    {
        private const char TAB = '\t';

        /// <summary>
        /// 例外の詳細メッセージを作成します。
        /// </summary>
        /// <param name="ex">対象の例外</param>
        /// <returns>例外の詳細メッセージ</returns>
        public static string CreateExceptionDetail(Exception ex)
        {
            Guard.ArgumentNotNull(ex, "ex");

            var builder = new StringBuilder();
            builder.AppendLine(string.Format("[Exception] Time : {0}", DateTime.Now));
            AppendExceptionMessage(builder, ex, 0);

            var innerEx = ex.InnerException;
            var level = 1;
            while (innerEx != null)
            {
                builder.AppendLine();
                builder.Append(TAB, level);
                builder.AppendLine("Inner Exception");
                builder.Append(TAB, level);
                builder.AppendLine("---------------");
                AppendExceptionMessage(builder, innerEx, level);

                level++;
                innerEx = innerEx.InnerException;
            }

            return builder.ToString();
        }

        /// <summary>
        /// 例外の詳細を<see cref="StringBuilder"/>に追加します。
        /// </summary>
        private static void AppendExceptionMessage(StringBuilder builder, Exception ex, int level)
        {
            builder.Append(TAB, level);
            builder.AppendLine(string.Format("Type : {0}", ex.GetType().FullName));
            builder.Append(TAB, level);
            builder.AppendLine(string.Format("Message : {0}", ex.Message));
            builder.Append(TAB, level);
            builder.AppendLine(string.Format("Source : {0}", ex.Source));

            if (string.IsNullOrEmpty(ex.StackTrace) == false)
            {
                using (var reader = new StringReader(ex.StackTrace))
                {
                    var line = reader.ReadLine();
                    while (line != null)
                    {
                        builder.Append(TAB, level);
                        builder.AppendLine(line);

                        line = reader.ReadLine();
                    }
                }
            }
        }
    }
}
