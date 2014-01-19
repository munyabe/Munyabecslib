using System;
using System.Windows;
using System.Windows.Input;

namespace Munyabe.Windows.Commands
{
    /// <summary>
    /// 依存関係プロパティを実装しないコントロールでもXAMLでコマンドをバインドできるよう、
    /// <see cref="Command"/>依存関係プロパティを実装をするクラスです。
    /// </summary>
    public class CommandReference : Freezable, ICommand
    {
        /// <summary>
        /// コマンドを実行するかどうかに影響する変更があった場合に発生します。
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// <see cref="Command"/>依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command",
            typeof(ICommand),
            typeof(CommandReference),
            new PropertyMetadata(new PropertyChangedCallback(OnCommandChanged)));

        /// <summary>
        /// 起動するコマンドを取得または設定します。これは、依存関係プロパティです。
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// 現在の状態でこのコマンドを実行できるかどうかを判断するメソッドを定義します。
        /// </summary>
        /// <param name="parameter">コマンドで使用されたデータ。コマンドにデータを渡す必要がない場合は、このオブジェクトを<c>null</c>に設定できます。</param>
        /// <returns>このコマンドを実行できる場合は<c>true</c>。実行できない場合は<c>false</c>。</returns>
        public bool CanExecute(object parameter)
        {
            bool result = false;

            if (Command != null)
            {
                result = Command.CanExecute(parameter);
            }

            return result;
        }

        /// <summary>
        /// コマンドの起動時に呼び出されるメソッドを定義します。
        /// </summary>
        /// <param name="parameter">コマンドで使用されたデータ。コマンドにデータを渡す必要がない場合は、このオブジェクトを<c>null</c>に設定できます。</param>
        public void Execute(object parameter)
        {
            Command.Execute(parameter);
        }

        /// <summary>
        /// 新しいインスタンスを作成しないため、実装されません。
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        protected override Freezable CreateInstanceCore()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// コマンドが変更されたときに呼び出されるコールバックです。
        /// </summary>
        private static void OnCommandChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var commandReference = obj as CommandReference;
            var oldCommand = e.OldValue as ICommand;
            var newCommand = e.NewValue as ICommand;

            if (oldCommand != null)
            {
                oldCommand.CanExecuteChanged -= commandReference.CanExecuteChanged;
            }

            if (newCommand != null)
            {
                newCommand.CanExecuteChanged += commandReference.CanExecuteChanged;
            }
        }
    }
}