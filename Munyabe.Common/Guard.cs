using System;
using System.Diagnostics;

namespace Munyabe.Common
{
    /// <summary>
    /// パラメーターが契約を満たしていることを示すためのクラスです。
    /// </summary>
    [DebuggerStepThrough]
    public static class Guard
    {
        /// <summary>
        /// パラメーターが正数であることを示します。
        /// </summary>
        /// <param name="argumentValue">チェックするパラメーター</param>
        /// <param name="argumentName">チェックするパラメーター名</param>
        /// <exception cref="ArgumentException"><paramref name="argumentValue"/>が正数ではありません。</exception>
        public static void ArgumentIsPositive(int argumentValue, string argumentName)
        {
            if (argumentValue < 1)
            {
                throw new ArgumentException(string.Format("The int argument [{0}] must be positive.", argumentName));
            }
        }

        /// <summary>
        /// パラメーターが<see langword="null"/>でないことを示します。
        /// </summary>
        /// <typeparam name="T">パラメーターの型</typeparam>
        /// <param name="argumentValue">チェックするパラメーター</param>
        /// <param name="argumentName">チェックするパラメーター名</param>
        /// <exception cref="ArgumentNullException"><paramref name="argumentValue"/>が<see langword="null"/>です。</exception>
        public static void ArgumentNotNull<T>(T argumentValue, string argumentName) where T : class
        {
            if (argumentValue == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        /// <summary>
        /// パラメーターが<see langword="null"/>もしくは空文字でないことを示します。
        /// </summary>
        /// <exception cref="ArgumentNullException">パラメーターが<see langword="null"/>のときに発生します。</exception>
        /// <exception cref="ArgumentException">パラメーターが空文字のときに発生します。</exception>
        /// <param name="argumentValue">チェックするパラメーター</param>
        /// <param name="argumentName">チェックするパラメーター名</param>
        /// <exception cref="ArgumentNullException"><paramref name="argumentValue"/>が<see langword="null"/>です。</exception>
        /// <exception cref="ArgumentException"><paramref name="argumentValue"/>が空文字です。</exception>
        public static void ArgumentNotNullOrEmpty(string argumentValue, string argumentName)
        {
            ArgumentNotNull(argumentValue, argumentName);

            if (argumentValue.Length == 0)
            {
                throw new ArgumentException("The string argument [{0}] must not be empty.", argumentName);
            }
        }

        /// <summary>
        /// パラメーターのインスタンスが指定のクラスから割り当てられることを示します。
        /// </summary>
        /// <param name="assignmentTargetType">割り当てる予定のクラス</param>
        /// <param name="assignmentInstance">チェックするインスタンス</param>
        /// <param name="argumentName">チェックするパラメーター名</param>
        /// <exception cref="ArgumentException"><paramref name="assignmentInstance"/>は<paramref name="assignmentTargetType"/>に割り当てられません。</exception>
        public static void InstanceIsAssignable(Type assignmentTargetType, object assignmentInstance, string argumentName)
        {
            ArgumentNotNull(assignmentTargetType, "assignmentTargetType");
            ArgumentNotNull(assignmentInstance, "assignmentInstance");

            if (assignmentTargetType.IsInstanceOfType(assignmentInstance) == false)
            {
                throw new ArgumentException(
                    string.Format("The instance type [{0}] cannot be assigned to variables of the type [{1}].",
                        assignmentInstance.GetType().FullName, assignmentTargetType),
                    argumentName);
            }
        }

        /// <summary>
        /// パラメーターのクラスが指定のクラスから割り当てられることを示します。
        /// </summary>
        /// <param name="assignmentTargetType">割り当てる予定のクラス</param>
        /// <param name="assignmentValueType">チェックするクラス</param>
        /// <param name="argumentName">チェックするパラメーター名</param>
        /// <exception cref="ArgumentException"><paramref name="assignmentValueType"/>は<paramref name="assignmentTargetType"/>に割り当てられません。</exception>
        public static void TypeIsAssignable(Type assignmentTargetType, Type assignmentValueType, string argumentName)
        {
            ArgumentNotNull(assignmentTargetType, "assignmentTargetType");
            ArgumentNotNull(assignmentValueType, "assignmentValueType");

            if (assignmentTargetType.IsAssignableFrom(assignmentValueType) == false)
            {
                throw new ArgumentException(
                    string.Format("The type [{0}] cannot be assigned to variables of the type [{1}].", assignmentValueType, assignmentTargetType),
                    argumentName);
            }
        }

        /// <summary>
        /// パラメーターのクラスが指定のクラスに一致することを示します。
        /// </summary>
        /// <param name="argumentTargetType">一致する予定のクラス</param>
        /// <param name="argumentType">チェックするクラス</param>
        /// <param name="argumentName">チェックするパラメーター名</param>
        /// <exception cref="ArgumentException"><paramref name="argumentType"/>は<paramref name="argumentTargetType"/>に一致しません。</exception>
        public static void TypeIsEqual(Type argumentTargetType, Type argumentType, string argumentName)
        {
            ArgumentNotNull(argumentTargetType, "argumentTargetType");
            ArgumentNotNull(argumentType, "argumentType");

            if (argumentTargetType != argumentType)
            {
                throw new ArgumentException(
                    string.Format("The type [{0}] cannot be equal to the type [{1}].", argumentType, argumentTargetType), argumentName);
            }
        }

        /// <summary>
        /// パラメーターのクラスがジェネッリック型でないことを示します。
        /// </summary>
        /// <param name="argumentType">チェックするクラス</param>
        /// <param name="argumentName">チェックするパラメーター名</param>
        /// <exception cref="ArgumentException"><paramref name="argumentType"/>はジェネリック型です。</exception>
        public static void TypeNotGeneric(Type argumentType, string argumentName)
        {
            ArgumentNotNull(argumentType, "argumentType");

            if (argumentType.IsGenericType)
            {
                throw new ArgumentException(
                    string.Format("The type [{0}] cannot be generic.", argumentType), argumentName);
            }
        }
    }
}