namespace Logic.DataSystem
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    [Serializable]
    public class Data
    {
        private static readonly Lazy<Data> _instance = new(DataSaver.Load);

        public string audioDirectory;
        public string imagesDirectory;
        public List<string> textsList;
        public string videosDirectory;

        public Data(string audioDirectory,
            string imagesDirectory,
            List<string> textsList,
            string videosDirectory)
        {
            this.audioDirectory = audioDirectory;
            this.imagesDirectory = imagesDirectory;
            this.textsList = textsList;
            this.videosDirectory = videosDirectory;
        }

        public Data() : this(
            @"Assets\Presentation\ИГРА - 2024-ФИНАЛ\ppt\media\VideosAndAudio",
            @"Assets\Presentation\ИГРА - 2024-ФИНАЛ\ppt\media\Images",
            new List<string> { "Hello", "World", "!" },
            @"Assets\Presentation\ИГРА - 2024-ФИНАЛ\ppt\media\VideosAndAudio"
        )
        {
        }

        public static Data Instance => _instance.Value;

        public string GetAudioPath(string name) => Path.Combine(audioDirectory, name);
        public string GetImagePath(string name) => Path.Combine(imagesDirectory, name);
        public string GetText(int index) => textsList[index];
        public string GetVideoPath(string name) => Path.Combine(videosDirectory, name);

        private void SaveChanges() => DataSaver.Save(this);
    }
}