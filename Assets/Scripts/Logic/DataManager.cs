namespace Logic
{
    using JetBrains.Annotations;

    public class DataManager
    {
        [CanBeNull] private static DataManager _instance;


        public readonly EventList<Pair<string, string>> Audio;
        public readonly EventList<Pair<string, string>> Images;
        public readonly EventList<Pair<string, string>> Texts;
        public readonly EventList<Pair<string, string>> Videos;

        public DataManager(
            EventList<Pair<string, string>> dtoAudio,
            EventList<Pair<string, string>> dtoTexts,
            EventList<Pair<string, string>> dtoVideos,
            EventList<Pair<string, string>> dtoImages
        )
        {
            Texts = dtoTexts;
            Images = dtoImages;
            Videos = dtoVideos;
            Audio = dtoAudio;

            Texts.OnUpdate.AddListener(_ => SaveChanges());
            Videos.OnUpdate.AddListener(_ => SaveChanges());
            Audio.OnUpdate.AddListener(_ => SaveChanges());
            Images.OnUpdate.AddListener(_ => SaveChanges());
        }

        public static DataManager Instance => _instance ??= DataSaver.Load();

        private void SaveChanges() => DataSaver.Save(this);

        public static DataManager NewDefault() =>
            new(
                new EventList<Pair<string, string>>(),
                new EventList<Pair<string, string>> { new("text1", "Hello, World!") },
                new EventList<Pair<string, string>>(),
                new EventList<Pair<string, string>>()
            );
    }
}