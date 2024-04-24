namespace View.Objects
{
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


        protected T GetOrAddComponent<T>() where T : Component
        {
            var hasComponent = gameObject.TryGetComponent<T>(out var audioSource);
            if (!hasComponent) audioSource = gameObject.AddComponent<T>();

            return audioSource;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            EditorApplication.delayCall += () =>
            {
                if (Application.isPlaying)
                    return;

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
            // TODO: добавить в ISlideObjectComponent зависимые компоненты и игнорировать их
            var components = GetComponents<Component>()
                .Where(x => x != null && x is not RectTransform and not Transform and not CanvasRenderer
                    and not ISlideObjectComponent)
                .ToArray();

            if (recDepth == 15)
                throw new LockRecursionException(string.Join(", ", components.Select(x => x.GetType())));

            foreach (var component in components)
                DestroyImmediate(component);

            if (components.Any(x => x != null))
                RemoveAllComponents(recDepth + 1);
        }
#endif
    }
}