namespace View.Objects
{
    using System.IO;
    using Logic.DataSystem;
    using TMPro;
    using UnityEngine;

    public class ObjectText : ObjectVisualizer
    {
        [SerializeField] private float fontSize = 36;
        [SerializeField] private TextAlignmentOptions textAlignment;

        public override void Init()
        {
            SetText();
        }

        private void SetText()
        {
            var text = GetOrAddComponent<TextMeshProUGUI>();

            text.text = File.ReadAllText(Data.Instance.GetTextPath(key));
            text.alignment = textAlignment;
            text.fontSize = fontSize;
        }
    }
}