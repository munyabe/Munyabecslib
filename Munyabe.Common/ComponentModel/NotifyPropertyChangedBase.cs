using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Munyabe.Common.ComponentModel
{
    /// <summary>
    /// <see cref="INotifyPropertyChanged"/>を実装する基底クラスです。
    /// </summary>
    public abstract class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        /// <summary>
        /// プロパティ値が変更されたときに発生するイベントです。
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// <see cref="PropertyChanged"/>イベントを発生させます。
        /// </summary>
        /// <param name="getter">変更されたプロパティの Get アクセサー</param>
        protected virtual void OnPropertyChanged<TResult>(Expression<Func<TResult>> getter)
        {
            var propertyName = ExpressionUtil.GetMemberName(getter);
            OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// <see cref="PropertyChanged"/>イベントを発生させます。
        /// </summary>
        /// <param name="propertyName">変更されたプロパティ名</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
