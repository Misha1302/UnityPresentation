namespace View.Objects
{
    using System.Collections.Generic;
    using Logic.DataSystem;
    using Shared.Extensions;
    using UnityEngine;
    using UnityEngine.UI;

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
            var player = GetOrAddComponent<CustomVideoPlayer>();
            var image = GetOrAddComponent<RawImage>();
            var renderTexture = new RenderTexture(Screen.currentResolution.width, Screen.currentResolution.height, 16);

            player.Init(Data.Instance.GetVideoPath(key).PathToUrl(), renderTexture);

            image.texture = renderTexture;
        }
    }
}