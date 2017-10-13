using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Citadel.Infrastructure
{
    public class TrackedList<T> : IEnumerable<T>, IEnumerable
    {
        private readonly IList<TrackedItem<T>> _trackedItems;

        public TrackedList(IEnumerable<T> source)
        {
            if(source != null && source.Any())
            {
                _trackedItems = new List<TrackedItem<T>>(source.Select(x => new TrackedItem<T>(x, TrackedItemActivity.None)));
            }
            else
            {
                _trackedItems = new List<TrackedItem<T>>();
            }
        }

        public TrackedList() : this(null)
        {

        }

        public T this[int index]
        {
            get
            {
                return _trackedItems[index].Item;
            }
            set
            {
                _trackedItems[index].Activity = TrackedItemActivity.Remove;
                Add(value);
            }
        }

        public void Add(T item) => _trackedItems.Add(new TrackedItem<T>(item, TrackedItemActivity.Add));

        public void Remove(T item)
        {
            var trackedItem = _trackedItems.FirstOrDefault(x => x.Item.Equals(item));
            if (trackedItem == null) return;
            if (trackedItem.Activity == TrackedItemActivity.Add) _trackedItems.Remove(trackedItem);
            trackedItem.Activity = TrackedItemActivity.Remove;
        }

        public IReadOnlyList<TrackedItem<T>> GetTrackedItems() => new List<TrackedItem<T>>(_trackedItems);

        public IEnumerator<T> GetEnumerator() => _trackedItems.Select(x => x.Item).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
