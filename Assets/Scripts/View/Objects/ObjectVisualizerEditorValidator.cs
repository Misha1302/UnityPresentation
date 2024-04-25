namespace View.Objects
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;
    using View.Interfaces;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(ObjectVisualizer))]
    public class ObjectVisualizerEditorValidator : MonoBehaviour
    {
        private float _prevCallTime;

        public void TryValidate()
        {
#if UNITY_EDITOR
            const float delay = 0.02f;
            if (Time.time - _prevCallTime < delay) return;
            if (Application.isPlaying) return;

            EditorApplication.delayCall += Validate;
#endif
        }

        private void Validate()
        {
            _prevCallTime = Time.time;
            RemoveAllComponents();
            GetComponent<ObjectVisualizer>().Init();
        }

        private void RemoveAllComponents()
        {
            foreach (var component in GetComponentsToRemove())
                DestroyImmediate(component);
        }

        private List<Component> GetComponentsToRemove()
        {
            var components = GetComponents<Component>().ToList();

            components.RemoveAll(x => x == null || x is Transform or CanvasRenderer or ObjectVisualizerEditorValidator);

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
    }
}