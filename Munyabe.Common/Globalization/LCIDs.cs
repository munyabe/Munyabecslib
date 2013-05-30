using System.Diagnostics.CodeAnalysis;

namespace Munyabe.Common.Globalization
{
    /// <summary>
    /// ロケールIDを定義するクラスです。
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly")]
    public static class LCIDs
    {
        /// <summary>
        /// 英語を表すロケールIDです。
        /// </summary>
        public const int EN = 9;

        /// <summary>
        /// 英語 (米国) を表すロケールIDです。
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        public const int EN_US = 1033;

        /// <summary>
        /// 日本語を表すロケールIDです。
        /// </summary>
        public const int JA = 17;

        /// <summary>
        /// 日本語 (日本) を表すロケールIDです。
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        public const int JA_JP = 1041;

        /// <summary>
        /// 韓国語を表すロケールIDです。
        /// </summary>
        public const int KO = 18;

        /// <summary>
        /// 中国語を表すロケールIDです。
        /// </summary>
        public const int ZH = 30724;

        /// <summary>
        /// 空のロケールを表すロケールIDです。
        /// </summary>
        public const int Empty = 127;
    }
}
