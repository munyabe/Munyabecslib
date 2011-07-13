using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Munyabe.Common.Algorithms;

namespace Munyabe.Common
{
    /// <summary>
    /// <see cref="IEnumerable{T}"/>の拡張メソッドを定義するクラスです。
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// シーケンスの要素数を比較し、相対値を示す値を返します。
        /// </summary>
        /// <typeparam name="TSource">各要素の型</typeparam>
        /// <param name="source">比較基のシーケンス</param>
        /// <param name="target">比較対象のシーケンス</param>
        /// <returns>
        /// 要素数の相対値を示す値
        /// <list type="table">
        ///		<listheader><term>戻り値</term><description>説明</description></listheader>
        ///		<item><term>0より小さい値</term><description>このシーケンスの要素数は<paramref name="target"/>より小さいことを示します。</description></item>
        ///		<item><term>0</term><description>このシーケンスの要素数は<paramref name="target"/>と等しいことを示します。</description></item>
        ///		<item><term>0より大きい値</term><description>このシーケンスの要素数は<paramref name="target"/>より大きいことを示します。</description></item>
        /// </list>
        /// </returns>
        [DebuggerStepThrough]
        public static int CompareCount<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> target)
        {
            if (source == target)
            {
                return 0;
            }
            else if (target == null)
            {
                return 1;
            }
            else if (source == null)
            {
                return -1;
            }

            var sourceEnumerator = source.GetEnumerator();
            var targetEnumerator = target.GetEnumerator();

            bool existSource;
            bool existTarget;

            while (true)
            {
                existSource = sourceEnumerator.MoveNext();
                existTarget = targetEnumerator.MoveNext();

                if (existSource == false)
                {
                    return existTarget ? -1 : 0;
                }
                else if (existTarget == false)
                {
                    return 1;
                }
            }
        }

        /// <summary>
        /// 指定された<paramref name="keySelector"/>を使用して取得した値を比較することにより、シーケンスから一意の要素を返します。
        /// </summary>
        /// <typeparam name="TSource">各要素の型</typeparam>
        /// <typeparam name="TKey">比較する値の型</typeparam>
        /// <param name="source">重複する要素を削除する対象となるシーケンス</param>
        /// <param name="keySelector">比較する値に変換する関数</param>
        /// <returns>シーケンスの一意の要素を格納する<see cref="IEnumerable{T}"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>または<paramref name="keySelector"/>が<see langword="null"/>です。</exception>
        [DebuggerStepThrough]
        public static IEnumerable<TSource> Distinct<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            Guard.ArgumentNotNull(source, "source");
            Guard.ArgumentNotNull(keySelector, "selector");

            return source.Distinct(new SelectionComparer<TSource, TKey>(keySelector));
        }

        /// <summary>
        /// <see cref="IEnumerable{T}"/>の各要素に対して、指定された処理を実行します。
        /// </summary>
        /// <typeparam name="T">各要素の型</typeparam>
        /// <param name="source">処理を適用する値のシーケンス</param>
        /// <param name="action">各要素に対して実行するアクション</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>または<paramref name="action"/>が<see langword="null"/>です。</exception>
        [DebuggerStepThrough]
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            Guard.ArgumentNotNull(source, "source");
            Guard.ArgumentNotNull(action, "action");

            foreach (T each in source)
            {
                action(each);
            }
        }

        /// <summary>
        /// <see cref="IEnumerable{T}"/>の各要素に対して、インデックスを利用して指定された処理を実行します。
        /// </summary>
        /// <typeparam name="T">各要素の型</typeparam>
        /// <param name="source">処理を適用する値のシーケンス</param>
        /// <param name="action">各要素に対して実行するアクション。<c>int</c>パラメーターはアクセスインデックスです。</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>または<paramref name="action"/>が<see langword="null"/>です。</exception>
        [DebuggerStepThrough]
        public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            Guard.ArgumentNotNull(source, "source");
            Guard.ArgumentNotNull(action, "action");

            int i = 0;
            foreach (T each in source)
            {
                action(each, i++);
            }
        }

        /// <summary>
        /// <see cref="IEnumerable{T}"/>の要素が単一であるかどうかを判断します。
        /// </summary>
        /// <typeparam name="T">各要素の型</typeparam>
        /// <param name="source">処理を適用する値のシーケンス</param>
        /// <returns>要素が単一のとき<see langword="true"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>が<see langword="null"/>です。</exception>
        [DebuggerStepThrough]
        public static bool IsSingle<T>(this IEnumerable<T> source)
        {
            Guard.ArgumentNotNull(source, "source");

            int count = 0;
            foreach (T each in source)
            {
                count++;
                if (count == 2)
                {
                    break;
                }
            }

            return count == 1;
        }

        /// <summary>
        /// <see cref="IEnumerable{T}"/>の要素が単一であるかどうかを判断します。
        /// </summary>
        /// <typeparam name="T">各要素の型</typeparam>
        /// <param name="source">処理を適用する値のシーケンス</param>
        /// <param name="predicate">各要素が条件を満たしているかどうかをテストする関数</param>
        /// <returns>要素が単一のとき<see langword="true"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>が<see langword="null"/>です。</exception>
        [DebuggerStepThrough]
        public static bool IsSingle<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            Guard.ArgumentNotNull(source, "source");
            Guard.ArgumentNotNull(predicate, "predicate");

            int count = 0;
            foreach (T each in source)
            {
                if (predicate(each))
                {
                    count++;
                    if (count == 2)
                    {
                        break;
                    }
                }
            }

            return count == 1;
        }

        /// <summary>
        /// <see cref="IEnumerable{T}"/>の各要素と子要素を再帰的に列挙します。アルゴリズムは深さ優先探索になります。
        /// </summary>
        /// <typeparam name="T">各要素の型</typeparam>
        /// <param name="source">処理を適用する値のシーケンス</param>
        /// <param name="getChildren">親要素から子要素の集合を取得する処理</param>
        /// <returns>走査する親要素</returns>
        /// <exception cref="ArgumentNullException"><paramref name="getChildren"/>が<see langword="null"/>です。</exception>
        [DebuggerStepThrough]
        public static IEnumerable<T> Recursive<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> getChildren)
        {
            return Recursive(source, getChildren, GraphSearchAlgorithm.DepthFirstSearch);
        }

        /// <summary>
        /// <see cref="IEnumerable{T}"/>の各要素と子要素を再帰的に列挙します。
        /// </summary>
        /// <typeparam name="T">各要素の型</typeparam>
        /// <param name="source">処理を適用する値のシーケンス</param>
        /// <param name="getChildren">親要素から子要素の集合を取得する処理</param>
        /// <param name="searchAlgorithm">探索アルゴリズム</param>
        /// <returns>走査する親要素</returns>
        /// <exception cref="ArgumentNullException"><paramref name="getChildren"/>が<see langword="null"/>です。</exception>
        [DebuggerStepThrough]
        public static IEnumerable<T> Recursive<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> getChildren, GraphSearchAlgorithm searchAlgorithm)
        {
            Guard.ArgumentNotNull(getChildren, "getChildren");
            if (source == null)
            {
                yield break;
            }

            switch (searchAlgorithm)
            {
                case GraphSearchAlgorithm.BreadthFirstSearch:
                    {
                        foreach (var item in source)
                        {
                            yield return item;
                        }
                        foreach (var item in source)
                        {
                            var results = GraphSearch.SearchBreadthFirst(item, getChildren);
                            foreach (var result in results)
                            {
                                yield return result;
                            }
                        }

                        yield break;
                    }
                case GraphSearchAlgorithm.DepthFirstSearch:
                    {
                        foreach (var item in source)
                        {
                            yield return item;

                            var results = GraphSearch.SearchDepthFirst(item, getChildren);
                            foreach (var result in results)
                            {
                                yield return result;
                            }
                        }

                        yield break;
                    }
                default:
                    {
                        throw new NotSupportedException(string.Format("This algorithm [{0}] is not supported", searchAlgorithm));
                    }
            }
        }

        /// <summary>
        /// <see cref="IEnumerable{T}"/>から<see cref="HashSet{T}"/>を作成します。
        /// </summary>
        /// <typeparam name="T">各要素の型</typeparam>
        /// <param name="source">処理を適用する値のシーケンス</param>
        /// <returns>作成した<see cref="HashSet{T}"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>が<see langword="null"/>です。</exception>
        [DebuggerStepThrough]
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            Guard.ArgumentNotNull(source, "source");

            return new HashSet<T>(source);
        }

        /// <summary>
        /// <see cref="IEnumerable{T}"/>から<see cref="HashSet{T}"/>を作成します。
        /// </summary>
        /// <typeparam name="TSource">各要素の型</typeparam>
        /// <typeparam name="TResult">変換後の要素の型</typeparam>
        /// <param name="source">処理を適用する値のシーケンス</param>
        /// <param name="selector">各要素に適用する変換関数</param>
        /// <returns>作成した<see cref="HashSet{T}"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>または<paramref name="selector"/>が<see langword="null"/>です。</exception>
        [DebuggerStepThrough]
        public static HashSet<TResult> ToHashSet<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            Guard.ArgumentNotNull(source, "source");
            Guard.ArgumentNotNull(selector, "selector");

            return source.Select(selector).ToHashSet();
        }

        /// <summary>
        /// 各要素に対して副作用を与えつつ要素の一覧を返します。
        /// </summary>
        /// <typeparam name="T">各要素の型</typeparam>
        /// <param name="source">処理を適用する値のシーケンス</param>
        /// <param name="action">各要素に対して実行するアクション</param>
        /// <returns>要素の一覧</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>または<paramref name="action"/>が<see langword="null"/>です。</exception>
        [DebuggerStepThrough]
        public static IEnumerable<T> With<T>(this IEnumerable<T> source, Action<T> action)
        {
            Guard.ArgumentNotNull(source, "source");
            Guard.ArgumentNotNull(action, "action");

            foreach (var value in source)
            {
                action(value);
                yield return value;
            }
        }

        /// <summary>
        /// 変換した値から2つのオブジェクトが等しいかどうかを比較します。
        /// </summary>
        private class SelectionComparer<TSource, TKey> : IEqualityComparer<TSource>
        {
            /// <summary>
            /// 比較する値に変換する関数です。
            /// </summary>
            private Func<TSource, TKey> _keySelector;

            /// <summary>
            /// インスタンスを初期化します。
            /// </summary>
            public SelectionComparer(Func<TSource, TKey> keySelector)
            {
                Guard.ArgumentNotNull(keySelector, "selector");

                _keySelector = keySelector;
            }

            /// <summary>
            /// 指定したオブジェクトが等しいかどうかを判断します。
            /// </summary>
            public bool Equals(TSource x, TSource y)
            {
                var xKey = _keySelector(x);
                var yKey = _keySelector(y);

                if (xKey == null)
                {
                    return yKey == null;
                }

                return xKey.Equals(yKey);
            }

            /// <summary>
            /// 指定したオブジェクトのハッシュコードを取得します。
            /// </summary>
            public int GetHashCode(TSource obj)
            {
                var key = _keySelector(obj);
                return key != null ? key.GetHashCode() : 0;
            }
        }
    }
}