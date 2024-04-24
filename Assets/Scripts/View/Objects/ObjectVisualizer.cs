namespace View.Objects
{
    using System;
    using System.Linq;
    using Logic.DataSystem;
    using Shared.Exceptions;
    using Shared.Extensions;
    using Shared.ResourceLoader;
    using TMPro;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.UI;
    using View.Interfaces;

    public class ObjectVisualizer : MonoBehaviour, ISlideInitableHidable
    {
        [SerializeField] private ObjectType visualizeType;
        [SerializeField] private string key;

        public void Init()
        {
            ActObjType(SetText, SetAudio, SetImage, SetVideo);
        }

        public void Hide()
        {
        }


        private T GetOrAddComponent<T>() where T : Component
        {
            var hasComponent = gameObject.TryGetComponent<T>(out var audioSource);
            if (!hasComponent) audioSource = gameObject.AddComponent<T>();

            return audioSource;
        }

        private void SetVideo()
        {
            var player = GetOrAddComponent<RVideoPlayer>();
            var image = GetOrAddComponent<RawImage>();
            var renderTexture = new RenderTexture(Screen.currentResolution.width, Screen.currentResolution.height, 16);

            player.Init(Data.Videos.FindOrDefaultInstance(_findX).value.PathToUrl(), renderTexture);

            image.texture = renderTexture;
        }

        private void SetText()
        {
            GetOrAddComponent<TextMeshProUGUI>().text = Data.Texts.FindOrDefaultInstance(_findX).value;
        }

        private void SetAudio()
        {
            ResourceLoader.LoadAudio(
                Data.Audio.FindOrDefaultInstance(_findX).value.PathToUrl(),
                clip => GetOrAddComponent<AudioSource>().clip = clip
            );
        }

        private void SetImage()
        {
            ResourceLoader.LoadImage(Data.Images.FindOrDefaultInstance(_findX).value.PathToUrl(), SetSprite);
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
                    .Where(x => x != null && x is not RectTransform and not Transform and not ISlideObjectComponent)
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