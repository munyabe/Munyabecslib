using System;
using System.Xml.Linq;

namespace Munyabe.Common.Xml
{
    /// <summary>
    /// <see cref="XElement"/>の拡張メソッドを定義するクラスです。
    /// </summary>
    public static class XElementExtensions
    {
        /// <summary>
        /// <c>XML</c>要素から属性の値を取得します。
        /// </summary>
        /// <param name="element"><c>XML</c>要素</param>
        /// <param name="attributeName">取得する属性名</param>
        /// <returns>指定した属性の値</returns>
        /// <exception cref="ArgumentNullException"><paramref name="element"/>が<see langword="null"/>です。</exception>
        /// <exception cref="InvalidOperationException">指定した属性が見つかりません。</exception>
        public static string GetAttributeValue(this XElement element, XName attributeName)
        {
            Guard.ArgumentNotNull(element, "element");

            var attribute = element.Attribute(attributeName);
            if (attribute != null)
            {
                return attribute.Value;
            }
            else
            {
                throw new InvalidOperationException(string.Format("This attribute [{0}] is not found", attributeName));
            }
        }

        /// <summary>
        /// <c>XML</c>要素から属性の値を取得します。
        /// </summary>
        /// <remarks>
        /// 属性が見つからない場合は空文字を返します。
        /// </remarks>
        /// <param name="element"><c>XML</c>要素</param>
        /// <param name="attributeName">取得する属性名</param>
        /// <returns>指定した属性の値</returns>
        public static string GetAttributeValueOrDefault(this XElement element, XName attributeName)
        {
            return GetAttributeValueOrDefault(element, attributeName, string.Empty);
        }

        /// <summary>
        /// <c>XML</c>要素から属性の値を取得します。
        /// </summary>
        /// <remarks>
        /// 属性が見つからない場合は指定したデフォルト値を返します。
        /// </remarks>
        /// <param name="element"><c>XML</c>要素</param>
        /// <param name="attributeName">取得する属性名</param>
        /// <param name="defaultValue">属性が見つからない場合に返すデフォルト値</param>
        /// <returns>指定した属性の値</returns>
        /// <exception cref="ArgumentNullException"><paramref name="element"/>が<see langword="null"/>です。</exception>
        public static string GetAttributeValueOrDefault(this XElement element, XName attributeName, string defaultValue)
        {
            Guard.ArgumentNotNull(element, "element");

            var attribute = element.Attribute(attributeName);
            return attribute != null ? attribute.Value : defaultValue;
        }
    }
}