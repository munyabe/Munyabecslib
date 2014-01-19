using System.Windows;

namespace Munyabe.Windows.Triggers
{
    /// <summary>
    /// 指定されたイベントが発生したときにコマンドを実行するトリガーです。
    /// </summary>
    public sealed class EventCommandTrigger : CommandTriggerBase
    {
        /// <summary>
        /// <see cref="RoutedEvent"/>依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty RoutedEventProperty = DependencyProperty.Register(
            "RoutedEvent",
            typeof(RoutedEvent),
            typeof(EventCommandTrigger),
            new FrameworkPropertyMetadata(null));

        /// <summary>
        /// ルーティングイベントを取得または設定します。これは、依存関係プロパティです。
        /// </summary>
        public RoutedEvent RoutedEvent
        {
            get { return (RoutedEvent)GetValue(RoutedEventProperty); }
            set { SetValue(RoutedEventProperty, value); }
        }

        /// <summary>
        /// 新しいインスタンスを作成します。
        /// </summary>
        protected override Freezable CreateInstanceCore()
        {
            return new EventCommandTrigger();
        }

        /// <summary>
        /// トリガーを初期化する処理です。
        /// </summary>
        protected override void InitializeCore(FrameworkElement source)
        {
            source.AddHandler(RoutedEvent, (RoutedEventHandler)((sender, e) =>
            {
                if (e.Handled == false)
                {
                    ExecuteCommand();
                }
            }));
        }
    }
}