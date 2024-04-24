namespace Shared.ResourceLoader
{
    using System;
    using System.Collections;
    using Shared.Coroutines;
    using UnityEngine;
    using UnityEngine.Networking;

    public static class ResourceLoader
    {
        public static void LoadAudio(string path, Action<AudioClip> callback)
        {
            CoroutinesHelper.Start(
                IfSuccess(
                    UnityWebRequestMultimedia.GetAudioClip(path, AudioType.MPEG),
                    www => callback(DownloadHandlerAudioClip.GetContent(www))
                )
            );
        }

        public static void LoadImage(string path, Action<Sprite> callback)
        {
            CoroutinesHelper.Start(
                IfSuccess(
                    UnityWebRequestTexture.GetTexture(path),
                    www =>
                    {
                        var texture2D = DownloadHandlerTexture.GetContent(www);
                        var position = new Vector2(0, 0);
                        var vector2 = new Vector2(texture2D.width, texture2D.height);
                        var pivot = new Vector2(0.5f, 0.5f);
                        var sprite = Sprite.Create(texture2D, new Rect(position, vector2), pivot);
                        callback(sprite);
                    }
                )
            );
        }

        private static IEnumerator IfSuccess(UnityWebRequest www, Action<UnityWebRequest> onSuccess)
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
                onSuccess(www);
        }
    }
}