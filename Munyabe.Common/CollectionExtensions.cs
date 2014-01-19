using System.Collections.ObjectModel;

namespace Munyabe.Common
{
    /// <summary>
    /// <see cref="Collection{T}"/>の拡張メソッドを定義するクラスです。
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// <see cref="Collection{T}"/>の指定した範囲外にある要素を削除します。
        /// </summary>
        /// <typeparam name="T">各要素の型</typeparam>
        /// <param name="source">処理を適用する<see cref="Collection{T}"/></param>
        /// <param name="endIndex">このインスタンス内の部分要素の 0 から始まる終了要素位置</param>
        public static void RemoveOutRange<T>(this Collection<T> source, int endIndex)
        {
            Guard.ArgumentNotNull(source, "source");

            RemoveRangeHelper(source, endIndex, source.Count);
        }

        /// <summary>
        /// <see cref="Collection{T}"/>の指定した範囲外にある要素を削除します。
        /// </summary>
        /// <typeparam name="T">各要素の型</typeparam>
        /// <param name="source">処理を適用する<see cref="Collection{T}"/></param>
        /// <param name="startIndex">このインスタンス内の部分要素の 0 から始まる開始要素位置</param>
        /// <param name="length">部分要素の要素数</param>
        public static void RemoveOutRange<T>(this Collection<T> source, int startIndex, int length)
        {
            Guard.ArgumentNotNull(source, "source");

            RemoveRangeHelper(source, 0, startIndex);
            RemoveRangeHelper(source, length, source.Count);
        }

        /// <summary>
        /// <see cref="Collection{T}"/>の指定した範囲にある要素を削除します。
        /// </summary>
        /// <typeparam name="T">各要素の型</typeparam>
        /// <param name="source">処理を適用する<see cref="Collection{T}"/></param>
        /// <param name="startIndex">このインスタンス内の削除する要素の 0 から始まる開始要素位置</param>
        /// <param name="length">削除する要素数</param>
        public static void RemoveRange<T>(this Collection<T> source, int startIndex, int length)
        {
            Guard.ArgumentNotNull(source, "source");

            RemoveRangeHelper(source, startIndex, startIndex + length);
        }

        /// <summary>
        /// <see cref="Collection{T}"/>の指定した範囲にある要素を削除するヘルパーメソッドです。
        /// </summary>
        private static void RemoveRangeHelper<T>(Collection<T> source, int startIndex, int endIndex)
        {
            int currentIndex = endIndex;
            while (startIndex < currentIndex)
            {
                currentIndex--;
                source.RemoveAt(currentIndex);
            }
        }
    }
}