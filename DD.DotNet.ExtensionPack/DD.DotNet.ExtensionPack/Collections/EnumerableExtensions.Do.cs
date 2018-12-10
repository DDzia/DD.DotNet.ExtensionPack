using System;
using System.Collections.Generic;
using DD.DotNet.ExtensionPack.Validation;

namespace DD.DotNet.ExtensionPack.Collections
{
    public static partial class EnumerableExtensions
    {
        /// <summary>
        /// Handle every for a collection aggregation.
        /// </summary>
        /// <typeparam name="T">Item type of <paramref name="self"/>.</typeparam>
        /// <param name="self">Current collection.</param>
        /// <param name="handler">The handler.</param>
        /// <returns>Same collection.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="self"/> or <paramref name="handler"/> is null.</exception>
        public static IEnumerable<T> Do<T>(this IEnumerable<T> self, Action<T, long> handler)
        {
            Argument.ThrowIfNull(() => self);
            Argument.ThrowIfNull(() => handler);

            long currentIndex = 0;
            foreach (var item in self)
            {
                handler(item, currentIndex++);
                yield return item;
            }
        }

        /// <summary>
        /// Handle every for a collection aggregation.
        /// </summary>
        /// <typeparam name="T">Item type of <paramref name="self"/>.</typeparam>
        /// <param name="self">Current collection.</param>
        /// <param name="handler">The handler.</param>
        /// <returns>Same collection.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="self"/> or <paramref name="handler"/> is null.</exception>
        public static IEnumerable<T> Do<T>(this IEnumerable<T> self, Action<T, int> handler)
        {
            Argument.ThrowIfNull(() => self);
            Argument.ThrowIfNull(() => handler);

            int currentIndex = 0;
            foreach (var item in self)
            {
                handler(item, currentIndex++);
                yield return item;
            }
        }

        /// <summary>
        /// Handle every for a collection aggregation.
        /// </summary>
        /// <typeparam name="T">Item type of <paramref name="self"/>.</typeparam>
        /// <param name="self">Current collection.</param>
        /// <param name="handler">The handler.</param>
        /// <returns>Same collection.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="self"/> or <paramref name="handler"/> is null.</exception>
        public static IEnumerable<T> Do<T>(this IEnumerable<T> self, Action<T> handler)
        {
            Argument.ThrowIfNull(() => handler);

            Action<T, long> wrapHandler = (item, index) => handler(item);
            return Do(self, wrapHandler);
        }

        /// <summary>
        /// Handle every for a collection aggregation.
        /// </summary>
        /// <typeparam name="T">Item type of <paramref name="self"/>.</typeparam>
        /// <param name="self">Current collection.</param>
        /// <param name="handler">The handler.</param>
        /// <returns>Same collection.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="self"/> or <paramref name="handler"/> is null.</exception>
        public static IEnumerable<T> Do<T>(this IEnumerable<T> self, Action handler)
        {
            Argument.ThrowIfNull(() => handler);

            Action<T> wrapHandler = item => handler();
            return Do(self, wrapHandler);
        }
    }
}