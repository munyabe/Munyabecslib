using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Munyabe.Common.DataAnnotations.DataAnnotations
{
    /// <summary>
    /// 検証属性の<see cref="CustomAttributeBuilder"/>を作成する機能を提供するクラスです。
    /// </summary>
    public static class ValidationAttributeBuilder
    {
        /// <summary>
        /// <see cref="RangeAttribute"/>の<c>Builder</c>を作成します。
        /// </summary>
        /// <param name="minimum">値の最小値</param>
        /// <param name="maximum">値の最大値</param>
        /// <param name="errorMessage">エラーメッセージ</param>
        /// <returns><see cref="RangeAttribute"/>の<c>Builder</c></returns>
        public static CustomAttributeBuilder CreateRangeBuilder(int minimum, int maximum, string errorMessage)
        {
            var type = typeof(RangeAttribute);
            return new CustomAttributeBuilder(
                type.GetConstructor(new[] { typeof(int), typeof(int) }),
                new object[] { minimum, maximum },
                GetNamedProperties(type),
                new object[] { minimum, maximum, typeof(int), errorMessage, null, null });
        }

        /// <summary>
        /// <see cref="RangeAttribute"/>の<c>Builder</c>を作成します。
        /// </summary>
        /// <param name="targetType">検証するオブジェクトの型</param>
        /// <param name="minimum">値の最小値</param>
        /// <param name="maximum">値の最大値</param>
        /// <param name="errorMessage">エラーメッセージ</param>
        /// <returns><see cref="RangeAttribute"/>の<c>Builder</c></returns>
        public static CustomAttributeBuilder CreateRangeBuilder(Type targetType, string minimum, string maximum, string errorMessage)
        {
            var type = typeof(RangeAttribute);
            return new CustomAttributeBuilder(
                type.GetConstructor(new[] { typeof(Type), typeof(string), typeof(string) }),
                new object[] { targetType, minimum, maximum },
                GetNamedProperties(type),
                new object[] { minimum, maximum, targetType, errorMessage, null, null });
        }

        /// <summary>
        /// <see cref="RegularExpressionAttribute"/>の<c>Builder</c>を作成します。
        /// </summary>
        /// <param name="pattern">検証に使用する正規表現</param>
        /// <param name="errorMessage">エラーメッセージ</param>
        /// <returns><see cref="RegularExpressionAttribute"/>の<c>Builder</c></returns>
        public static CustomAttributeBuilder CreateRegularExpressionBuilder(string pattern, string errorMessage)
        {
            var type = typeof(RegularExpressionAttribute);
            return new CustomAttributeBuilder(
                type.GetConstructor(new[] { typeof(string) }),
                new object[] { pattern },
                GetNamedProperties(type),
                new object[] { pattern, errorMessage, null, null });
        }

        /// <summary>
        /// <see cref="RequiredAttribute"/>の<c>Builder</c>を作成します。
        /// </summary>
        /// <param name="errorMessage">エラーメッセージ</param>
        /// <returns><see cref="RequiredAttribute"/>の<c>Builder</c></returns>
        public static CustomAttributeBuilder CreateRequiredBuilder(string errorMessage)
        {
            var type = typeof(RequiredAttribute);
            return new CustomAttributeBuilder(
                type.GetConstructor(Type.EmptyTypes),
                new object[] { },
                GetNamedProperties(type),
                new object[] { false, errorMessage, null, null });
        }

        /// <summary>
        /// <see cref="StringLengthAttribute"/>の<c>Builder</c>を作成します。
        /// </summary>
        /// <param name="minimum">文字列の最小長</param>
        /// <param name="maximum">文字列の最大長</param>
        /// <param name="errorMessage">エラーメッセージ</param>
        /// <returns><see cref="StringLengthAttribute"/>の<c>Builder</c></returns>
        public static CustomAttributeBuilder CreateStringLengthBuilder(int minimum, int maximum, string errorMessage)
        {
            var type = typeof(StringLengthAttribute);
            return new CustomAttributeBuilder(
                type.GetConstructor(new[] { typeof(int) }),
                new object[] { maximum },
                GetNamedProperties(type),
                new object[] { maximum, minimum, errorMessage, null, null });
        }

        /// <summary>
        /// 名前付きプロパティを取得します。
        /// </summary>
        /// <param name="type">プロパティを取得する型</param>
        /// <returns>名前付きプロパティ</returns>
        private static PropertyInfo[] GetNamedProperties(Type type)
        {
            return type.GetProperties().Where(prop => prop.CanRead && prop.CanWrite).ToArray();
        }
    }
}