using System;
using System.Linq.Expressions;

namespace DD.DotNet.ExtensionPack.Validation
{
    public static partial class Argument
    {
        /// <summary>
        /// Throws if <paramref name="o"/> is null or return value argument.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="o">Object to check.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <param name="errorMsg">
        /// Exception message.
        /// If <paramref name="errorMsg"/> is null then wil be used default error message of <see cref="System.ArgumentNullException"/>.
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="o"/> is null.</exception>
        /// </param>
        /// <returns><paramref name="o"/></returns>
        public static T ThrowIfNullOrReturn<T>(T o, string paramName = null, string errorMsg = null)
        {
            ThrowIfNull(o, paramName, errorMsg);
            return o;
        }

        /// <summary>
        /// Throws if argument of <paramref name="returnLambda"/> is null or return this argument.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="returnLambda">Return lambda.</param>
        /// <param name="errorMsg">
        /// Exception message.
        /// If <paramref name="errorMsg"/> is null then wil be used default error message of <see cref="System.ArgumentNullException"/>.
        /// </param>
        /// <exception cref="System.ArgumentNullException">Thrown if argument of <paramref name="returnLambda"/> is null.</exception>
        /// <returns>Argument of <paramref name="returnLambda"/>.</returns>
        public static T ThrowIfNullOrReturn<T>(Expression<Func<T>> returnLambda, string errorMsg = null)
        {
            var (paramName, paramValue) = ExtractNameAndValueOfLambda(returnLambda);
            ThrowIfNull(paramValue, paramName, errorMsg);

            return paramValue;
        }
    }
}