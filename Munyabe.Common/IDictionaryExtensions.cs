using System;
using System.Collections.Generic;

namespace Munyabe.Common
{
    /// <summary>
    /// <see cref="IDictionary{TKey, TValue}"/>の拡張メソッドを定義するクラスです。
    /// </summary>
    public static class IDictionaryExtensions
    {
        /// <summary>
        /// キーがまだ存在しない場合に、<see cref="IDictionary{TKey, TValue}"/>にキーと値のペアを追加します。
        /// </summary>
        /// <typeparam name="TKey">キーの型</typeparam>
        /// <typeparam name="TValue">値の型</typeparam>
        /// <param name="dictionary">呼出し対象の<see cref="IDictionary{TKey, TValue}"/>インスタンス</param>
        /// <param name="key">追加する要素のキー</param>
        /// <param name="addValue">キーがまだ存在しない場合に追加する値</param>
        /// <returns>キーの値。キーが<paramref name="dictionary"/>に既に存在する場合はキーの既存の値、キーが存在していなかった場合は<paramref name="addValue"/>の値になります。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dictionary"/>が<see langword="null"/>です。</exception>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue addValue)
        {
            return dictionary.GetOrAdd(key, x => addValue);
        }

        /// <summary>
        /// キーがまだ存在しない場合に、<see cref="IDictionary{TKey, TValue}"/>にキーと値のペアを追加します。
        /// </summary>
        /// <typeparam name="TKey">キーの型</typeparam>
        /// <typeparam name="TValue">値の型</typeparam>
        /// <param name="dictionary">呼出し対象の<see cref="IDictionary{TKey, TValue}"/>インスタンス</param>
        /// <param name="key">追加する要素のキー</param>
        /// <param name="addValueFactory">キーの値を生成するために使用される関数</param>
        /// <returns>キーの値。キーが<paramref name="dictionary"/>に既に存在する場合はキーの既存の値、キーが存在していなかった場合は<paramref name="addValueFactory"/>から返されたキーの新しい値になります。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dictionary"/>または<paramref name="addValueFactory"/>が<see langword="null"/>です。</exception>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> addValueFactory)
        {
            Guard.ArgumentNotNull(dictionary, "dictionary");
            Guard.ArgumentNotNull(addValueFactory, "addValueFactory");

            TValue value;
            if (dictionary.TryGetValue(key, out value) == false)
            {
                value = addValueFactory(key);
                dictionary.Add(key, value);
            }

            return value;
        }

        /// <summary>
        /// キーが存在すればその値を、存在しない場合はデフォルト値を返します。
        /// </summary>
        /// <typeparam name="TKey">キーの型</typeparam>
        /// <typeparam name="TValue">値の型</typeparam>
        /// <param name="dictionary">呼出し対象の<see cref="IDictionary{TKey, TValue}"/>インスタンス</param>
        /// <param name="key">値を取得するキー</param>
        /// <returns>取得する値</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dictionary"/>が<see langword="null"/>です。</exception>
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            Guard.ArgumentNotNull(dictionary, "dictionary");

            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : default(TValue);
        }

        /// <summary>
        /// キーが存在すればその値を、存在しない場合は<paramref name="defalutValue"/>を返します。
        /// </summary>
        /// <typeparam name="TKey">キーの型</typeparam>
        /// <typeparam name="TValue">値の型</typeparam>
        /// <param name="dictionary">呼出し対象の<see cref="IDictionary{TKey, TValue}"/>インスタンス</param>
        /// <param name="key">値を取得するキー</param>
        /// <param name="defalutValue">デフォルト値</param>
        /// <returns>取得する値</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dictionary"/>が<see langword="null"/>です。</exception>
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defalutValue)
        {
            Guard.ArgumentNotNull(dictionary, "dictionary");

            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : defalutValue;
        }

        /// <summary>
        /// キーが存在すればその値を、存在しない場合は<paramref name="defalutValueFactory"/>によって生成された値を返します。
        /// </summary>
        /// <typeparam name="TKey">キーの型</typeparam>
        /// <typeparam name="TValue">値の型</typeparam>
        /// <param name="dictionary">呼出し対象の<see cref="IDictionary{TKey, TValue}"/>インスタンス</param>
        /// <param name="key">値を取得するキー</param>
        /// <param name="defalutValueFactory">デフォルト値を作成するファクトリー</param>
        /// <returns>取得する値</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dictionary"/>または<paramref name="defalutValueFactory"/>が<see langword="null"/>です。</exception>
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> defalutValueFactory)
        {
            Guard.ArgumentNotNull(dictionary, "dictionary");
            Guard.ArgumentNotNull(defalutValueFactory, "defalutValueFactory");

            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : defalutValueFactory(key);
        }
    }
}
