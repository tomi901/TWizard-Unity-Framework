using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace TWizard.Core.UI
{
    public interface IItem<T>
    {
        T Data { get; set; }
    }

    [System.Serializable] // Generic classes will be serializable in 2020.1 2020.2
    public class UIList<TItem, TData> : IReadOnlyList<TItem>
        where TItem : MonoBehaviour, IItem<TData>
    {
        public struct DataList : IReadOnlyList<TData>
        {
            private readonly UIList<TItem, TData> list;
            private List<TItem> Items => list.items;

            internal DataList(UIList<TItem, TData> list) => this.list = list;

            public int Count => list.Count;
            public TData this[int index]
            {
                get => Items[index].Data;
                set => Items[index].Data = value;
            }

            public IEnumerator<TData> GetEnumerator()
            {
                int i = 0;
                foreach (TItem item in list.items)
                {
                    yield return item.Data;

                    if (i >= Count)
                        yield break;
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }


        [SerializeField]
        private TItem itemPrefab;

        private readonly List<TItem> items = new List<TItem>();

        private readonly Transform container;

        /// <summary>
        /// How many items were given when <see cref="Populate(IEnumerable{TData})"/> was called.
        /// </summary>
        public int Count { get; private set; }
        public TItem this[int index] => (index < Count) ? items[index] : throw new System.ArgumentOutOfRangeException(nameof(index));

        /// <summary>
        /// Wrapper list to access the data directly.
        /// </summary>
        public DataList Data => new DataList(this);


        /// <summary>
        /// Creates a <see cref="UIList{TItem, TData}"/> with a given transform container, the container has to have at least 1 child <see cref="TItem"/>.
        /// </summary>
        /// <param name="container">The parent container for the items, has to have at least 1 child <see cref="TItem"/>.</param>
        public UIList(Transform container)
        {
            this.container = !!container ? container : throw new System.ArgumentNullException(nameof(container));
            container.GetComponentsInChildren(true, items);

            if (items.Count > 0)
                itemPrefab = items[0];
            else
                throw new System.ArgumentException($"Transform container \"{container}\" has to contain at least 1 child item of type \"{nameof(TItem)}\" to use this constructor.",
                    nameof(container));
        }

        /// <summary>
        /// Creates a <see cref="UIList{TItem, TData}"/> with the selected prefab.
        /// </summary>
        /// <param name="container">The parent container for the items.</param>
        /// <param name="itemPrefab">The prefab.</param>
        public UIList(Transform container, TItem itemPrefab)
        {
            this.itemPrefab = !!itemPrefab ? itemPrefab : throw new System.ArgumentNullException(nameof(container));
            this.container = !!container ? container : throw new System.ArgumentNullException(nameof(container));
            container.GetComponentsInChildren(true, items);
        }


        /// <summary>
        /// Populates the list with the items of the given data collection, if null everything will be hidden.
        /// </summary>
        /// <param name="data">The data list, collection or enumerable with the <see cref="TData"/> for each item.</param>
        public void Populate(IEnumerable<TData> data)
        {
            if (data == null)
                data = System.Linq.Enumerable.Empty<TData>();

            int i = 0;
            foreach (TData d in data)
            {
                // Add the missing ones
                if (i >= items.Count)
                    AddItem();

                items[i].gameObject.SetActive(true);
                items[i].Data = d;
                i++;
            }
            // Set the count to allow list accesing
            Count = i;

            // Deactivate the pooled items that we don't really need
            for (int length = items.Count; i < length; i++)
                items[i].gameObject.SetActive(false);
        }

        /// <summary>
        /// Adds a new item to the pool.
        /// </summary>
        /// <returns>The new item.</returns>
        private TItem AddItem()
        {
            var newItem = Object.Instantiate(itemPrefab, container);
            items.Add(newItem);
            return newItem;
        }



        public IEnumerator<TItem> GetEnumerator()
        {
            int i = 0;
            foreach (TItem item in items)
            {
                yield return item;

                if (i >= Count)
                    yield break;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
