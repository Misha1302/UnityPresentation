namespace View
{
    using System;
    using System.Collections;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public static class CoroutinesHelper
    {
        private static readonly Lazy<MonoBeh> _gameObject =
            new(() =>
            {
                var obj = Object.FindObjectOfType<MonoBeh>();
                return obj != null ? obj : new GameObject().AddComponent<MonoBeh>();
            });

        public static Coroutine Start(IEnumerator enumerator) =>
            _gameObject.Value.StartCoroutine(enumerator);

        public static Coroutine StartAfter(Action act, float time) =>
            Start(StartAfterCoroutine(act, time));

        private static IEnumerator StartAfterCoroutine(Action act, float time)
        {
            const float almostZero = 1e-7f;
            if (time > almostZero) 
                yield return new WaitForSeconds(time);

            act();
        }


        private sealed class MonoBeh : MonoBehaviour
        {
        }
    }
}