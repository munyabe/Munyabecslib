using System;
using System.Collections.Generic;

namespace Munyabe.Common
{
    /// <summary>
    /// ビット演算に関するユーティリティクラスです。
    /// </summary>
    public static class BitOperationUtil
    {
        /// <summary>
        /// 整数をビットの数列に分解します。
        /// </summary>
        /// <remarks>
        /// ex) 13 = 1 + 4 + 8
        /// </remarks>
        /// <param name="num">分解する整数</param>
        /// <returns>ビットの数列</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="num"/>が負の整数です。</exception>
        public static IEnumerable<int> DivideToBits(int num)
        {
            if (num < 0)
            {
                throw new ArgumentOutOfRangeException(string.Format("The argument [num] must be over 0."));
            }

            for (var i = 0; 0 < num; num >>= 1, i++)
            {
                if ((num & 1) != 0)
                {
                    yield return 1 << i;
                }
            }
        }
    }
}
