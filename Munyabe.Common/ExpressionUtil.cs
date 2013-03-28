using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace Munyabe.Common
{
    /// <summary>
    /// <see cref="Expression"/>を利用したユーティリティを提供するクラスです。
    /// </summary>
    public static class ExpressionUtil
    {
        /// <summary>
        /// 指定したプロパティの Get アクセサーを作成します。
        /// <remarks>
        /// 次のような<see cref="Func{T, T}"/>を作成します。
        /// </remarks>
        /// <example>
        /// <code>
        /// model => (object)(((Person)model).Name);
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="propertyInfo">アクセスするプロパティ</param>
        /// <returns>Get アクセサー</returns>
        /// <exception cref="ArgumentNullException"><paramref name="propertyInfo"/>が<see langword="null"/>です。</exception>
        public static Expression<Func<object, object>> CreateGetter(PropertyInfo propertyInfo)
        {
            Guard.ArgumentNotNull(propertyInfo, "property");

            var modelParameter = Expression.Parameter(typeof(object), "model");

            return Expression.Lambda<Func<object, object>>(
                Expression.Convert(
                    Expression.Property(
                        Expression.Convert(modelParameter, propertyInfo.DeclaringType),
                        propertyInfo),
                    typeof(object)),
                modelParameter);
        }

        /// <summary>
        /// 指定したプロパティの Get アクセサーを作成します。
        /// <remarks>
        /// 次のような<see cref="Func{TDeclaring, TProperty}"/>を作成します。
        /// </remarks>
        /// <example>
        /// <code>
        /// model => model.Name;
        /// </code>
        /// </example>
        /// </summary>
        /// <typeparam name="TDeclaring">プロパティを宣言するクラスの型</typeparam>
        /// <typeparam name="TProperty">プロパティの型</typeparam>
        /// <param name="propertyInfo">アクセスするプロパティ</param>
        /// <returns>Get アクセサー</returns>
        /// <exception cref="ArgumentNullException"><paramref name="propertyInfo"/>が<see langword="null"/>です。</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TDeclaring"/>が<paramref name="propertyInfo"/>を宣言するクラスに割り当てられません。</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TProperty"/>が<paramref name="propertyInfo"/>の型に一致しません。</exception>
        public static Expression<Func<TDeclaring, TProperty>> CreateGetter<TDeclaring, TProperty>(PropertyInfo propertyInfo)
        {
            Guard.ArgumentNotNull(propertyInfo, "property");
            Guard.TypeIsAssignable(propertyInfo.DeclaringType, typeof(TDeclaring), "TDeclaring");
            Guard.TypeIsEqual(propertyInfo.PropertyType, typeof(TProperty), "TProperty");

            var modelParameter = Expression.Parameter(typeof(TDeclaring), "model");

            return Expression.Lambda<Func<TDeclaring, TProperty>>(
                Expression.Property(modelParameter, propertyInfo), modelParameter);
        }

        /// <summary>
        /// 指定したプロパティの Set アクセサーを作成します。
        /// <remarks>
        /// 次のような<see cref="Action{T, T}"/>を作成します。
        /// </remarks>
        /// <example>
        /// <code>
        /// (model, value) => ((Person)model).Name = (string)value;
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="propertyInfo">アクセスするプロパティ</param>
        /// <returns>Set アクセサー</returns>
        /// <exception cref="ArgumentNullException"><paramref name="propertyInfo"/>が<see langword="null"/>です。</exception>
        /// <exception cref="ArgumentException"><paramref name="propertyInfo"/>がジェネリック型です。</exception>
        public static Expression<Action<object, object>> CreateSetter(PropertyInfo propertyInfo)
        {
            Guard.ArgumentNotNull(propertyInfo, "property");

            var modelParameter = Expression.Parameter(typeof(object), "model");
            var valueParameter = Expression.Parameter(typeof(object), "value");

            return Expression.Lambda<Action<object, object>>(
                Expression.Assign(
                    Expression.Property(
                        Expression.Convert(modelParameter, propertyInfo.DeclaringType),
                        propertyInfo),
                    Expression.ConvertChecked(valueParameter, propertyInfo.PropertyType)),
                modelParameter, valueParameter);
        }

        /// <summary>
        /// 指定したプロパティの Set アクセサーを作成します。
        /// <remarks>
        /// 次のような<see cref="Action{T, T}"/>を作成します。
        /// </remarks>
        /// <example>
        /// <code>
        /// (model, value) => model.Name = value;
        /// </code>
        /// </example>
        /// </summary>
        /// <typeparam name="TDeclaring">プロパティを宣言するクラスの型</typeparam>
        /// <typeparam name="TProperty">プロパティの型</typeparam>
        /// <param name="propertyInfo">アクセスするプロパティ</param>
        /// <returns>Set アクセサー</returns>
        /// <exception cref="ArgumentNullException"><paramref name="propertyInfo"/>が<see langword="null"/>です。</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TDeclaring"/>が<paramref name="propertyInfo"/>を宣言するクラスに割り当てられません。</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TProperty"/>がプロパティの型に割り当てられません。</exception>
        public static Expression<Action<TDeclaring, TProperty>> CreateSetter<TDeclaring, TProperty>(PropertyInfo propertyInfo)
        {
            Guard.ArgumentNotNull(propertyInfo, "property");
            Guard.TypeIsAssignable(propertyInfo.DeclaringType, typeof(TDeclaring), "TDeclaring");
            Guard.TypeIsAssignable(propertyInfo.PropertyType, typeof(TProperty), "TProperty");

            var modelParameter = Expression.Parameter(typeof(TDeclaring), "model");
            var valueParameter = Expression.Parameter(typeof(TProperty), "value");

            return Expression.Lambda<Action<TDeclaring, TProperty>>(
                Expression.Assign(
                    Expression.Property(modelParameter, propertyInfo), valueParameter),
                modelParameter, valueParameter);
        }

        /// <summary>
        /// ラムダ式からフィールドまたはプロパティ名を取得します。
        /// <example>
        /// <code>
        /// ExpressionUtil.GetBodyName(() => PropertyName); // 文字列 "PropertyName" が取得できる。
        /// </code>
        /// </example>
        /// </summary>
        /// <typeparam name="TResult">戻り値の型</typeparam>
        /// <param name="expression">ラムダ式</param>
        /// <returns>フィールドまたはプロパティ名</returns>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/>が<see langword="null"/>です。</exception>
        /// <exception cref="ArgumentException"><paramref name="expression"/>がフィールドまたはプロパティへのアクセスではありません。</exception>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static string GetMemberName<TResult>(Expression<Func<TResult>> expression)
        {
            return GetMemberNameHelper(expression);
        }

        /// <summary>
        /// ラムダ式からインスタンスのフィールドまたはプロパティ名を取得します。
        /// <example>
        /// <code>
        /// ExpressionUtil.GetBodyName(dummy => dummy.PropertyName); // 文字列 "PropertyName" が取得できる。
        /// </code>
        /// </example>
        /// </summary>
        /// <typeparam name="TInstance">フィールドまたはプロパティ名を取得したいインスタンスの型</typeparam>
        /// <typeparam name="TResult">戻り値の型</typeparam>
        /// <param name="expression">ラムダ式</param>
        /// <returns>フィールドまたはプロパティ名</returns>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/>が<see langword="null"/>です。</exception>
        /// <exception cref="ArgumentException"><paramref name="expression"/>がフィールドまたはプロパティへのアクセスではありません。</exception>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static string GetMemberName<TInstance, TResult>(Expression<Func<TInstance, TResult>> expression)
        {
            return GetMemberNameHelper(expression);
        }

        /// <summary>
        /// ラムダ式からメソッド名を取得します。
        /// <example>
        /// <code>
        /// ExpressionUtil.GetMethodName(() => Initialize()); // 文字列 "Initialize" が取得できる。
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="expression">ラムダ式</param>
        /// <returns>メソッド名</returns>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/>が<see langword="null"/>です。</exception>
        /// <exception cref="ArgumentException"><paramref name="expression"/>がメソッドの呼び出しではありません。</exception>
        public static string GetMethodName(Expression<Action> expression)
        {
            Guard.ArgumentNotNull(expression, "expression");

            var member = expression.Body as MethodCallExpression;
            if (member == null)
            {
                throw new ArgumentException(string.Format("The expression [{0}] is not a method call.", expression));
            }

            return member.Method.Name;
        }

        /// <summary>
        /// ラムダ式のメンバーの名前を取得します。
        /// </summary>
        private static string GetMemberNameHelper(LambdaExpression expression)
        {
            Guard.ArgumentNotNull(expression, "expression");

            var member = expression.Body as MemberExpression;
            if (member == null)
            {
                throw new ArgumentException(string.Format("The expression [{0}] is not a field or property access.", expression));
            }

            return member.Member.Name;
        }
    }
}