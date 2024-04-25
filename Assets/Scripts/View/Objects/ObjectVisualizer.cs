namespace View.Objects
{
    using System.Collections.Generic;
    using UnityEngine;
    using View.Interfaces;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(ObjectVisualizerEditorValidator))]
    public abstract class ObjectVisualizer : ExtendedMonoBehaviour, ISlideInitableHidable
    {
        [SerializeField] protected string key;

        private void OnValidate()
        {
            GetComponent<ObjectVisualizerEditorValidator>().TryValidate();
        }

        public abstract void Init();

        public virtual void Hide()
        {
        }

        public abstract List<Component> GetNecessaryComponents();
    }
}