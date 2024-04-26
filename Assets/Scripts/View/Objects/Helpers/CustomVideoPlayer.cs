namespace View.Objects.Helpers
{
    using System;
    using Shared.Extensions;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Video;
    using View.Animations;

    [RequireComponent(typeof(VideoPlayer))]
    public class CustomVideoPlayer : MonoBehaviour, ISlideAfterAnimationInitable
    {
        private readonly Lazy<VideoPlayer> _videoPlayer;
        [NonSerialized] public bool AutoPlay = true;

        public CustomVideoPlayer()
        {
            _videoPlayer = new Lazy<VideoPlayer>(GetComponent<VideoPlayer>);
        }

        private void Awake()
        {
            Prepare();
        }

        public void Init()
        {
            if (AutoPlay) Play();
        }

        private void Prepare()
        {
            _videoPlayer.Value.aspectRatio = VideoAspectRatio.Stretch;
            _videoPlayer.Value.Prepare();
            _videoPlayer.Value.waitForFirstFrame = true;
            _videoPlayer.Value.playOnAwake = false;
        }

        public void Init(string url, RenderTexture renderTexture)
        {
            _videoPlayer.Value.url = url;
            _videoPlayer.Value.targetTexture = renderTexture;
            Prepare();
        }

        public void Play()
        {
            var img = GetComponent<RawImage>();
            img.color = img.color.WithA(1);

            _videoPlayer.Value.Play();
        }
    }
}