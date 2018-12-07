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
        
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T, int> action, int startIndex = 0)
        {
            if (action == null) return;
            
            int i = startIndex;
            foreach (T item in enumerable) action(item, i++);
        }

    }

}
