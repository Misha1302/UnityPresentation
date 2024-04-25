namespace View.Objects
{
    using System.Collections.Generic;
    using Logic.DataSystem;
    using Shared.Extensions;
    using Shared.ResourceLoader;
    using UnityEngine;
    using UnityEngine.UI;

    [ExecuteAlways]
    public class ObjectImage : ObjectVisualizer
    {
        private void Update()
        {
#if !DEBUG
            return;
#endif

            var img = GetOrAddComponent<Image>();
            ScaleAsNativeRatio(img);

            if (img.sprite == null) SetImage();
        }

        public override List<Component> GetNecessaryComponents() => new() { GetOrAddComponent<Image>() };

        public override void Init()
        {
            SetImage();
        }

        private void SetImage()
        {
            ResourceLoader.LoadImage(Data.Instance.GetImagePath(key).PathToUrl(), SetSprite);
            return;

            void SetSprite(Sprite sprite)
            {
                var img = GetOrAddComponent<Image>();
                img.sprite = sprite;
                ScaleAsNativeRatio(img);
            }
        }

        private static void ScaleAsNativeRatio(Image img)
        {
            if (img.sprite == null)
                return;

            var w = img.rectTransform.rect.width;
            var h = w * (img.sprite.rect.height / img.sprite.rect.width);
            img.rectTransform.sizeDelta = new Vector2(w, h);
        }
    }
}