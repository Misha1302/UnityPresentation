namespace View.Objects.Visualizers
{
    using System.Collections.Generic;
    using UnityEngine;
    using View.Interfaces;
    using View.Objects.Helpers;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(ObjectVisualizerEditorValidator))]
    public abstract class ObjectVisualizer : ExtendedMonoBehaviour, ISlideInitable, ISlideShowableHidable
    {
        [SerializeField] protected string key;

        private void OnValidate()
        {
            GetComponent<ObjectVisualizerEditorValidator>().TryValidate();
        }

        public abstract void Init();

        public abstract List<Component> GetNecessaryComponents();

        public virtual void Show()
        {
        }

        public virtual void Hide()
        {
        }

        public abstract void PreShow();
    }
}