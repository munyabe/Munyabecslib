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
            GuardIsOverZero(num, "num");

            for (var i = 0; 0 < num; num >>= 1, i++)
            {
                if ((num & 1) == 1)
                {
                    yield return 1 << i;
                }
            }
        }

        /// <summary>
        /// 1ビットからなる整数かどうかを判定します。
        /// </summary>
        /// <param name="num">判定する整数</param>
        /// <returns>1ビットからなる数値の場合は<see langword="true"/></returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="num"/>が負の整数です。</exception>
        public static bool IsSingleBit(int num)
        {
            GuardIsOverZero(num, "num");

            var hasBit = false;

            for (; 0 < num; num >>= 1)
            {
                if ((num & 1) == 1)
                {
                    if (hasBit)
                    {
                        return false;
                    }

                    hasBit = true;
                }
            }

            return hasBit;
        }

        /// <summary>
        /// パラメーターが0以上の整数であることを示します。
        /// </summary>
        private static void GuardIsOverZero(int argumentValue, string argumentName)
        {
            if (argumentValue < 0)
            {
                throw new ArgumentOutOfRangeException(string.Format("The argument [{0}] must be over 0.", argumentName));
            }
        }
    }
}
