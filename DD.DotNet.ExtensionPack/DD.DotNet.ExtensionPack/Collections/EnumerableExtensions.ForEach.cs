using System;
using System.Collections.Generic;
using System.Linq;
using DD.DotNet.ExtensionPack.Validation;

namespace DD.DotNet.ExtensionPack.Collections
{
    public static partial class EnumerableExtensions
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
            Argument.ThrowIfNull(() => self);
            Argument.ThrowIfNull(() => handler);

            var enumerable = self as T[] ?? self.ToArray();
            for (long index = 0; index < enumerable.LongLength; index++)
            {
                handler(enumerable[index], index);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> self, Action<T, int> handler)
        {
            Argument.ThrowIfNull(() => self);
            Argument.ThrowIfNull(() => handler);

            var enumerable = self as T[] ?? self.ToArray();
            for (int index = 0; index < enumerable.Length; index++)
            {
                handler(enumerable[index], index);
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
            Argument.ThrowIfNull(() => handler);

            Action<T, long> wrapper = (item, index) => handler(item);

            ForEach(self, wrapper);
        }
    }
}