using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Munyabe.Common;
using Munyabe.Windows.Properties;

namespace Munyabe.Windows.Commands
{
    /// <summary>
    /// 複数の<see cref="ICommand"/>をまとめて実行するコマンドです。
    /// Prism の Microsoft.Practices.Composite.Presentation.Commands.CompositeCommand を参考にしています。
    /// </summary>
    public class CompositeCommand : ICommand
    {
        private readonly List<ICommand> _registeredCommands = new List<ICommand>();
        private readonly bool _monitorCommandActivity;
        private readonly EventHandler _onRegisteredCommandCanExecuteChanged;

        /// <summary>
        /// コマンドの実行可能判定に影響があったとき発生するイベントです。
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// <see cref="CompositeCommand"/>のインスタンスを作成するコンストラクターです。
        /// </summary>
        public CompositeCommand()
            : this(false)
        {
        }

        /// <summary>
        /// <see cref="CompositeCommand"/>のインスタンスを作成するコンストラクターです。
        /// </summary>
        /// <param name="monitorCommandActivity">登録されたコマンドがアクティブかどうかチェックするとき<see langword="true"/></param>
        public CompositeCommand(bool monitorCommandActivity)
        {
            _monitorCommandActivity = monitorCommandActivity;

            _onRegisteredCommandCanExecuteChanged = (sender, e) => OnCanExecuteChanged();
        }

        /// <summary>
        /// 登録されたコマンドが現在の状態で実行可能かどうかを判定します。
        /// </summary>
        /// <param name="parameter">コマンドのパラメーター。パラメーターが必要ないときは<see langword="null"/>を指定します。</param>
        /// <returns>全てのコマンドが実行可能なとき<see langword="true"/></returns>
        public virtual bool CanExecute(object parameter)
        {
            ICommand[] commandList;
            lock (_registeredCommands)
            {
                commandList = _registeredCommands.ToArray();
            }

            return commandList.Where(ShouldExecute).Any(c => c.CanExecute(parameter) == false) == false;
        }

        /// <summary>
        /// 登録されたコマンドをパラメーターなしで実行します。
        /// </summary>
        public virtual void Execute()
        {
            Execute(null);
        }

        /// <summary>
        /// 登録されたコマンドを実行します。
        /// </summary>
        /// <param name="parameter">コマンドのパラメーター。パラメーターが必要ないときは<see langword="null"/>を指定します。</param>
        public virtual void Execute(object parameter)
        {
            Queue<ICommand> commands;
            lock (_registeredCommands)
            {
                commands = new Queue<ICommand>(_registeredCommands.Where(ShouldExecute).ToArray());
            }

            while (commands.Any())
            {
                var command = commands.Dequeue();
                command.Execute(parameter);
            }
        }

        /// <summary>
        /// コマンドを登録します。
        /// </summary>
        /// <param name="command">登録するコマンド</param>
        /// <exception cref="ArgumentNullException"><paramref name="command"/>が<see langword="null"/>です。</exception>
        /// <exception cref="InvalidOperationException">同じコマンドを重複して登録できません。</exception>
        /// <exception cref="ArgumentException">自分自身は登録できません。</exception>
        public virtual void RegisterCommand(ICommand command)
        {
            Guard.ArgumentNotNull(command, "command");

            if (command == this)
            {
                throw new ArgumentException(ErrorMessageResource.CannotRegisterCompositeCommandInItself);
            }

            lock (_registeredCommands)
            {
                if (_registeredCommands.Contains(command))
                {
                    throw new InvalidOperationException(ErrorMessageResource.CannotRegisterSameCommandTwice);
                }
                _registeredCommands.Add(command);
            }

            command.CanExecuteChanged += _onRegisteredCommandCanExecuteChanged;
            OnCanExecuteChanged();

            if (_monitorCommandActivity)
            {
                var activeAwareCommand = command as IActiveAware;
                if (activeAwareCommand != null)
                {
                    activeAwareCommand.IsActiveChanged += OnRegisteredCommandIsActiveChanged;
                }
            }
        }

        /// <summary>
        /// 実行するコマンドリストから登録を解除します。
        /// </summary>
        /// <param name="command">登録を解除するコマンド</param>
        /// <exception cref="ArgumentNullException"><paramref name="command"/>が<see langword="null"/>です。</exception>
        public virtual void UnregisterCommand(ICommand command)
        {
            Guard.ArgumentNotNull(command, "command");

            bool removed;
            lock (_registeredCommands)
            {
                removed = _registeredCommands.Remove(command);
            }

            if (removed)
            {
                command.CanExecuteChanged -= _onRegisteredCommandCanExecuteChanged;
                OnCanExecuteChanged();

                if (_monitorCommandActivity)
                {
                    var activeAwareCommand = command as IActiveAware;
                    if (activeAwareCommand != null)
                    {
                        activeAwareCommand.IsActiveChanged -= OnRegisteredCommandIsActiveChanged;
                    }
                }
            }
        }

        /// <summary>
        /// <see cref="CanExecuteChanged"/>イベントを発生させます。
        /// </summary>
        protected virtual void OnCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// コマンドを利用すべきか判定します。
        /// </summary>
        /// <param name="command">判定するコマンド</param>
        /// <returns><see cref="CompositeCommand.Execute"/>と<see cref="CompositeCommand.CanExecute"/>を利用できるとき<see langword="true"/></returns>
        protected virtual bool ShouldExecute(ICommand command)
        {
            var activeAwareCommand = command as IActiveAware;
            return _monitorCommandActivity == false || activeAwareCommand == null || activeAwareCommand.IsActive;
        }

        /// <summary>
        /// <see cref="IActiveAware.IsActiveChanged"/>イベントが発生したときに呼び出されるコールバックです。
        /// </summary>
        private void OnRegisteredCommandIsActiveChanged(object sender, EventArgs e)
        {
            OnCanExecuteChanged();
        }
    }

    /// <summary>
    /// 複数の<see cref="ICommand"/>をまとめて実行するコマンドです。コマンドパラメーターの型を指定できます。
    /// </summary>
    /// <typeparam name="T">コマンドパラメーターの型</typeparam>
    public class CompositeCommand<T> : CompositeCommand
    {
        /// <summary>
        /// コマンドを登録します。詳細は<see cref="CompositeCommand.RegisterCommand"/>のサマリーを参照してください。
        /// </summary>
        /// <param name="command">登録するコマンド</param>
        /// <exception cref="ArgumentException">コマンドパラメーターの型が異なります。</exception>
        public override void RegisterCommand(ICommand command)
        {
            Guard.InstanceIsAssignable(typeof(DelegateCommand<T>), command, "command");
            base.RegisterCommand(command);
        }
    }
}