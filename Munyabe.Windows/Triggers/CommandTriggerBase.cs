using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Munyabe.Windows.Triggers
{
    /// <summary>
    /// 指定したタイミングでコマンドを実行するトリガーの基底クラスです。
    /// </summary>
    public abstract class CommandTriggerBase : Freezable, ICommandTrigger
    {
        /// <summary>
        /// <see cref="Command"/>依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command",
            typeof(ICommand),
            typeof(CommandTriggerBase),
            new FrameworkPropertyMetadata(null));

        /// <summary>
        /// <see cref="CommandParameter"/>依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            "CommandParameter",
            typeof(object),
            typeof(CommandTriggerBase),
            new FrameworkPropertyMetadata(null));

        /// <summary>
        /// <see cref="CommandTarget"/>依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register(
            "CommandTarget",
            typeof(IInputElement),
            typeof(CommandTriggerBase),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        /// <see cref="UpdateCommandParameter"/>依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty UpdateCommandParameterProperty = DependencyProperty.Register(
            "UpdateCommandParameter",
            typeof(bool),
            typeof(CommandTriggerBase),
            new FrameworkPropertyMetadata(false));

        /// <summary>
        /// インスタンスが初期化されたかどうかを取得または設定します。
        /// </summary>
        public bool IsInitialized { get; set; }

        /// <summary>
        /// 実行するコマンドを取得または設定します。これは、依存関係プロパティです。
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// コマンドのパラメーターを取得または設定します。これは、依存関係プロパティです。
        /// </summary>
        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        /// <summary>
        /// コマンドが<see cref="RoutedCommand"/>の場合にハンドラを検索する要素を取得または設定します。これは、依存関係プロパティです。
        /// </summary>
        public IInputElement CommandTarget
        {
            get { return (IInputElement)GetValue(CommandTargetProperty); }
            set { SetValue(CommandTargetProperty, value); }
        }

        /// <summary>
        /// コマンド実行時に<see cref="CommandParameter"/>を再バインドするかどうかを取得または設定します。これは、依存関係プロパティです。
        /// </summary>
        public bool UpdateCommandParameter
        {
            get { return (bool)GetValue(UpdateCommandParameterProperty); }
            set { SetValue(UpdateCommandParameterProperty, value); }
        }

        /// <summary>
        /// トリガーを初期化する処理を記述してください。
        /// </summary>
        protected abstract void InitializeCore(FrameworkElement source);

        /// <summary>
        /// コマンドを実行します。
        /// </summary>
        protected void ExecuteCommand()
        {
            if (Command == null || Command.CanExecute(CommandParameter) == false)
            {
                return;
            }

            if (UpdateCommandParameter)
            {
                var bindingExpression = BindingOperations.GetBindingExpression(this, CommandParameterProperty);
                if (bindingExpression != null)
                {
                    bindingExpression.UpdateTarget();
                }
            }

            var routedCommand = Command as RoutedCommand;
            if (routedCommand != null)
            {
                routedCommand.Execute(CommandParameter, CommandTarget);
            }
            else
            {
                Command.Execute(CommandParameter);
            }
        }

        /// <summary>
        /// トリガーを初期化します。
        /// </summary>
        void ICommandTrigger.Initialize(FrameworkElement source)
        {
            if (IsInitialized == false)
            {
                InitializeCore(source);
                IsInitialized = true;
            }
        }
    }
}