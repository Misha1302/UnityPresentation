namespace View
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Logic;
    using TMPro;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Video;

    public class ObjectVisualizer : MonoBehaviour, IDontDestroyMe
    {
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
                    .Where(x => x is not RectTransform and not Transform and not IDontDestroyMe)
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
            var player = GetOrAddComponent<VideoPlayer>();
            var image = GetOrAddComponent<RawImage>();
            var renderTexture = new RenderTexture(Screen.width, Screen.height, 16);

            player.url = Data.Videos.First(_findX).value.ToUrl();

            player.targetTexture = renderTexture;
            image.texture = renderTexture;
        }

        private void SetText()
        {
            GetOrAddComponent<TextMeshProUGUI>().text = Data.Texts.First(_findX).value;
        }

        private void SetAudio()
        {
            StartCoroutine(DataLoader.LoadAudio(
                Data.Audio.First(_findX).value.ToUrl(),
                clip => GetOrAddComponent<AudioSource>().clip = clip
            ));
        }

        private void SetImage()
        {
            StartCoroutine(DataLoader.LoadImage(
                Data.Images.First(_findX).value.ToUrl(),
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

        [DoesNotReturn]
        private static void ArgumentOutOfRange()
        {
            throw new ArgumentOutOfRangeException();
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
                _ => ArgumentOutOfRange
            };

            act?.Invoke();
        }
    }
}