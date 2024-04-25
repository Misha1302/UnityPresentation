namespace View.Objects
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using UnityEditor;
    using UnityEngine;
    using View.Interfaces;

    public abstract class ObjectVisualizer : MonoBehaviour, ISlideInitableHidable
    {
        [SerializeField] protected string key;

        public abstract void Init();

        public virtual void Hide()
        {
        }

        public abstract List<Component> GetNecessaryComponents();


        protected T GetOrAddComponent<T>() where T : Component
        {
            var hasComponent = gameObject.TryGetComponent<T>(out var audioSource);
            if (!hasComponent) audioSource = gameObject.AddComponent<T>();

            return audioSource;
        }

#if UNITY_EDITOR
        private float _prevCallTime;

        private void OnValidate()
        {
            EditorApplication.delayCall += () =>
            {
                if (Application.isPlaying)
                    return;

                if (Time.time - _prevCallTime < 0.1f)
                    return;

                _prevCallTime = Time.time;

                RemoveAllComponents();

                try
                {
                    Init();
                }
                catch
                {
                    // ignored
                }
            };
        }

        private void RemoveAllComponents(int recDepth = 0)
        {
            var components = GetComponentsToRemove();

            if (recDepth == 15)
                throw new LockRecursionException(string.Join(", ", components.Select(x => x.GetType())));

            foreach (var component in components)
                DestroyImmediate(component);

            if (components.Any(x => x != null))
                RemoveAllComponents(recDepth + 1);
        }

        private List<Component> GetComponentsToRemove()
        {
            var components = GetComponents<Component>().ToList();

            components.RemoveAll(x => x == null || x is RectTransform or Transform or CanvasRenderer);

            for (var i = 0; i < components.Count; i++)
            {
                if (components[i] is not ISlideObjectComponent slideComponent)
                    continue;

                var necessaryComponents = slideComponent.GetNecessaryComponents();
                var oldCount = components.Count;
                components.RemoveAll(x => necessaryComponents.Contains(x));
                if (components.Count != oldCount) i = 0;
            }

            components.RemoveAll(x => x is ISlideObjectComponent);
            return components;
        }
#endif
    }
}