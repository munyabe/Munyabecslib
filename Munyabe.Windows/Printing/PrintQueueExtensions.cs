using System.Printing;
using Munyabe.Common;

namespace Munyabe.Windows.Printing
{
    /// <summary>
    /// <see cref="PrintQueue"/>の拡張メソッドを定義するクラスです。
    /// </summary>
    public static class PrintQueueExtensions
    {
        /// <summary>
        /// プリンターのドライバーが Microsoft XPS Document Writer かどうかを判定します。
        /// </summary>
        /// <remarks>
        /// Microsoft XPS Document Writer では<see cref="PrintQueue.IsXpsDevice"/>は<see langword="false"/>となるため判別できません。
        /// ※ 参考
        /// http://stackoverflow.com/questions/5043761/intercepting-printdialog-to-xps-document-writer%EF%BC%89
        /// </remarks>
        /// <param name="source">判定する印刷のキュー</param>
        /// <returns>Microsoft XPS Document Writer の場合は<see langword="true"/></returns>
        public static bool IsXpsDocumentWriter(this PrintQueue source)
        {
            Guard.ArgumentNotNull(source, "source");

            // MEMO : 他に適切な判別方法が無いため、ドライバ名で判別します。
            return source.QueueDriver.Name == "Microsoft XPS Document Writer";
        }
    }
}
