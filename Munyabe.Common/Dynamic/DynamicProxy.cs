using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Munyabe.Common.Dynamic
{
    /// <summary>
    /// 内包する<c>Model</c>のプロパティを透過的に公開するプロキシークラスです。
    /// </summary>
    /// <remarks><see cref="DynamicProxy"/>を実装するオブジェクトのプロパティを再帰的に公開します。</remarks>
    public class DynamicProxy : DynamicObject, IDisposable, INotifyPropertyChanged
    {
        /// <summary>
        /// 動的プロキシーによりアクセスするプロパティのゲッターメソッドを保持するディクショナリです。
        /// </summary>
        private readonly Dictionary<string, GetMethod> _getMethods = new Dictionary<string, GetMethod>();

        /// <summary>
        /// 動的プロキシーによりアクセスするプロパティのセッターメソッドを保持するディクショナリです。
        /// </summary>
        private readonly Dictionary<string, SetMethod> _setMethods = new Dictionary<string, SetMethod>();

        /// <summary>
        /// <see cref="Dispose()"/>が呼び出されたときに発生するイベントです。
        /// </summary>
        public event EventHandler Disposing;

        /// <summary>
        /// プロパティ値が変更されたときに発生するイベントです。
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 内包する<c>Model</c>が変更されたときに発生するイベントです。
        /// </summary>
        protected event EventHandler<ModelChangedEventArgs> ModelChanged;

        /// <summary>
        /// 内包する<c>Model</c>を取得します。
        /// <para>
        /// <list type="bullet">
        ///     <listheader>留意点 :</listheader>
        ///     <item><see cref="ModelChanged"/>イベント発生時にコレクションが更新されます。</item>
        ///     <item>同じ型の<c>Model</c>を複数登録することはできません。登録時に上書きされます。</item>
        /// </list>
        /// </para>
        /// </summary>
        /// <remarks><see cref="OnModelChanged"/>によって<c>Model</c>が登録されます。</remarks>
        public IDictionary<Type, object> InnerModels { get; private set; }

        /// <summary>
        /// <see cref="InnerModels"/>に登録されているプロパティ名の一覧を取得します。
        /// </summary>
        public ISet<string> PropertyNames { get; private set; }

        /// <summary>
        /// <see cref="ValidationAttribute"/>が付与されたプロパティ名の一覧を取得します。
        /// </summary>
        public ISet<string> ValidatedPropertyNames { get; private set; }

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        public DynamicProxy()
        {
            InnerModels = new Dictionary<Type, object>();
            PropertyNames = new HashSet<string>();
            ValidatedPropertyNames = new HashSet<string>();

            ModelChanged += (sender, e) => RegisterModel(e.Model);
        }

        /// <summary>
        /// <see cref="DynamicProxy"/>によって使用されているすべてのリソースを解放します。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// <c>Model</c>から同名のプロパティを検出して値を取得します。
        /// </summary>
        /// <param name="propertyName">プロパティ名</param>
        /// <returns>検出したプロパティの値</returns>
        /// <exception cref="InvalidOperationException">プロパティが存在しません。</exception>
        public object GetMember(string propertyName)
        {
            object result;
            if (TryGetMemberHelper(propertyName, out result))
            {
                return result;
            }
            else
            {
                throw new InvalidOperationException(string.Format("This property [{0}] is not found.", propertyName));
            }
        }

        /// <summary>
        /// <see cref="InnerModels"/>から指定したプロパティを持つ<c>Model</c>を取得します。
        /// </summary>
        /// <remarks>
        /// <c>Model</c>が見つからない場合は<see langword="null"/>が返却されます。
        /// </remarks>
        /// <param name="propertyName">取得する<c>Model</c>のプロパティ名</param>
        /// <returns>指定したプロパティを持つ<c>Model</c></returns>
        public object GetSourceModel(string propertyName)
        {
            return InnerModels
                .Select(pair => pair.Value)
                .Where(model => model != null)
                .FirstOrDefault(model => model.GetType().GetProperty(propertyName) != null);
        }

        /// <summary>
        /// プロパティを透過的に公開する<c>Model</c>を登録します。
        /// </summary>
        /// <param name="model">登録する<c>Model</c></param>
        public void RegisterModel(object model)
        {
            if (model == null)
            {
                return;
            }

            var modelType = model.GetType();
            InnerModels[modelType] = model;

            var properties = modelType.GetProperties();
            properties
                .Select(property => property.Name)
                .ForEach(propertyName =>
                {
                    PropertyNames.Add(propertyName);

                    _getMethods.Remove(propertyName);
                    _setMethods.Remove(propertyName);
                    OnPropertyChanged(propertyName);
                });

            properties
                .Where(property => property.GetCustomAttributes(typeof(ValidationAttribute), false).Any())
                .ForEach(property => ValidatedPropertyNames.Add(property.Name));

            if (ReferenceEquals(this, model) == false)
            {
                PropagatePropertyChanged(model);
            }
        }

        /// <summary>
        /// <c>Model</c>から同名のプロパティを検出して値を設定します。
        /// </summary>
        /// <param name="propertyName">プロパティ名</param>
        /// <param name="value">設定する値</param>
        /// <exception cref="InvalidOperationException">プロパティが存在しません。</exception>
        public void SetMember(string propertyName, object value)
        {
            if (TrySetMemberHelper(propertyName, value) == false)
            {
                throw new InvalidOperationException(string.Format("This property [{0}] is not found.", propertyName));
            }
        }

        /// <summary>
        /// プロパティが存在しないとき、<c>Model</c>から同名のプロパティを検出して値を取得します。
        /// </summary>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return TryGetMemberHelper(binder.Name, out result);
        }

        /// <summary>
        /// プロパティが存在しないとき、<c>Model</c>から同名のプロパティを検出して値を設定します。
        /// </summary>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            return TrySetMemberHelper(binder.Name, value);
        }

        /// <summary>
        /// 入力値をクリアする処理です。
        /// <remarks>規定の実装では、<see cref="DynamicObject"/>を通して設定したプロパティにデフォルト値を設定します。</remarks>
        /// </summary>
        protected virtual void ClearWritableProperty()
        {
            _setMethods.ForEach(pair =>
            {
                var setMethod = pair.Value;
                setMethod.Method(setMethod.Model, setMethod.PropertyType.GetDafaultValue());
                OnPropertyChanged(pair.Key);
            });
        }

        /// <summary>
        /// <see cref="DynamicProxy"/>によって使用されているアンマネージリソースを解放し、オプションでマネージリソースも解放します。
        /// </summary>
        /// <param name="disposing">
        /// マネージリソースとアンマネージリソースの両方を解放する場合は<c>true</c>。
        /// アンマネージリソースだけを解放する場合は<c>false</c>。
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                InnerModels
                    .Select(pair => pair.Value)
                    .Where(obje => ReferenceEquals(this, obje) == false)
                    .OfType<IDisposable>()
                    .ForEach(each => each.Dispose());

                OnDisposing(EventArgs.Empty);
            }
        }

        /// <summary>
        /// <see cref="Disposing"/>イベントを発生させます。
        /// </summary>
        protected virtual void OnDisposing(EventArgs e)
        {
            var handler = Disposing;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// <see cref="ModelChanged"/>イベントを発生させます。
        /// </summary>
        /// <param name="model">変更された<c>Model</c></param>
        protected virtual void OnModelChanged(object model)
        {
            var handler = ModelChanged;
            if (handler != null)
            {
                handler(this, new ModelChangedEventArgs { Model = model });
            }
        }

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

        /// <summary>
        /// オブジェクトから指定した名前のプロパティ情報を取得します。
        /// </summary>
        /// <param name="target">プロパティを取得するオブジェクト</param>
        /// <param name="propertyName">取得するプロパティ名</param>
        /// <param name="propertyFilter">プロパティを絞り込むフィルター</param>
        /// <returns>指定したプロパティと、その対象となるオブジェクトの組</returns>
        protected static Tuple<object, PropertyInfo> GetPropertyTuple(
            object target, string propertyName, Predicate<PropertyInfo> propertyFilter)
        {
            Guard.ArgumentNotNull(target, "target");

            Tuple<object, PropertyInfo> result = null;

            var property = target.GetType().GetProperty(propertyName);
            if (property != null && propertyFilter(property))
            {
                result = new Tuple<object, PropertyInfo>(target, property);
            }
            else
            {
                var proxy = target as DynamicProxy;
                if (proxy != null)
                {
                    result = proxy.InnerModels
                        .Where(pair => pair.Key != target.GetType())
                        .Select(pair => GetPropertyTuple(pair.Value, propertyName, propertyFilter))
                        .FirstOrDefault(each => each != null);
                }
            }

            return result;
        }

        /// <summary>
        /// オブジェクトから指定した名前のプロパティ情報を取得します。
        /// </summary>
        /// <param name="targets">プロパティを取得するオブジェクト</param>
        /// <param name="propertyName">取得するプロパティ名</param>
        /// <param name="propertyFilter">プロパティを絞り込むフィルター</param>
        /// <returns>指定したプロパティと、その対象となるオブジェクトの組</returns>
        protected static Tuple<object, PropertyInfo> GetPropertyTuple(
            IEnumerable<object> targets, string propertyName, Predicate<PropertyInfo> propertyFilter)
        {
            Guard.ArgumentNotNull(targets, "targets");

            return targets
                .Select(model => GetPropertyTuple(model, propertyName, propertyFilter))
                .FirstOrDefault(each => each != null);
        }

        /// <summary>
        /// 指定したオブジェクトの<see cref="PropertyChanged"/>イベントの発生を伝播します。
        /// </summary>
        private void PropagatePropertyChanged(object model)
        {
            var notifyModel = model as INotifyPropertyChanged;
            if (notifyModel != null)
            {
                PropertyChangedEventHandler handler = (sender, e) => OnPropertyChanged(e.PropertyName);

                notifyModel.PropertyChanged += handler;
                Disposing += (sender, e) => notifyModel.PropertyChanged -= handler;
            }
        }

        /// <summary>
        /// プロパティが存在しないとき、<c>Model</c>から同名のプロパティを検出して値を取得するヘルパーメソッドです。
        /// </summary>
        private bool TryGetMemberHelper(string propertyName, out object result)
        {
            GetMethod getMethod;
            if (_getMethods.TryGetValue(propertyName, out getMethod) == false)
            {
                var targetTuple = GetPropertyTuple(
                    InnerModels.Where(pair => pair.Key != GetType()).Select(pair => pair.Value),
                    propertyName,
                    property => property.CanRead);
                if (targetTuple == null)
                {
                    result = null;
                    return false;
                }

                getMethod = new GetMethod(targetTuple);
                _getMethods.Add(propertyName, getMethod);
            }

            result = getMethod.Method(getMethod.Model);
            return true;
        }

        /// <summary>
        /// プロパティが存在しないとき、<c>Model</c>から同名のプロパティを検出して値を設定するヘルパーメソッドです。
        /// </summary>
        private bool TrySetMemberHelper(string propertyName, object value)
        {
            SetMethod setMethod;
            if (_setMethods.TryGetValue(propertyName, out setMethod) == false)
            {
                var targetTuple = GetPropertyTuple(
                    InnerModels.Where(pair => pair.Key != GetType()).Select(pair => pair.Value),
                    propertyName,
                    property => property.CanRead);
                if (targetTuple == null)
                {
                    return false;
                }

                setMethod = new SetMethod(targetTuple);
                _setMethods.Add(propertyName, setMethod);
            }

            object convertedValue;
            if (ConverterUtil.TryConvertValue(value, setMethod.PropertyType, out convertedValue))
            {
                setMethod.Method(setMethod.Model, convertedValue);
            }

            OnPropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        /// プロパティのセッターメソッドを保持するクラスです。
        /// </summary>
        private sealed class GetMethod
        {
            /// <summary>
            /// ゲッターメソッドを取得します。
            /// </summary>
            public Func<object, object> Method { get; private set; }

            /// <summary>
            /// プロパティを持つModelのインスタンスを取得します。
            /// </summary>
            public object Model { get; private set; }

            /// <summary>
            /// インスタンスを初期化します。
            /// </summary>
            /// <param name="propertyTuple">プロパティと、値を取得するオブジェクトの組</param>
            public GetMethod(Tuple<object, PropertyInfo> propertyTuple)
            {
                Model = propertyTuple.Item1;
                var property = propertyTuple.Item2;

                Method = ExpressionUtil.CreateGetter(property).Compile();
            }
        }

        /// <summary>
        /// プロパティのゲッターメソッドを保持するクラスです。
        /// </summary>
        private sealed class SetMethod
        {
            /// <summary>
            /// セッターメソッドを取得します。
            /// </summary>
            public Action<object, object> Method { get; private set; }

            /// <summary>
            /// プロパティを持つModelのインスタンスを取得します。
            /// </summary>
            public object Model { get; private set; }

            /// <summary>
            /// プロパティの型を取得します。
            /// </summary>
            public Type PropertyType { get; private set; }

            /// <summary>
            /// インスタンスを初期化します。
            /// </summary>
            /// <param name="propertyTuple">プロパティと、値を設定するオブジェクトの組</param>
            public SetMethod(Tuple<object, PropertyInfo> propertyTuple)
            {
                Model = propertyTuple.Item1;
                var property = propertyTuple.Item2;
                PropertyType = property.PropertyType;

                Method = ExpressionUtil.CreateSetter(property).Compile();
            }
        }
    }
}