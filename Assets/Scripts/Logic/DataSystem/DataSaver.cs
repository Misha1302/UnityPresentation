namespace Logic.DataSystem
{
    using System.IO;
    using Shared.Debug;
    using UnityEngine;

    public static class DataSaver
    {
        private const string FileName = "game_data.json";
        private static readonly string _path = Path.Combine(Application.persistentDataPath, FileName);

        static DataSaver()
        {
            DataLogger.Log(_path);
        }

        public static void Save(Data data)
        {
            var value = JsonUtility.ToJson(data, true);
            File.WriteAllText(_path, value);
        }

        public static Data Load()
        {
            if (!File.Exists(_path))
                return new Data();

            var json = File.ReadAllText(_path);

            return string.IsNullOrWhiteSpace(json)
                ? new Data()
                : JsonUtility.FromJson<Data>(json);
        }
    }
}