namespace Logic.DataSystem
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine.Events;

    public class EventList<T> : IReadOnlyList<T> where T : new()
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


        public T FindOrDefaultInstance(Func<T, bool> predicate) => this.FirstOrDefault(predicate) ?? new T();

        public void Add(T value)
        {
            _list.Add(value);
            OnUpdate.Invoke(_list.Count - 1);
        }
    }
}