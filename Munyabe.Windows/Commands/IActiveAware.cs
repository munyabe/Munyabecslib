using System;

namespace Munyabe.Windows.Commands
{
    /// <summary>
    /// オブジェクトがアクティブになったことをクライアントに通知します。
    /// </summary>
    public interface IActiveAware
    {
        /// <summary>
        /// <see cref="IsActive"/>プロパティが変更されたことを通知します。
        /// </summary>
        event EventHandler IsActiveChanged;

        /// <summary>
        /// アクティブかどうかを取得または設定します。
        /// </summary>
        bool IsActive { get; set; }
    }
}