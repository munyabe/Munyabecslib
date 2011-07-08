using System.Windows;

namespace Munyabe.Windows.Triggers
{
    /// <summary>
    /// コマンドトリガーを定義します。
    /// </summary>
    public interface ICommandTrigger
    {
        /// <summary>
        /// トリガーを初期化します。
        /// </summary>
        void Initialize(FrameworkElement source);
    }
}