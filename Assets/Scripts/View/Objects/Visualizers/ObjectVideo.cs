namespace View.Objects.Visualizers
{
    using System;
    using System.Collections.Generic;
    using Logic.DataSystem;
    using Shared.Coroutines;
    using Shared.Extensions;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Video;
    using View.Objects.Helpers;

    public class ObjectVideo : ObjectVisualizer
    {
        [SerializeField] private bool autoPlay = true;
        [SerializeField] private float delay;

        private readonly CoroutineManager _coroutineManager = new();

        private void OnDisable()
        {
            _coroutineManager.StopCors();
        }

        public override void Init()
        {
            SetVideo();
        }

        public override void Hide()
        {
            SetAlpha(0);
            base.Hide();
        }

        public override void Show()
        {
            SetAlpha(0);
            if (!autoPlay) return;

            Action play = GetComponent<CustomVideoPlayer>().Play;
            var coroutine = CoroutinesHelper.StartAfterCoroutine(play, delay);
            _coroutineManager.StartCor(coroutine);
        }

        private void SetAlpha(int a)
        {
            var g = GetComponent<Graphic>();
            g.color = g.color.WithA(a);
        }

        public override List<Component> GetNecessaryComponents() => new()
            { GetOrAddComponent<CustomVideoPlayer>(), GetOrAddComponent<VideoPlayer>(), GetOrAddComponent<RawImage>() };

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