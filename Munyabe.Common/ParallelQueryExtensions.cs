using System;
using System.Linq;
using System.Threading;

namespace Munyabe.Common
{
    /// <summary>
    /// <see cref="ParallelQuery{T}"/>の拡張メソッドを定義するクラスです。
    /// </summary>
    public static class ParallelQueryExtensions
    {
        /// <summary>
        /// Parallel LINQ で実行されるスレッドに現在のカルチャを設定します。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="source"/>の要素の型</typeparam>
        /// <param name="source">対象の<see cref="ParallelQuery{T}"/></param>
        /// <returns>現在のスレッドが設定された<see cref="ParallelQuery{T}"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>が<see langword="null"/>です。</exception>
        public static ParallelQuery<TSource> AsCurrentCulture<TSource>(this ParallelQuery<TSource> source)
        {
            Guard.ArgumentNotNull(source, "source");

            var currentCulture = Thread.CurrentThread.CurrentCulture;
            return source.Select(each =>
                {
                    Thread.CurrentThread.CurrentCulture = currentCulture;
                    Thread.CurrentThread.CurrentUICulture = currentCulture;
                    return each;
                });
        }
    }
}
