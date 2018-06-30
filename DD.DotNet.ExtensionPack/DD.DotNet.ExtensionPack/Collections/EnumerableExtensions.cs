using System;
using System.Collections.Generic;
using System.Linq;

namespace DD.DotNet.ExtensionPack.Collections
{
    /// <summary>
    /// Method extensions for <see cref="IEnumerable{T}"/> types.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Cycrcle collection with <paramref name="handler"/>
        /// </summary>
        /// <typeparam name="T">Object type of collection.</typeparam>
        /// <param name="self">Current collection.</param>
        /// <param name="handler">Each item handler.</param>
        /// <exception cref="ArgumentNullException">Throw if <paramref name="self"/> or <paramref name="handler"/> is null.</exception>
        public static void ForEach<T>(this IEnumerable<T> self, Action<T, long> handler)
        {
            Check.Argument.ThrowIfNull(() => self);
            Check.Argument.ThrowIfNull(() => handler);

            var enumerable = self as T[] ?? self.ToArray();
            for (var index = 0; index < enumerable.LongCount(); index++)
            {
                handler(enumerable.ElementAt(index), index);
            }
        }

        /// <summary>
        /// Cycrcle collection with <paramref name="handler"/>
        /// </summary>
        /// <typeparam name="T">Object type of collection.</typeparam>
        /// <param name="self">Current collection.</param>
        /// <param name="handler">Each item handler.</param>
        /// <exception cref="ArgumentNullException">Throw if <paramref name="self"/> or <paramref name="handler"/> is null.</exception>
        public static void ForEach<T>(this IEnumerable<T> self, Action<T> handler)
        {
            Check.Argument.ThrowIfNull(() => handler);

            ForEach(self, (item, index) => handler(item));
        }
    }
}