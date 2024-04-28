namespace View.Objects.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;
    using View.Interfaces;
    using View.Objects.Visualizers;

    [DisallowMultipleComponent]
    public class ObjectVisualizerEditorValidator : MonoBehaviour
    {
        public void TryValidate()
        {
            if (Application.isPlaying)
                return;

#if UNITY_EDITOR
            EditorApplication.delayCall += Validate;
#endif
        }

        private void Validate()
        {
            RemoveAllComponents();
            if (!Application.isPlaying)
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