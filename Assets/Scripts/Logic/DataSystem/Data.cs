namespace Logic.DataSystem
{
    using System;
    using System.IO;

    [Serializable]
    public class Data
    {
        private static readonly Lazy<Data> _instance = new(DataSaver.Load);

        public string audioDirectory;
        public string imagesDirectory;
        public string textsDirectory;
        public string videosDirectory;

        public Data(string audioDirectory, string imagesDirectory, string textsDirectory, string videosDirectory)
        {
            this.audioDirectory = audioDirectory;
            this.imagesDirectory = imagesDirectory;
            this.textsDirectory = textsDirectory;
            this.videosDirectory = videosDirectory;
        }

        public Data() : this(
            @"C:\Users\Modern 14\UnityProjects\UnityPresentation\Assets\Presentation\ИГРА - 2024-ФИНАЛ\ppt\media\VideosAndAudio",
            @"C:\Users\Modern 14\UnityProjects\UnityPresentation\Assets\Presentation\ИГРА - 2024-ФИНАЛ\ppt\media\Images",
            @"C:\Users\Modern 14\UnityProjects\UnityPresentation\Assets\Presentation\ИГРА - 2024-ФИНАЛ\ppt\media\Texts",
            @"C:\Users\Modern 14\UnityProjects\UnityPresentation\Assets\Presentation\ИГРА - 2024-ФИНАЛ\ppt\media\VideosAndAudio"
        )
        {
        }

        public static Data Instance => _instance.Value;

        public string GetAudioPath(string name) => Path.Combine(audioDirectory, name);
        public string GetImagePath(string name) => Path.Combine(imagesDirectory, name);
        public string GetTextPath(string name) => Path.Combine(textsDirectory, name);
        public string GetVideoPath(string name) => Path.Combine(videosDirectory, name);

        private void SaveChanges() => DataSaver.Save(this);
    }
}