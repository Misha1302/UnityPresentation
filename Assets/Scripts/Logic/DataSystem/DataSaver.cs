namespace Logic.DataSystem
{
    using System.IO;
    using UnityEngine;

    public static class DataSaver
    {
        private const string FileName = "game_data.json";
        private static readonly string _path = Path.Combine(Application.persistentDataPath, FileName);

        public static void Save(DataManager dataManager)
        {
            var dataManagerDto = dataManager.ToDto();
            var value = JsonUtility.ToJson(dataManagerDto, true);
            File.WriteAllText(_path, value);
        }

        public static DataManager Load()
        {
            if (!File.Exists(_path))
                return DataManager.NewDefault();

            var json = File.ReadAllText(_path);

            return string.IsNullOrWhiteSpace(json)
                ? DataManager.NewDefault()
                : JsonUtility.FromJson<DataManagerDto>(json).FromDto();
        }
    }
}