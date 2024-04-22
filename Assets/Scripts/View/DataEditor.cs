namespace View
{
    using Logic.SaveSystem;
    using Shared.Extensions;
    using TMPro;
    using UnityEngine;

    public class DataEditor : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;

        private void Start()
        {
            inputField.text = JsonUtility.ToJson(DataManager.Instance.ToDto(), true).FromJsonString();
            inputField.onEndEdit.AddListener(SaveChanges);
        }

        private static void SaveChanges(string dataString)
        {
            if (IsValidDataString(dataString, out var dto))
                DataSaver.Save(dto.FromDto());
        }

        private static bool IsValidDataString(string dataString, out DataManagerDto dto)
        {
            dto = JsonUtility.FromJson<DataManagerDto>(dataString.ToJsonString());

            return dto.audio is not null && dto.texts is not null && dto.videos is not null;
        }
    }
}