namespace View.Objects
{
    using System;
    using UnityEngine;
    using UnityEngine.Video;

    [RequireComponent(typeof(VideoPlayer))]
    public class RVideoPlayer : MonoBehaviour
    {
        private readonly Lazy<VideoPlayer> _videoPlayer;

        public RVideoPlayer()
        {
            _videoPlayer = new Lazy<VideoPlayer>(GetComponent<VideoPlayer>);
        }

        private void Awake()
        {
            Prepare();
        }

        private void Prepare()
        {
            _videoPlayer.Value.aspectRatio = VideoAspectRatio.Stretch;
            _videoPlayer.Value.Prepare();
            _videoPlayer.Value.waitForFirstFrame = true;
        }

        public void Init(string url, RenderTexture renderTexture)
        {
            _videoPlayer.Value.url = url;
            _videoPlayer.Value.targetTexture = renderTexture;
            Prepare();
        }

        public void Play()
        {
            _videoPlayer.Value.Play();
        }
    }
}