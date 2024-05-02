namespace View.Slides
{
    using System.Collections.Generic;
    using Shared.Extensions;
    using UnityEngine;
    using UnityEngine.UI;
    using View.Interfaces;

    public class SlideObjectsController : MonoBehaviour, ISlideShowableHidable
    {
        private Dictionary<Graphic, ObjectStartInfo> _objectsStartInfos;

        public List<Component> GetNecessaryComponents() => new();

        public void Show()
        {
            SetInfo();
        }

        public void Hide()
        {
        }


        private void SetInfo()
        {
            if (_objectsStartInfos == null)
                InitInfo();
            else UseInfo();
        }

        private void UseInfo()
        {
            foreach (var pair in _objectsStartInfos)
            {
                pair.Key.color = pair.Value.defaultColor;
                pair.Key.rectTransform.position = pair.Value.defaultPosition;
                pair.Key.rectTransform.rotation = Quaternion.Euler(pair.Value.defaultRotation);
                pair.Key.rectTransform.localScale = pair.Value.defaultScale;
            }
        }

        private void InitInfo()
        {
            _objectsStartInfos = new Dictionary<Graphic, ObjectStartInfo>();
            GetComponentsInChildren<Graphic>(true).ForAll(x =>
                _objectsStartInfos.Add(x, MakeInfo(x))
            );
        }

        private static ObjectStartInfo MakeInfo(Graphic x) =>
            new(
                x.color,
                x.rectTransform.position,
                x.rectTransform.rotation.eulerAngles,
                x.rectTransform.localScale
            );
    }
}