namespace Logic
{
    using UnityEngine;

    public static class DataSaver
    {
        private const string Key = "game_data";

        public static void Save(DataManager dataManager)
        {
            var dataManagerDto = dataManager.ToDto();
            var value = JsonUtility.ToJson(dataManagerDto, true);
            PlayerPrefs.SetString(Key, value);
        }

        public static DataManager Load()
        {
            var json = PlayerPrefs.GetString(Key, string.Empty);

            return string.IsNullOrWhiteSpace(json)
                ? DataManager.NewEmpty()
                : JsonUtility.FromJson<DataManagerDto>(json).FromDto();
        }
    }
}