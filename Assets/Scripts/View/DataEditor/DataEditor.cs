namespace View.DataEditor
{
    using Logic.DataSystem;
    using Shared.Extensions;
    using TMPro;
    using UnityEngine;

    public class DataEditor : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;

        private void Start()
        {
            inputField.text = JsonUtility.ToJson(Data.Instance, true).FromJsonString();
            inputField.onEndEdit.AddListener(SaveChanges);
        }

        private static void SaveChanges(string dataString)
        {
            if (IsValidDataString(dataString, out var dto))
                DataSaver.Save(dto);
        }

        private static bool IsValidDataString(string dataString, out Data dto)
        {
            dto = JsonUtility.FromJson<Data>(dataString.ToJsonString());

            return !string.IsNullOrWhiteSpace(dto.audioDirectory)
                   && !string.IsNullOrWhiteSpace(dto.imagesDirectory)
                   && !string.IsNullOrWhiteSpace(dto.textsDirectory)
                   && !string.IsNullOrWhiteSpace(dto.textsDirectory);
        }
    }
}