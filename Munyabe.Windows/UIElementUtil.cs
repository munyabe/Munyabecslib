using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Munyabe.Common.Algorithms;

namespace Munyabe.Windows
{
    /// <summary>
    /// <see cref="UIElement"/>に関するユーティリティクラスです。
    /// </summary>
    public static class UIElementUtil
    {
        /// <summary>
        /// 指定した論理ツリーを子方向に走査し、指定の型のオブジェクトを見つけます。
        /// 見つからない場合には空のコレクションを返却します。
        /// </summary>
        /// <remarks>デフォルトの探索アルゴリズムは深さ優先探索です。</remarks>
        /// <typeparam name="T">取得するオブジェクトの型</typeparam>
        /// <param name="element">走査する親ビジュアル</param>
        /// <param name="searchAlgorithm">探索アルゴリズム</param>
        /// <returns>親ビジュアルに含まれる子要素</returns>
        public static IEnumerable<T> FindLogicalChildren<T>(
            Visual element, GraphSearchAlgorithm searchAlgorithm = GraphSearchAlgorithm.DepthFirstSearch)
            where T : class
        {
            return SearchTreeHelper<T>(LogicalTreeHelper.GetChildren, element, searchAlgorithm);
        }

        /// <summary>
        /// 指定したビジュアルツリーを子方向に走査し、指定の型のオブジェクトを見つけます。
        /// 見つからない場合には空のコレクションを返却します。
        /// </summary>
        /// <remarks>デフォルトの探索アルゴリズムは深さ優先探索です。</remarks>
        /// <typeparam name="T">取得するオブジェクトの型</typeparam>
        /// <param name="element">走査する親ビジュアル</param>
        /// <param name="searchAlgorithm">探索アルゴリズム</param>
        /// <returns>親ビジュアルに含まれる子要素</returns>
        public static IEnumerable<T> FindVisualChildren<T>(
            Visual element, GraphSearchAlgorithm searchAlgorithm = GraphSearchAlgorithm.DepthFirstSearch)
            where T : class
        {
            return SearchTreeHelper<T>(GetVisualTreeChildren, element, searchAlgorithm);
        }

