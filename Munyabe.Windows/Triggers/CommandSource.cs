using System.Windows;

namespace Munyabe.Windows.Triggers
{
    /// <summary>
    /// プロパティにアタッチされたトリガーを所有する静的クラスです。
    /// </summary>
    public static class CommandSource
    {
        /// <summary>
        /// <see cref="Triggers"/>依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty TriggersProperty = DependencyProperty.RegisterAttached(
            "Triggers",
            typeof(ICommandTrigger),
            typeof(CommandSource),
            new UIPropertyMetadata(null, TriggersPropertyChanged));

        /// <summary>
        /// トリガーを取得します。これは、依存関係プロパティです。
        /// </summary>
        public static ICommandTrigger GetTriggers(FrameworkElement source)
        {
            return (ICommandTrigger)source.GetValue(TriggersProperty);
        }

        /// <summary>
        /// トリガーを設定します。これは、依存関係プロパティです。
        /// </summary>
        public static void SetTriggers(FrameworkElement source, ICommandTrigger value)
        {
            source.SetValue(TriggersProperty, value);
        }

        /// <summary>
        /// <see cref="Triggers"/>依存関係プロパティが変更されたときに呼び出されるコールバックです。
        /// </summary>
        private static void TriggersPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as FrameworkElement;

            var commandTrigger = e.NewValue as ICommandTrigger;
            if (commandTrigger != null)
            {
                commandTrigger.Initialize(element);
            }
        }
    }
}