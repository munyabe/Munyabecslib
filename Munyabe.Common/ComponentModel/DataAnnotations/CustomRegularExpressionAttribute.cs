using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CommonLibrary.DataAnnotations.DataAnnotations
{
    /// <summary>
    /// 値が指定した正規表現に一致しなければならないことを指定します。
    /// <remarks>値が空文字の場合も検証するため<see cref="RegularExpressionAttribute"/>を拡張しています。</remarks>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class CustomRegularExpressionAttribute : RegularExpressionAttribute
    {
        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        /// <param name="pattern">値の検証に使用する正規表現</param>
        public CustomRegularExpressionAttribute(string pattern)
            : base(pattern)
        {
        }

        /// <summary>
        /// ユーザーが入力した値が正規表現パターンと一致するかどうかをチェックします。
        /// <remarks>値が<c>null</c>のときはチェックせず、<c>true</c>になります。</remarks>
        /// </summary>
        /// <param name="value">検証する値</param>
        /// <returns>検証が成功した場合は<c>true</c></returns>
        /// <exception cref="ValidationException">値が正規表現パターンと一致ないときに発生します。</exception>
        public override bool IsValid(object value)
        {
            var targetValue = value ?? string.Empty;
            return new Regex(Pattern).IsMatch(targetValue.ToString());
        }
    }
}