        /// <summary>
        /// エレメントのビジュアルツリーを親方向に走査し、指定の型のオブジェクトを見つけます。
        /// </summary>
        /// 見つからない場合には<c>null</c>を返却します。
        /// <remarks>走査中に<see cref="Adorner"/>が出現した場合には、<see cref="Adorner.AdornedElement"/>を辿ります。</remarks>
        /// <typeparam name="T">取得するオブジェクトの型</typeparam>
        public static T FindVisualParent<T>(Visual element)
            where T : class
        {
            if (element == null)
            {
                return null;
            }

            var parent = VisualTreeHelper.GetParent(element);
            T result = null;
            while (parent != null && result == null)
            {
                result = parent as T;
                if (result == null)
                {
                    var adorner = parent as Adorner;
                    if (adorner != null)
                    {
                        parent = adorner.AdornedElement;
                    }
                    else
                    {
                        parent = VisualTreeHelper.GetParent(parent);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 指定したビジュアルオブジェクトに含まれる子要素を取得します。
        /// </summary>
        /// <param name="element">走査する親ビジュアル</param>
        /// <returns>親ビジュアルに含まれる子要素</returns>
        public static IEnumerable<DependencyObject> GetVisualTreeChildren(DependencyObject element)
        {
            int childCount = VisualTreeHelper.GetChildrenCount(element);
            for (int childIndex = 0; childIndex < childCount; childIndex++)
            {
                yield return VisualTreeHelper.GetChild(element, childIndex);
            }
        }

        /// <summary>
        /// 指定された位置のヒットテストを行います。
        /// ヒットしたオブジェクトからビジュアルツリーを親方向に辿り、指定された型のオブジェクトを取得します。
        /// 返却されるオブジェクトは、最前面にあるオブジェクトもしくはそのビジュアル親要素となります。
        /// </summary>
        /// <typeparam name="T">取得したいエレメントの型</typeparam>
        /// <param name="uielement">対象のUIElement</param>
        /// <param name="point">ヒットテストを行う位置</param>
        /// <returns>ヒットテスト結果</returns>
        public static T HitTest<T>(this UIElement uielement, Point point)
            where T : class
        {
            T result = null;
            uielement.HitTest(new PointHitTestParameters(point), element =>
            {
                return (result = element as T) != null;
            });

            return result;
        }

        /// <summary>
        /// 指定された範囲のヒットテストを行います。
        /// ヒットしたオブジェクトからビジュアルツリーを親方向に辿り、指定された型のオブジェクトを取得します。
        /// 返却されるオブジェクトは、最前面にあるオブジェクトもしくはそのビジュアル親要素となります。
        /// </summary>
        /// <typeparam name="T">取得したいエレメントの型</typeparam>
        /// <param name="uielement">対象のUIElement</param>
        /// <param name="geometry">ヒットテストを行うジオメトリ</param>
        /// <returns>ヒットテスト結果</returns>
        public static T HitTest<T>(this UIElement uielement, Geometry geometry)
            where T : class
        {
            T result = null;
            uielement.HitTest(new GeometryHitTestParameters(geometry), element =>
            {
                return (result = element as T) != null;
            });

            return result;
        }

        /// <summary>
        /// 指定された範囲のヒットテストを行います。
        /// ヒットしたオブジェクトからビジュアルツリーを親方向に辿り、指定された型のオブジェクトを取得します。
        /// 返却されるコレクションは、前面にあるものから順に格納されます。
        /// </summary>
        /// <typeparam name="T">取得したいエレメントの型</typeparam>
        /// <param name="uielement">対象のUIElement</param>
        /// <param name="point">ヒットテストを行う位置</param>
        /// <returns>ヒットテスト結果</returns>
        public static IEnumerable<T> HitTestAll<T>(this UIElement uielement, Point point)
            where T : class
        {
            var result = new HashSet<T>();
            uielement.HitTest(new PointHitTestParameters(point), element =>
            {
                var castedElement = element as T;
                if (castedElement != null)
                {
                    result.Add(castedElement);
                }
                return false;
            });

            return result;
        }

        /// <summary>
        /// 指定された範囲のヒットテストを行います。
        /// ヒットしたオブジェクトからビジュアルツリーを親方向に辿り、指定された型のオブジェクトを取得します。
        /// 返却されるコレクションは、前面にあるものから順に格納されます。
        /// </summary>
        /// <typeparam name="T">取得したいエレメントの型</typeparam>
        /// <param name="uielement">対象のUIElement</param>
        /// <param name="geometry">ヒットテストを行うジオメトリ</param>
        /// <returns>ヒットテスト結果</returns>
        public static IEnumerable<T> HitTestAll<T>(this UIElement uielement, Geometry geometry)
            where T : class
        {
            var result = new HashSet<T>();
            uielement.HitTest(new GeometryHitTestParameters(geometry), element =>
            {
                var castedElement = element as T;
                if (castedElement != null)
                {
                    result.Add(castedElement);
                }
                return false;
            });

            return result;
        }

        /// <summary>
        /// 指定された位置のヒットテストを行います。
        /// ヒットしたオブジェクトから、ビジュアルツリーをさかのぼってエレメントを取得します。
        /// 全てのヒットしたオブジェクトの走査が完了するか、<paramref name="hitFunction"/>の結果が<c>True</c>を返却するまでヒットテストは継続します。
        /// </summary>
        /// <param name="uielement">対象のUIElement</param>
        /// <param name="hittestParameters">ヒットテストを行う位置・範囲を示すパラメータ</param>
        /// <param name="hitFunction">ヒットしたエレメントに対して実行するファンクション</param>
        public static void HitTest(this UIElement uielement, HitTestParameters hittestParameters, Func<DependencyObject, bool> hitFunction)
        {
            HitTestResultCallback hitTestResultCallback = hitResult =>
            {
                var checkVisual = hitResult.VisualHit as UIElement;
                if (checkVisual != null
                    && (!checkVisual.IsHitTestVisible || checkVisual.Visibility == Visibility.Collapsed))
                {
                    return HitTestResultBehavior.Continue;
                }

                while (checkVisual != null && checkVisual != uielement)
                {
                    if (hitFunction(checkVisual))
                    {
                        return HitTestResultBehavior.Stop;
                    }

                    checkVisual = VisualTreeHelper.GetParent(checkVisual) as UIElement;
                }

                return HitTestResultBehavior.Continue;
            };

            VisualTreeHelper.HitTest(uielement, null, hitTestResultCallback, hittestParameters);
        }

        /// <summary>
        /// ビジュアルを子方向に走査し、指定の型のオブジェクトを見つけるヘルパーメソッドです。
        /// </summary>
        /// <typeparam name="T">取得するオブジェクトの型</typeparam>
        /// <param name="getChildren">子要素を取得する処理</param>
        /// <param name="element">走査する親ビジュアル</param>
        /// <param name="searchAlgorithm">探索アルゴリズム</param>
        /// <returns>親ビジュアルに含まれる子要素</returns>
        private static IEnumerable<T> SearchTreeHelper<T>(
            Func<Visual, IEnumerable> getChildren, Visual element, GraphSearchAlgorithm searchAlgorithm)
            where T : class
        {
            switch (searchAlgorithm)
            {
                case GraphSearchAlgorithm.BreadthFirstSearch:
                    return GraphSearch.SearchBreadthFirst(element, visual => getChildren(visual).OfType<Visual>()).OfType<T>();
                case GraphSearchAlgorithm.DepthFirstSearch:
                    return GraphSearch.SearchDepthFirst(element, visual => getChildren(visual).OfType<Visual>()).OfType<T>();
                default:
                    throw new NotSupportedException(string.Format("This algorithm [{0}] is not supported", searchAlgorithm));
            }
        }
    }
}