using System;
using System.Collections.Generic;


namespace Tomi.Utils
{

    public static class CollectionUtils
    {

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            if (action == null) return;

            foreach (T item in enumerable) action(item);
        }

    }

}