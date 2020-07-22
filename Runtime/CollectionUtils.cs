using System;
using System.Collections;
using System.Collections.Generic;

namespace TWizard.Framework
{
    public static class CollectionUtils
    {
        public static void Shuffle<T>(this T list, Random rng = null, int range = int.MaxValue) where T : IList
        {
            if (rng == null)
                rng = new Random();

            int count = list.Count;
            range = Math.Min(range, list.Count);

            for (int n = 0; n < range; n++)
            {
                int randomIndex = rng.Next(n, count);
                object temp = list[n];
                list[n] = list[randomIndex];
                list[randomIndex] = temp;
            }
        }

        public static T SelectRandom<T>(this IReadOnlyList<T> list, Random rng = null)
        {
            if (list.Count <= 0)
                throw new ArgumentException("List count is 0.", nameof(list));

            if (rng == null)
                rng = new Random();

            return list[rng.Next(0, list.Count)];
        }


        public static ReadOnlyCollection<T> ToReadOnly<T>(this IReadOnlyCollection<T> collection)
        {
            return new ReadOnlyCollection<T>(collection);
        }

        public static ReadOnlyList<T> ToReadOnly<T>(this IReadOnlyList<T> collection)
        {
            return new ReadOnlyList<T>(collection);
        }
    }


    public struct ReadOnlyCollection<T> : IReadOnlyCollection<T>
    {
        private readonly IReadOnlyCollection<T> collection;

        public int Count => collection.Count;


        public ReadOnlyCollection(IReadOnlyCollection<T> collection)
        {
            this.collection = collection;
        }


        public IEnumerator<T> GetEnumerator() => collection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => collection.GetEnumerator();
    }

    /// <summary>
    /// A struct version of <see cref="System.Collections.ObjectModel.ReadOnlyCollection{T}"/> to save allocations
    /// and protect a list or array from being modified when exposed.
    /// </summary>
    /// <typeparam name="T">The list type.</typeparam>
    public struct ReadOnlyList<T> : IReadOnlyList<T>
    {
        private readonly IReadOnlyList<T> collection;

        public int Count => collection.Count;

        public T this[int index] => collection[index];


        public ReadOnlyList(IReadOnlyList<T> collection)
        {
            this.collection = collection;
        }


        public IEnumerator<T> GetEnumerator() => collection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => collection.GetEnumerator();
    }
}
