namespace View
{
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.Networking;

    public static class DataLoader
    {
        public static IEnumerator LoadAudio(string path, Action<AudioClip> callback)
        {
            var www = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.WAV);
            yield return www.SendWebRequest();

            ActResult(www, () => callback(DownloadHandlerAudioClip.GetContent(www)));
        }

        public static IEnumerator LoadImage(string path, Action<Sprite> callback)
        {
            var www = UnityWebRequestTexture.GetTexture(path);
            yield return www.SendWebRequest();

            ActResult(www, () =>
            {
                var texture2D = DownloadHandlerTexture.GetContent(www);
                var position = new Vector2(0, 0);
                var vector2 = new Vector2(texture2D.width, texture2D.height);
                var pivot = new Vector2(0.5f, 0.5f);
                var sprite = Sprite.Create(texture2D, new Rect(position, vector2), pivot);
                callback(sprite);
            });
        }

        private static void ActResult(UnityWebRequest request, Action onSuccess)
        {
            if (request.result == UnityWebRequest.Result.ConnectionError)
                onSuccess();
        }
    }
}