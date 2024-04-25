namespace View.Objects
{
    using System.Collections.Generic;
    using Logic.DataSystem;
    using TMPro;
    using UnityEngine;

    public class ObjectText : ObjectVisualizer
    {
        [SerializeField] private float fontSize = 36;
        [SerializeField] private TextAlignmentOptions textAlignment;

        public override List<Component> GetNecessaryComponents() => new() { GetOrAddComponent<TextMeshProUGUI>() };

        public override void Init()
        {
            SetText();
        }

        private void SetText()
        {
            if (!int.TryParse(key, out var index)) return;

            var text = GetOrAddComponent<TextMeshProUGUI>();
            text.text = Data.Instance.GetText(index);
            text.alignment = textAlignment;
            text.fontSize = fontSize;
            text.enableWordWrapping = false;
        }
    }
}