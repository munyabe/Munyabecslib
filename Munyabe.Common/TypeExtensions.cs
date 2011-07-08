using System;
using System.Collections.Generic;

namespace Munyabe.Common
{
    /// <summary>
    /// <see cref="Type"/>の拡張メソッドを定義するクラスです。
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// 現在の<see cref="Type"/>で継承している全ての親クラスを取得します。
        /// </summary>
        /// <param name="type">現在の<see cref="Type"/></param>
        /// <param name="containsSelf">戻り値に自身を含める場合は<c>true</c></param>
        /// <returns>継承している全ての親クラス</returns>
        public static IEnumerable<Type> GetBaseTypes(this Type type, bool containsSelf = false)
        {
            Guard.ArgumentNotNull(type, "type");

            if (containsSelf)
            {
                yield return type;
            }

            var currentType = type.BaseType;
            while (currentType != null)
            {
                yield return currentType;
                currentType = currentType.BaseType;
            }
        }

        /// <summary>
        /// デフォルト値を取得します。
        /// </summary>
        /// <param name="type">デフォルト値を取得する型</param>
        /// <returns>デフォルト値</returns>
        public static object GetDafaultValue(this Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        /// <summary>
        /// 値型を<see langword="null"/>許容型に変換します。
        /// </summary>
        /// <remarks>
        /// 参照型が指定された場合は元の型を返します。
        /// </remarks>
        /// <param name="type"><see langword="null"/>許容型の基になる値型</param>
        /// <returns>指定された値型の<see langword="null"/>許容型</returns>
        public static Type ToNullableType(this Type type)
        {
            return type.IsValueType ? typeof(Nullable<>).MakeGenericType(type) : type;
        }
    }
}