namespace View
{
    using System.Linq;
    using Logic;
    using UnityEngine;
    using UnityEngine.Video;

    public class VideoVisualizer : MonoBehaviour
    {
        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private string videoKey;

        private void Start()
        {
            videoPlayer.aspectRatio = VideoAspectRatio.Stretch;

            if (DataManager.Instance.Videos.All(x => x.key != videoKey))
                DataManager.Instance.Videos.Add(
                    new Pair<string, string>(videoKey, @"C:\Users\Modern 14\Downloads\video_2024-03-24_11-09-13.mp4")
                );

            videoPlayer.url = DataManager.Instance.Videos.First(x => x.key == videoKey).value.PathToUrl();

            videoPlayer.Play();
        }
    }
}