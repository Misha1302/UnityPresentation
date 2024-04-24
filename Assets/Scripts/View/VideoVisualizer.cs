namespace View
{
    using Logic.SaveSystem;
    using Shared.Extensions;
    using UnityEngine;
    using UnityEngine.Video;

    public class VideoVisualizer : MonoBehaviour
    {
        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private string videoKey;

        private void Start()
        {
            videoPlayer.aspectRatio = VideoAspectRatio.Stretch;

            videoPlayer.url = DataManager.Instance.Videos
                .FindOrDefaultInstance(x => x.key == videoKey).value.PathToUrl();

            videoPlayer.Play();
        }
    }
}