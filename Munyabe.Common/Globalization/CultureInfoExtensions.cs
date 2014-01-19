using System;
using System.Globalization;

namespace Munyabe.Common.Globalization
{
    /// <summary>
    /// <see cref="CultureInfo"/>の拡張メソッドを定義するクラスです。
    /// </summary>
    public static class CultureInfoExtensions
    {
        /// <summary>
        /// カルチャが一致するかどうかを判定します。
        /// </summary>
        /// <param name="source">比較元のカルチャ</param>
        /// <param name="target">比較先のカルチャ</param>
        /// <returns>カルチャが一致するとき<see langword="true"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>または<paramref name="target"/>が<see langword="null"/>です。</exception>
        public static bool IsMatchCulture(this CultureInfo source, CultureInfo target)
        {
            Guard.ArgumentNotNull(source, "source");
            Guard.ArgumentNotNull(target, "target");

            if (source.IsNeutralCulture)
            {
                if (target.IsNeutralCulture == false)
                {
                    return source.Name == target.TwoLetterISOLanguageName;
                }
            }
            else
            {
                if (target.IsNeutralCulture)
                {
                    return false;
                }
            }

            return source.LCID == target.LCID;
        }

        /// <summary>
        /// 全角をサポートするカルチャかどうかを判定します。
        /// </summary>
        /// <param name="culture">判定するカルチャ</param>
        /// <returns>全角をサポートするカルチャのときは<see langword="true"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="culture"/>が<see langword="null"/>です。</exception>
        public static bool IsTwoByteCulture(this CultureInfo culture)
        {
            Guard.ArgumentNotNull(culture, "culture");

            var parentCulture = GetRootCultureInternal(culture);
            switch (parentCulture.LCID)
            {
                case LCIDs.JA:
                case LCIDs.ZH:
                case LCIDs.KO:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// ルートのカルチャを取得します。
        /// </summary>
        /// <param name="culture">ルートを取得するカルチャ</param>
        /// <returns>ルートのカルチャ</returns>
        /// <exception cref="ArgumentNullException"><paramref name="culture"/>が<see langword="null"/>です。</exception>
        public static CultureInfo GetRootCulture(this CultureInfo culture)
        {
            Guard.ArgumentNotNull(culture, "culture");
            return GetRootCultureInternal(culture);
        }

        /// <summary>
        /// ルートのカルチャを取得する内部メソッドです。
        /// </summary>
        private static CultureInfo GetRootCultureInternal(CultureInfo culture)
        {
            var result = culture;
            while (result.Parent.LCID != LCIDs.Empty)
            {
                result = result.Parent;
            }

            return result;
        }
    }
}