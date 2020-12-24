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

    public abstract class UIList<TItem, TData> : UIBehaviour, IReadOnlyList<TItem>
        where TItem : MonoBehaviour, IItem<TData>
    {
        public struct DataList : IReadOnlyList<TData>
        {
            private readonly UIList<TItem, TData> list;

            internal DataList(UIList<TItem, TData> list) => this.list = list;

            public int Count => list.Count;
            public TData this[int index] => list.items[index].Data;

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
        private Transform container;

        [SerializeField]
        private TItem itemPrefab;

        private readonly List<TItem> items = new List<TItem>();
        

        public int Count { get; private set; }
        public TItem this[int index] => (index < Count) ? items[index] : throw new System.ArgumentOutOfRangeException(nameof(index));

        public DataList Data => new DataList(this);



        protected override void Start()
        {
            base.Start();

            // Try to get the already spawned items
            container.GetComponentsInChildren(true, items);
            if (items.Count <= 0)
                AddItem();
        }

        protected override void Reset()
        {
            base.Reset();
            container = transform;
        }


        public void Populate(IEnumerable<TData> data)
        {
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
            var newItem = Instantiate(items[0], container);
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
