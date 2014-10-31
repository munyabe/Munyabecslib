using System;
using System.Collections.Generic;

namespace Munyabe.Common
{
    /// <summary>
    /// メモリーリークをチェックするための機能を提供するクラスです。
    /// </summary>
    public static class MemoryChecker
    {
        /// <summary>
        /// メモリーリークをチェックする対象のインスタンスへの弱参照です。
        /// </summary>
        private static readonly IList<WeakReference> _checkObjects = new List<WeakReference>();

        /// <summary>
        /// GC を実行し、登録されたインスタンスで参照が保持されているインスタンスを列挙します。
        /// </summary>
        /// <remarks>
        /// ここで返却されるオブジェクトがメモリーリークしている可能性があります。
        /// </remarks>
        /// <returns>参照が保持されているインスタンス</returns>
        public static IEnumerable<object> GetAliveObjects()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            lock (_checkObjects)
            {
                for (int i = 0; i < _checkObjects.Count; i++)
                {
                    var weakRef = _checkObjects[i];
                    object target = weakRef.Target;
                    if (target != null)
                    {
                        yield return target;
                    }
                    else
                    {
                        _checkObjects.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        /// <summary>
        /// GC によるクリアの対象になっていると考えられるインスタンスを登録します。
        /// </summary>
        /// <param name="target">対象のインスタンス</param>
        /// <returns>対象のインスタンスへの弱参照</returns>
        public static WeakReference RegisterCheckObject(object target)
        {
            if (target == null)
            {
                return null;
            }

            var weakRef = new WeakReference(target);
            lock (_checkObjects)
            {
                _checkObjects.Add(weakRef);
            }

            return weakRef;
        }
    }
}
