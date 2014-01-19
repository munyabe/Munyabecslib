using System;
using System.Collections.Generic;
using System.Linq;

namespace Munyabe.Common.Algorithms
{
    /// <summary>
    /// グラフ探索のアルゴリズムを提供するクラスです。
    /// </summary>
    public static class GraphSearch
    {
        /// <summary>
        /// グラフ構造を幅優先探索します。
        /// </summary>
        public static IEnumerable<T> SearchBreadthFirst<T>(T source, Func<T, IEnumerable<T>> getChildren)
        {
            if (source == null)
            {
                yield break;
            }

            var queue = new Queue<T>();
            Action<T> addChild = item =>
                getChildren(item)
                    .ForEach(queue.Enqueue);

            addChild(source);

            while (queue.Any())
            {
                var current = queue.Dequeue();
                T target = current;
                if (target != null)
                {
                    yield return target;
                }

                addChild(current);
            }
        }

        /// <summary>
        /// グラフ構造を深さ優先探索します。
        /// </summary>
        public static IEnumerable<T> SearchDepthFirst<T>(T source, Func<T, IEnumerable<T>> getChildren)
        {
            if (source == null)
            {
                yield break;
            }

            var stack = new Stack<T>();
            Action<T> addChild = item =>
                getChildren(item)
                    .Reverse()
                    .ForEach(stack.Push);

            addChild(source);

            while (stack.Any())
            {
                var current = stack.Pop();
                T target = current;
                if (target != null)
                {
                    yield return target;
                }

                addChild(current);
            }
        }
    }
}