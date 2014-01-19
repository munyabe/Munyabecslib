using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Munyabe.Common
{
    /// <summary>
    /// オブジェクトの型を変換するユーティリティクラスです。
    /// </summary>
    public static class ConverterUtil
    {
        /// <summary>
        /// 基本データ型のコンバーターを保持するディクショナリーです。
        /// </summary>
        private static Dictionary<Type, Converter<object, object>> _converterMap = new Dictionary<Type, Converter<object, object>>();

        /// <summary>
        /// 静的メンバーを初期化します。
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
        static ConverterUtil()
        {
            _converterMap.Add(typeof(byte), value => Convert.ToByte(value, CultureInfo.InvariantCulture));
            _converterMap.Add(typeof(short), value => Convert.ToInt16(value, CultureInfo.InvariantCulture));
            _converterMap.Add(typeof(int), value => Convert.ToInt32(value, CultureInfo.InvariantCulture));
            _converterMap.Add(typeof(long), value => Convert.ToInt64(value, CultureInfo.InvariantCulture));
            _converterMap.Add(typeof(sbyte), value => Convert.ToSByte(value, CultureInfo.InvariantCulture));
            _converterMap.Add(typeof(ushort), value => Convert.ToUInt16(value, CultureInfo.InvariantCulture));
            _converterMap.Add(typeof(uint), value => Convert.ToUInt32(value, CultureInfo.InvariantCulture));
            _converterMap.Add(typeof(ulong), value => Convert.ToUInt64(value, CultureInfo.InvariantCulture));
            _converterMap.Add(typeof(double), value => Convert.ToDouble(value, CultureInfo.InvariantCulture));
            _converterMap.Add(typeof(decimal), value => Convert.ToDecimal(value, CultureInfo.InvariantCulture));
            _converterMap.Add(typeof(bool), value => Convert.ToBoolean(value, CultureInfo.InvariantCulture));
            _converterMap.Add(typeof(char), value => Convert.ToChar(value, CultureInfo.InvariantCulture));
            _converterMap.Add(typeof(string), value => Convert.ToString(value, CultureInfo.InvariantCulture));
            _converterMap.Add(typeof(DateTime), value => Convert.ToDateTime(value, CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// オブジェクトを基本データ型に変換します。
        /// </summary>
        /// <typeparam name="T">変換後の基本データ型</typeparam>
        /// <param name="value">変換する値</param>
        /// <returns>変換後の値</returns>
        /// <exception cref="NotSupportedException">変換できない型が指定されたときに発生します。</exception>
        public static T ConvertValue<T>(object value) where T : struct
        {
            var type = typeof(T);
            if (_converterMap.ContainsKey(type))
            {
                return (T)_converterMap[type](value);
            }
            else
            {
                throw new NotSupportedException(
                     string.Format("The type [{0}] is not supported.", type.FullName));
            }
        }

        /// <summary>
        /// オブジェクトを指定した型に変換します。
        /// <remarks>変換できなかった場合はデフォルト値を返します。</remarks>
        /// </summary>
        /// <param name="value">変換する値</param>
        /// <param name="type">変換後の型</param>
        /// <returns>変換後の値</returns>
        public static object ConvertValue(object value, Type type)
        {
            if (value == null || value.GetType() == type)
            {
                return value;
            }
            else
            {
                var converter = TypeDescriptor.GetConverter(type);
                return converter.IsValid(value) ? converter.ConvertFrom(value) : type.GetDafaultValue();
            }
        }

        /// <summary>
        /// オブジェクトを指定した型に変換します。
        /// </summary>
        /// <param name="value">変換する値</param>
        /// <param name="type">変換後の型</param>
        /// <param name="convertedValue">変換後の値。変換できなかった場合はデフォルト値になります。</param>
        /// <returns>変換できた場合は<see langword="true"/></returns>
        [SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate")]
        public static bool TryConvertValue(object value, Type type, out object convertedValue)
        {
            if (value == null || value.GetType().Equals(type))
            {
                convertedValue = value;
                return true;
            }

            var converter = TypeDescriptor.GetConverter(type);
            var targetValue = value.ToString();

            if (converter.IsValid(targetValue))
            {
                convertedValue = converter.ConvertFrom(targetValue);
                return true;
            }
            else
            {
                convertedValue = type.GetDafaultValue();
                return false;
            }
        }
    }
}
