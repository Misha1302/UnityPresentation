namespace Logic
{
    using System.IO;
    using UnityEngine;

    public static class DataSaver
    {
        private const string Key = "game_data.json";
        private static readonly string _path = Path.Combine(Application.persistentDataPath, Key);

        public static void Save(DataManager dataManager)
        {
            var dataManagerDto = dataManager.ToDto();
            var value = JsonUtility.ToJson(dataManagerDto, true);
            File.WriteAllText(_path, value);
        }

        public static DataManager Load()
        {
            Debug.Log(_path);

            if (!File.Exists(_path))
                return DataManager.NewEmpty();


            var json = File.ReadAllText(_path);

            return string.IsNullOrWhiteSpace(json)
                ? DataManager.NewEmpty()
                : JsonUtility.FromJson<DataManagerDto>(json).FromDto();
        }
    }
}