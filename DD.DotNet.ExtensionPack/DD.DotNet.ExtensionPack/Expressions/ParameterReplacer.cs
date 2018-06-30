using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using DD.DotNet.ExtensionPack.Utils;

namespace DD.DotNet.ExtensionPack.Expressions
{
    /// <summary>
    /// Visitor to processing(change type) of expression nodes.
    /// </summary>
    /// <seealso cref="System.Linq.Expressions.ExpressionVisitor" />
    internal class ParameterReplacer : ExpressionVisitor
    {
        /// <summary>
        /// Ignore chached parameters clear, because some methods do call current Visit method.
        /// </summary>
        private bool _noClear = false;

        // cached parameters
        private readonly Dictionary<string, ParameterExpression> _visitedParameters = new Dictionary<string, ParameterExpression>();

        private readonly Type _sourceType;
        private readonly Type _targetType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterReplacer"/> class.
        /// </summary>
        /// <param name="sourceType">Current parameter type of expression.</param>
        /// <param name="targetType">New parameter type for expression.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="sourceType"/> or <paramref name="targetType"/> is null.</exception>
        public ParameterReplacer(Type sourceType, Type targetType)
        {
            _sourceType = Check.Argument.ThrowIfNullOrReturn(() => sourceType);
            _targetType = Check.Argument.ThrowIfNullOrReturn(() => targetType);
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            var visitedParams = VisitAndConvert(node.Parameters, "lambdaVisit");
            var visitedBody = Visit(node.Body);

            var lambdaReulst = Expression.Lambda(visitedBody, visitedParams);

            return lambdaReulst;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            var visitedExpr = Visit(node.Expression);

            if (visitedExpr != node.Expression)
            {
                if (node.Member.MemberType == MemberTypes.Property)
                {
                    var pi = node.Member as PropertyInfo;
                    var tProperty = _targetType.GetProperty(pi.Name);
                    if (tProperty == null)
                    {
                        throw new InvalidOperationException($"A property '{pi.Name}' is not exists into target type.");
                    }

                    var propMemberReplacedExpr = Expression.MakeMemberAccess(visitedExpr, tProperty);
                    return propMemberReplacedExpr;
                }

                if (node.Member.MemberType == MemberTypes.Field)
                {
                    var fi = node.Member as FieldInfo;
                    var bFlags = ReflectionUtils.ResolveBindingFlags(fi);
                    var tField = _targetType.GetField(fi.Name, bFlags);
                    if (tField == null)
                    {
                        throw new InvalidOperationException($"A field '{fi.Name}' is not exists into target type.");
                    }

                    var replaced = Expression.Field(visitedExpr, tField);
                    return replaced;
                }
            }

            return base.VisitMember(node);
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var callInstanceExpr = Visit(node.Object);
            if (callInstanceExpr != node.Object)
            {
                var targetMi = ReflectionUtils.FindSameMethod(callInstanceExpr.Type, node.Method);
                var arguments = Visit(node.Arguments);

                if (targetMi == null)
                {
                    throw new InvalidOperationException($"Expression use unsupported method {node.Method.Name}.");
                }
                var call = Expression.Call(callInstanceExpr, targetMi, arguments);
                return call;
            }

            return base.VisitMethodCall(node);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (node.Type == _sourceType)
            {
                if (!_visitedParameters.ContainsKey(node.Name))
                {
                    var replacedParam = Expression.Parameter(_targetType, node.Name);
                    _visitedParameters[node.Name] = replacedParam;
                }

                return _visitedParameters[node.Name];
            }

            return base.VisitParameter(node);
        }

        public override Expression Visit(Expression node)
        {
            if (!_noClear)
            {
                _visitedParameters.Clear();

                _noClear = true;

                var visited = base.Visit(node);

                _noClear = false;

                return visited;
            }

            return base.Visit(node);
        }
    }
}