namespace View.Objects.Visualizers
{
    using System.Collections.Generic;
    using Logic.DataSystem;
    using Shared.Extensions;
    using UnityEngine;
    using UnityEngine.UI;
    using View.Objects.Helpers;

    public class ObjectVideo : ObjectVisualizer
    {
        public override void Init()
        {
            SetVideo();
        }

        public override List<Component> GetNecessaryComponents() => new()
            { GetOrAddComponent<CustomVideoPlayer>(), GetOrAddComponent<RawImage>() };

        private void SetVideo()
        {
            var path = Data.Instance.GetVideoPath(key).PathToUrl();
            if (!path.IsValid()) return;

            var player = GetOrAddComponent<CustomVideoPlayer>();
            var image = GetOrAddComponent<RawImage>();
            var renderTexture = new RenderTexture(Screen.currentResolution.width, Screen.currentResolution.height, 16);

            player.Init(path, renderTexture);

            image.texture = renderTexture;
        }
    }
}