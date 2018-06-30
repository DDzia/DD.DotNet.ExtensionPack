using System;
using System.Linq.Expressions;
using CheckArgument = DD.DotNet.ExtensionPack.Check.Argument;


namespace DD.DotNet.ExtensionPack.Expressions
{
    public static class ExpressionExtensions
    {
        #region To Lambda

        /// <summary>
        /// Convert expression to lambda.
        /// </summary>
        /// <typeparam name="TParam0">Parameter type.</typeparam>
        /// <typeparam name="TResult">Result type.</typeparam>
        /// <param name="expression">Current expression.</param>
        /// <returns>Converted lambda expression.</returns>
        /// <exception cref="ArgumentNullException">Thown if <paramref name="expression"/> is null.</exception>
        public static Expression<Func<TParam0, TResult>> ToLambda<TParam0, TResult>(this Expression expression)
        {
            CheckArgument.ThrowIfNull(() => expression);

            if (expression is Expression<Func<TParam0, TResult>> readyLambda)
            {
                return readyLambda;
            }

            var lambda = Expression.Lambda<Func<TParam0, TResult>>(expression);
            return lambda;
        }

        /// <summary>
        /// Convert expression to lambda.
        /// </summary>
        /// <typeparam name="TResult">Result type.</typeparam>
        /// <param name="expression">Current expression.</param>
        /// <returns>Converted lambda expression.</returns>
        /// <exception cref="ArgumentNullException">Thown if <paramref name="expression"/> is null.</exception>
        public static Expression<Func<TResult>> ToLambda<TResult>(this Expression expression)
        {
            CheckArgument.ThrowIfNull(() => expression);

            if (expression is Expression<Func<TResult>> readyLambda)
            {
                return readyLambda;
            }

            var lambda = Expression.Lambda<Func<TResult>>(expression);
            return lambda;
        }

        #endregion To Lambda

        /// <summary>
        /// Change parameter type of expression.
        /// </summary>
        /// <typeparam name="TSource">Source parameter type.</typeparam>
        /// <typeparam name="TTarget">Target parameter type.</typeparam>
        /// <param name="expression">Current expression.</param>
        /// <returns>New expression with changed parameters.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="expression"/> is null.</exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown if <typeparamref name="TSource"/> operations into body of expressoin incompatibility with <typeparamref name="TTarget"/>(method signature, property of type, field of type and etc.).
        /// </exception>
        public static Expression ChangeParameterType<TSource, TTarget>(this Expression expression)
        {
            CheckArgument.ThrowIfNull(() => expression);

            var replaced = ChangeParameterType(expression, typeof(TSource), typeof(TTarget));
            return replaced;
        }

        /// <summary>
        /// Change parameter type of expression.
        /// </summary>
        /// <param name="expression">Current expression.</param>
        /// <param name="sourceType">Source parameter type.</param>
        /// <param name="targetType">Target parameter type.</param>
        /// <returns>New expression with changed parameters.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="expression"/> is null.</exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown if <paramref name="sourceType"/> operations into body of expressoin incompatibility with <paramref name="targetType"/>(method signature, property of type, field of type and etc.).
        /// </exception>
        private static Expression ChangeParameterType(this Expression expression, Type sourceType, Type targetType)
        {
            var replaceVisitor = new ParameterReplacer(sourceType, targetType);
            var replaced = replaceVisitor.Visit(expression);

            return replaced;
        }
    }
}