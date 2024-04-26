namespace View.Objects.Helpers
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class ExtendedMonoBehaviour : MonoBehaviour
    {
        private readonly Dictionary<Type, Component> _componentsCache = new();

        protected T GetOrAddComponent<T>() where T : Component
        {
            if (TryGetFromCache(out var value)) return value;
            if (TryGetFromGameObject(out var value2)) return value2;
            return AddComponent();


            bool TryGetFromCache(out T component)
            {
                var hasCache = _componentsCache.TryGetValue(typeof(T), out var valueInternal) && valueInternal != null;
                component = (T)valueInternal;
                return hasCache;
            }

            bool TryGetFromGameObject(out T component)
            {
                var hasComponent = gameObject.TryGetComponent(out component);
                _componentsCache[typeof(T)] = component;
                return hasComponent;
            }

            T AddComponent() => gameObject.AddComponent<T>();
        }
    }
}