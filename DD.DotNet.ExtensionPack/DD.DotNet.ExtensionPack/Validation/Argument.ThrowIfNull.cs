using System;
using System.Linq.Expressions;

namespace DD.DotNet.ExtensionPack.Validation
{
    /// <summary>
    /// An argument checking helpers.
    /// </summary>
    public static partial class Argument
    {
        /// <summary>
        /// Throws if null.
        /// </summary>
        /// <typeparam name="T">Argument type.</typeparam>
        /// <param name="o">Object to check.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="o"/> is null.</exception>
        public static void ThrowIfNull<T>(T o)
        {
            ThrowIfNull(o, null, null);
        }

        /// <summary>
        /// Throws if null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o">Object to check.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="errorMsg">
        /// Exception message.
        /// If <paramref name="errorMsg"/> is null then wil be used default error message of <see cref="System.ArgumentNullException"/>.
        /// </param>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="o"/> is null.</exception>
        public static void ThrowIfNull<T>(T o, string paramName, string errorMsg = null)
        {
            ThrowIfNullInternal(o, paramName, errorMsg);
        }

        /// <summary>
        /// Throws if argument of <paramref name="returnLambda"/> is null.
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="returnLambda">Return object lambda.</param>
        /// <param name="errorMsg">
        /// Exception message.
        /// If <paramref name="errorMsg"/> is null then wil be used default error message of <see cref="System.ArgumentNullException"/>.
        /// </param>
        /// <exception cref="System.ArgumentNullException">Thrown if arguemnt of <paramref name="returnLambda"/> is null.</exception>
        public static void ThrowIfNull<T>(Expression<Func<T>> returnLambda, string errorMsg = null)
        {
            var (paramName, paramValue) = ExtractNameAndValueOfLambda(returnLambda);
            ThrowIfNull(paramValue, paramName, errorMsg);
        }

        #region Helpers

        private static void ThrowIfNullInternal<T>(T value, string paramName = null, string errorMsg = null)
        {
            if (value == null)
            {
                ArgumentNullException ex = null;

                if (!string.IsNullOrEmpty(paramName) && !string.IsNullOrEmpty(errorMsg))
                {
                    ex = new ArgumentNullException(paramName, errorMsg);
                }
                else if (!string.IsNullOrEmpty(paramName) && string.IsNullOrEmpty(errorMsg))
                {
                    ex = new ArgumentNullException(paramName);
                }
                else
                {
                    ex = new ArgumentNullException();
                }

                throw ex;
            }
        }

        private static (string paramName, T paramValue) ExtractNameAndValueOfLambda<T>(Expression<Func<T>> value)
        {
            if (value is LambdaExpression lambdaExpr && lambdaExpr.Body is MemberExpression mExpr)
            {
                var memberName = mExpr.Member.Name;
                var constantValue = value.Compile()();
                return (memberName, constantValue);
            }

            throw new ArgumentException("Invalid expression argument.");
        }

        #endregion
    }
}
