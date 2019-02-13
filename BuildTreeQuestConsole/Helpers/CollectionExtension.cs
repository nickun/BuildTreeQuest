using System;
using System.Collections.Generic;

namespace BuildTreeQuestConsole.Helpers
{
    public static class CollectionExtension
    {
        /// <summary>
        /// It's like List&lt;T&gt;.ForEach
        /// </summary>
        public static void ForEachExt<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            if (enumeration == null)
                return;
            foreach (T item in enumeration)
            {
                action(item);
            }
        }

        public static void ForEachExt<T>(this IEnumerable<T> enumeration, Action<T, int> action)
        {
            if (enumeration == null)
                return;

            int index = 0;
            foreach (var item in enumeration)
                action(item, index++);
        }
    }
}