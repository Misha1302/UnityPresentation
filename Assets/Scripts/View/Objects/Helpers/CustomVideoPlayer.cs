namespace View.Objects.Helpers
{
    using System;
    using Shared.Extensions;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Video;

    [RequireComponent(typeof(VideoPlayer))]
    public class CustomVideoPlayer : MonoBehaviour
    {
        private readonly Lazy<VideoPlayer> _videoPlayer;

        public CustomVideoPlayer()
        {
            _videoPlayer = new Lazy<VideoPlayer>(GetComponent<VideoPlayer>);
        }

        private void SetUpVideoPlayer()
        {
            _videoPlayer.Value.aspectRatio = VideoAspectRatio.Stretch;
            _videoPlayer.Value.waitForFirstFrame = true;
            _videoPlayer.Value.playOnAwake = false;
        }

        public void Init(string url, RenderTexture renderTexture)
        {
            _videoPlayer.Value.url = url;
            _videoPlayer.Value.targetTexture = renderTexture;
            SetUpVideoPlayer();
        }

        public void Play()
        {
            var img = GetComponent<RawImage>();
            img.color = img.color.WithA(1);

            _videoPlayer.Value.Play();
        }
    }
}