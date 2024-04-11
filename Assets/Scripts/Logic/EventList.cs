using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Logic
{
    public class EventList<T> : IReadOnlyList<T>
    {
        public readonly UnityEvent<int> OnUpdate = new();

        private readonly List<T> _list = new();

        public EventList()
        {
        }

        public EventList(List<T> list)
        {
            _list = list;
        }

        public T this[int index]
        {
            get => _list[index];
            set
            {
                _list[index] = value;
                OnUpdate.Invoke(index);
            }
        }

        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count => _list.Count;

        public void Add(T value)
        {
            _list.Add(value);
            OnUpdate.Invoke(_list.Count - 1);
        }
    }
}