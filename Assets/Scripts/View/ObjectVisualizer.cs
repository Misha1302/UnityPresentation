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
#if UNITY_EDITOR
        [SerializeField] private ObjectType visualizeType;
        [SerializeField] private string key;

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
                    .Where(x => x is not null && x is not RectTransform and not Transform and not IDontDestroyMe)
                    .ToArray();

                for (var index = 0; components.Any(x => x != null); index++)
                {
                    index %= components.Length;

                    var component = components[index];
                    if (component != null && gameObject.CanDestroy(component.GetType()))
                        DestroyImmediate(component);
                }

                ActObjType(
                    SetText,
                    SetAudio,
                    SetImage,
                    SetVideo
                );
            };
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
            StartCoroutine(DataLoader.LoadAudio(
                Data.Audio.FindOrDefaultInstance(_findX).value.ToUrl(),
                clip => GetOrAddComponent<AudioSource>().clip = clip
            ));
        }

        private void SetImage()
        {
            StartCoroutine(DataLoader.LoadImage(
                Data.Images.FindOrDefaultInstance(_findX).value.ToUrl(),
                sprite => GetOrAddComponent<Image>().sprite = sprite
            ));
        }

        private T GetOrAddComponent<T>() where T : Component
        {
            var audioSource = gameObject.GetComponent<T>();
            if (audioSource == null)
                audioSource = gameObject.AddComponent<T>();

            return audioSource;
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
#endif
    }
}