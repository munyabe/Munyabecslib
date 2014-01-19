using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using Munyabe.Common;

namespace Munyabe.Windows.Commands
{
    /// <summary>
    /// コマンドの実装クラスです。
    /// Prism の Microsoft.Practices.Composite.Presentation.Commands.DelegateCommand{T} を参考にしています。
    /// </summary>
    /// <typeparam name="T">コマンドのパラメーターの型</typeparam>
    /// <seealso cref="CompositeCommand"/>
    public class DelegateCommand<T> : ICommand, IActiveAware
    {
        private readonly Action<T> _executeMethod;
        private readonly Func<T, bool> _canExecuteMethod;
        private bool _isActive;

        /// <summary>
        /// コマンドの実行可能判定に影響があったとき発生するイベントです。
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// <see cref="IsActive"/>が変更されたときに発生するイベントです。
        /// </summary>
        public event EventHandler IsActiveChanged;

        /// <summary>
        /// コマンドがアクティブかどうかを取得または設定します。
        /// </summary>
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    OnIsActiveChanged();
                }
            }
        }

        /// <summary>
        /// <see cref="DelegateCommand{T}"/>のインスタンスを作成するコンストラクターです。
        /// </summary>
        /// <param name="executeMethod">コマンドが実行されたときに呼び出される処理</param>
        /// <remarks><seealso cref="CanExecute"/>は常に<c>true</c>になります。</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="executeMethod"/>が<see langword="null"/>です。</exception>
        public DelegateCommand(Action<T> executeMethod)
            : this(executeMethod, null)
        {
        }

        /// <summary>
        /// <see cref="DelegateCommand{T}"/>のインスタンスを作成するコンストラクターです。
        /// </summary>
        /// <param name="executeMethod">コマンドが実行されたときに呼び出される処理</param>
        /// <param name="canExecuteMethod">コマンドが実行可能かどうかを判定する処理</param>
        /// <exception cref="ArgumentNullException"><paramref name="executeMethod"/>が<see langword="null"/>です。</exception>
        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
        {
            Guard.ArgumentNotNull(executeMethod, "executeMethod");

            _executeMethod = executeMethod;
            _canExecuteMethod = canExecuteMethod;
        }

        /// <summary>
        /// コマンドが現在の状態で実行可能かどうかを判定します。
        /// </summary>
        /// <param name="parameter">コマンドのパラメーター。パラメーターが必要ないときは<see langword="null"/>を指定します。</param>
        public bool CanExecute(T parameter)
        {
            return _canExecuteMethod == null || _canExecuteMethod(parameter);
        }

        /// <summary>
        /// コマンドを実行します。
        /// </summary>
        /// <param name="parameter">コマンドのパラメーター。パラメーターが必要ないときは<see langword="null"/>を指定します。</param>
        public void Execute(T parameter)
        {
            _executeMethod(parameter);
        }

        /// <summary>
        /// <see cref="CanExecuteChanged"/>イベントを発生させます。
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }

        /// <summary>
        /// コマンドが現在の状態で実行可能かどうかを判定します。
        /// </summary>
        /// <param name="parameter">コマンドのパラメーター。パラメーターが必要ないときは<see langword="null"/>を指定します。</param>
        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute((T)parameter);
        }

        /// <summary>
        /// コマンドを実行します。
        /// </summary>
        /// <param name="parameter">コマンドのパラメーター。パラメーターが必要ないときは<see langword="null"/>を指定します。</param>
        void ICommand.Execute(object parameter)
        {
            Execute((T)parameter);
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
        /// <see cref="IsActiveChanged"/>イベントを発生させます。
        /// </summary>
        protected virtual void OnIsActiveChanged()
        {
            var handler = IsActiveChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }

    /// <summary>
    /// コマンドの実装クラスです。コマンドパラメーターが不要な場合に使用します。
    /// </summary>
    public class DelegateCommand : DelegateCommand<object>
    {
        /// <summary>
        /// 詳細は<see cref="DelegateCommand{T}"/>のコンストラクターサマリーを参照してください。
        /// </summary>
        public DelegateCommand(Action executeMethod)
            : base(obj => executeMethod())
        {
        }

        /// <summary>
        /// 詳細は<see cref="DelegateCommand{T}"/>のコンストラクターサマリーを参照してください。
        /// </summary>
        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod)
            : base(obj => executeMethod(), obj => canExecuteMethod())
        {
        }

        /// <summary>
        /// コマンドを実行します。
        /// </summary>
        public void Execute()
        {
            Execute(null);
        }

        /// <summary>
        /// コマンドが現在の状態で実行可能かどうかを判定します。
        /// </summary>
        public bool CanExecute()
        {
            return CanExecute(null);
        }
    }
}