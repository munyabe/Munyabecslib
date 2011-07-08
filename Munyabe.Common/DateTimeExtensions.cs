using System;

namespace Munyabe.Common
{
    /// <summary>
    /// <see cref="DateTime"/>の拡張メソッドを定義するクラスです。
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 指定された日付が属する月の最初の日付を取得します。
        /// </summary>
        /// <param name="date">対象の日付</param>
        /// <returns>月の最初の日付</returns>
        public static DateTime GetFirstDateOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        /// <summary>
        /// 指定された日付が属する月の最後の日付を取得します。
        /// </summary>
        /// <param name="date">対象の日付</param>
        /// <returns>月の最後の日付</returns>
        public static DateTime GetLastDateOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
        }
    }
}