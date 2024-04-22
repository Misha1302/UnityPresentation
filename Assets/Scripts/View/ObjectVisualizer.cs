namespace View
{
    using System;
    using System.Linq;
    using Extensions;
    using Logic.SaveSystem;
    using Shared;
    using TMPro;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.UI;

    public class ObjectVisualizer : MonoBehaviour, IDontDestroyMe
    {
        [SerializeField] private ObjectType visualizeType;
        [SerializeField] private string key;

        private void Start()
        {
            ActObjType(SetText, SetAudio, SetImage, SetVideo);
        }


        private T GetOrAddComponent<T>() where T : Component
        {
            var audioSource = gameObject.GetComponent<T>();
            if (audioSource == null)
                audioSource = gameObject.AddComponent<T>();

            return audioSource;
        }

        private void SetVideo()
        {
            var player = GetOrAddComponent<RVideoPlayer>();
            var image = GetOrAddComponent<RawImage>();
            var renderTexture = new RenderTexture(Screen.currentResolution.width, Screen.currentResolution.height, 16);

            player.Init(Data.Videos.FindOrDefaultInstance(_findX).value.ToUrl(), renderTexture);

            image.texture = renderTexture;
        }

        private void SetText()
        {
            GetOrAddComponent<TextMeshProUGUI>().text = Data.Texts.FindOrDefaultInstance(_findX).value;
        }

        private void SetAudio()
        {
            ResourceLoader.LoadAudio(
                Data.Audio.FindOrDefaultInstance(_findX).value.ToUrl(),
                clip => GetOrAddComponent<AudioSource>().clip = clip
            );
        }

        private void SetImage()
        {
            ResourceLoader.LoadImage(Data.Images.FindOrDefaultInstance(_findX).value.ToUrl(), SetSprite);
            return;

            void SetSprite(Sprite sprite)
            {
                var img = GetOrAddComponent<Image>();
                img.sprite = sprite;
                ScaleAsNativeRatio();
                return;

                void ScaleAsNativeRatio()
                {
                    var w = img.rectTransform.rect.width;
                    var h = w * (sprite.rect.height / sprite.rect.width);
                    img.rectTransform.sizeDelta = new Vector2(w, h);
                }
            }
        }


        private void ActObjType(Action textAct, Action audioAct, Action imageAct, Action videoAct)
        {
            var act = visualizeType switch
            {
                ObjectType.None => null,
                ObjectType.Audio => audioAct,
                ObjectType.Image => imageAct,
                ObjectType.Video => videoAct,
                ObjectType.Text => textAct,
                _ => () => Thrower.ArgumentOutOfRange($"Unknown object type: {visualizeType}")
            };

            act?.Invoke();
        }

#if UNITY_EDITOR
        private readonly Func<Pair<string, string>, bool> _findX;

        public ObjectVisualizer()
        {
            _findX = x => x.key == key;
        }

        private static DataManager Data => DataManager.Instance;

        private void OnValidate()
        {
            EditorApplication.delayCall += () =>
            {
                if (Application.isPlaying)
                    return;

                var components = GetComponents<Component>()
                    .Where(x => x != null && x is not RectTransform and not Transform and not IDontDestroyMe)
                    .ToArray();

                for (var index = 0; components.Any(x => x != null); index++)
                {
                    index %= components.Length;

                    var component = components[index];
                    if (component != null && gameObject.CanDestroy(component.GetType()))
                        DestroyImmediate(component);
                }

                ActObjType(SetText, SetAudio, SetImage, SetVideo);
            };
        }
#endif
    }
}