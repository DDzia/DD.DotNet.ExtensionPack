using System;
using System.Linq;
using System.Reflection;
using DD.DotNet.ExtensionPack.Validation;

namespace DD.DotNet.ExtensionPack.Utils
{
    /// <summary>
    /// Reflection utils.
    /// </summary>
    internal static class ReflectionUtils
    {
        #region resolve binding flags

        /// <summary>
        /// Resolve a binding flags of <see cref="System.Reflection.MethodInfo"/>.
        /// </summary>
        /// <param name="mi">Method info.</param>
        /// <returns>Reflected <see cref="System.Reflection.BindingFlags"/></returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="mi"/> is null.</exception>
        public static BindingFlags ResolveBindingFlags(MethodInfo mi)
        {
            Argument.ThrowIfNull(() => mi);

            var flags = BindingFlags.InvokeMethod;

            // resolve an access modificator
            var accessFlag = mi.IsPublic
                ? BindingFlags.Public
                : BindingFlags.NonPublic;
            flags = flags | accessFlag;

            // resolve an owner(static of type or instance)
            var instanceOwn = mi.IsStatic
                ? BindingFlags.Static
                : BindingFlags.Instance;
            flags = flags | instanceOwn;

            return flags;
        }

        /// <summary>
        /// Resolve a  binding flags of <see cref="System.Reflection.FieldInfo"/>.
        /// </summary>
        /// <param name="fi">Field info.</param>
        /// <returns>Reflected <see cref="System.Reflection.BindingFlags"/></returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="fi"/> is null.</exception>
        public static BindingFlags ResolveBindingFlags(FieldInfo fi)
        {
            Argument.ThrowIfNull(() => fi);

            var flags = BindingFlags.InvokeMethod;

            // resolve an access modificator
            var accessFlag = fi.IsPublic
                ? BindingFlags.Public
                : BindingFlags.NonPublic;
            flags = flags | accessFlag;

            // resolve an owner(static of type or instance)
            var instanceOwn = fi.IsStatic
                ? BindingFlags.Static
                : BindingFlags.Instance;
            flags = flags | instanceOwn;

            return flags;
        }

        #endregion resolve binding flags

        /// <summary>
        /// Finds the same method into <paramref name="searchType"/>.
        /// </summary>
        /// <param name="searchType">Type to search.</param>
        /// <param name="sourceMi">Method standard.</param>
        /// <returns>Founded <see cref="System.Reflection.MethodInfo"/> or null.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="searchType"/> or <paramref name="sourceMi"/> is null.</exception>
        public static MethodInfo FindSameMethod(Type searchType, MethodInfo sourceMi)
        {
            Argument.ThrowIfNull(() => searchType);
            Argument.ThrowIfNull(() => sourceMi);

            var bindings = ResolveBindingFlags(sourceMi);

            var originParameters = sourceMi.GetParameters();

            return searchType.GetMethods(bindings)
                // method with same name
                .Where(x => x.Name == sourceMi.Name)
                // method with same return type
                .Where(x => x.ReturnType == sourceMi.ReturnType)
                // method with same parameters count
                .Where(x => x.GetParameters().Length == originParameters.Length)
                .Where(current =>
                {
                    var parametersOfCurrent = current.GetParameters();

                    for (int positionOfSignature = 0; positionOfSignature < parametersOfCurrent.Length; positionOfSignature++)
                    {
                        var originParameter = originParameters[positionOfSignature];
                        var currentParameter = parametersOfCurrent[positionOfSignature];

                        var sameType = originParameter.ParameterType == currentParameter.ParameterType;

                        if (!sameType)
                        {
                            return false;
                        }
                    }

                    return true;
                })
                .FirstOrDefault();
        }
    }
}