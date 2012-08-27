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
        /// <param name="source">比較元のシーケンス</param>
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
        /// <exception cref="ArgumentNullException"><paramref name="source"/>が<see langword="null"/>です。</exception>
        [DebuggerStepThrough]
        public static int CompareCount<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> target)
        {
            Guard.ArgumentNotNull(source, "source");

            if (target == null)
            {
                return 1;
            }
            else if (source == target)
            {
                return 0;
            }

            using (var sourceEnumerator = source.GetEnumerator())
            using (var targetEnumerator = target.GetEnumerator())
            {
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
        }

        /// <summary>
        /// シーケンスの要素数を比較し、相対値を示す値を返します。
        /// </summary>
        /// <typeparam name="TSource">各要素の型</typeparam>
        /// <param name="source">比較元のシーケンス</param>
        /// <param name="count">比較する数</param>
        /// <returns>
        /// 要素数の相対値を示す値
        /// <list type="table">
        ///		<listheader><term>戻り値</term><description>説明</description></listheader>
        ///		<item><term>0より小さい値</term><description>このシーケンスの要素数は<paramref name="count"/>より小さいことを示します。</description></item>
        ///		<item><term>0</term><description>このシーケンスの要素数は<paramref name="count"/>と等しいことを示します。</description></item>
        ///		<item><term>0より大きい値</term><description>このシーケンスの要素数は<paramref count="target"/>より大きいことを示します。</description></item>
        /// </list>
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>が<see langword="null"/>です。</exception>
        public static int CompareCount<TSource>(this IEnumerable<TSource> source, int count)
        {
            Guard.ArgumentNotNull(source, "source");

            if (count < 0)
            {
                return 1;
            }

            var index = 0;
            using (var sourceEnumerator = source.GetEnumerator())
            {
                while (sourceEnumerator.MoveNext())
                {
                    index++;
                    if (count < index)
                    {
                        return 1;
                    }
                }
            }

            return index == count ? 0 : -1;
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
        /// 要素の数が指定の数と一致するかどうかを判断します。
        /// </summary>
        /// <typeparam name="T">各要素の型</typeparam>
        /// <param name="source">カウントする要素が格納されているシーケンス</param>
        /// <param name="count">期待する要素数</param>
        /// <returns>要素の数が指定の数と一致するとき<see langword="true"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>が<see langword="null"/>です。</exception>
        [DebuggerStepThrough]
        public static bool IsCount<T>(this IEnumerable<T> source, int count)
        {
            return CompareCount(source, count) == 0;
        }

        /// <summary>
        /// 要素が重複しているかどうかを判断します。
        /// </summary>
        /// <typeparam name="T">各要素の型</typeparam>
        /// <param name="source">処理を適用する値のシーケンス</param>
        /// <returns>要素が重複しているとき<see langword="true"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>が<see langword="null"/>です。</exception>
        [DebuggerStepThrough]
        public static bool IsOverlapped<T>(this IEnumerable<T> source)
        {
            Guard.ArgumentNotNull(source, "source");

            var set = new HashSet<T>();
            foreach (T each in source)
            {
                if (set.Contains(each))
                {
                    return true;
                }
                else
                {
                    set.Add(each);
                }
            }

            return false;
        }

        /// <summary>
        /// 要素が重複しているかどうかを判断します。
        /// </summary>
        /// <typeparam name="T">各要素の型</typeparam>
        /// <param name="source">処理を適用する値のシーケンス</param>
        /// <param name="element">重複した要素（重複がない場合はデフォルト値になります）</param>
        /// <returns>要素が重複しているとき<see langword="true"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>が<see langword="null"/>です。</exception>
        [DebuggerStepThrough]
        public static bool IsOverlapped<T>(this IEnumerable<T> source, out T element)
        {
            Guard.ArgumentNotNull(source, "source");

            var set = new HashSet<T>();
            foreach (T each in source)
            {
                if (set.Contains(each))
                {
                    element = each;
                    return true;
                }
                else
                {
                    set.Add(each);
                }
            }

            element = default(T);
            return false;
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
        /// 要素が1種類かどうかを判断します。
        /// </summary>
        /// <typeparam name="T">各要素の型</typeparam>
        /// <param name="source">処理を適用する値のシーケンス</param>
        /// <returns>要素が1種類のとき<see langword="true"/>。要素が空の場合は<see langword="false"/>になります。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>が<see langword="null"/>です。
        /// </exception>
        [DebuggerStepThrough]
        public static bool IsSingleKind<T>(this IEnumerable<T> source)
        {
            Guard.ArgumentNotNull(source, "source");

            using (var enumerator = source.GetEnumerator())
            {
                if (enumerator.MoveNext() == false)
                {
                    return false;
                }

                T first = enumerator.Current;
                while (enumerator.MoveNext())
                {
                    var current = enumerator.Current;
                    if (first == null)
                    {
                        if (current != null)
                        {
                            return false;
                        }
                    }
                    else if (first.Equals(current) == false)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 重複している要素を返します。
        /// </summary>
        /// <typeparam name="T">各要素の型</typeparam>
        /// <param name="source">処理を適用する値のシーケンス</param>
        /// <returns>重複している要素のシーケンス</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>が<see langword="null"/>です。</exception>
        [DebuggerStepThrough]
        public static IEnumerable<T> Overlapped<T>(this IEnumerable<T> source)
        {
            Guard.ArgumentNotNull(source, "source");

            var exists = new Dictionary<T, bool>();
            foreach (T each in source)
            {
                bool isExist;
                if (exists.TryGetValue(each, out isExist))
                {
                    if (isExist == false)
                    {
                        exists[each] = true;
                        yield return each;
                    }
                }
                else
                {
                    exists.Add(each, false);
                }
            }
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