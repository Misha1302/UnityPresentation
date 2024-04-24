namespace Shared.Coroutines
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

        public static IEnumerator StartAfterCoroutine(Action act, float time)
        {
            if (time > 0)
                yield return new WaitForSeconds(time);

            act();
        }

        public static IEnumerator StartAfterCoroutine(Action act, Func<bool> waitWhile)
        {
            yield return new WaitWhile(waitWhile);
            act();
        }

        public static void Stop(Coroutine coroutine)
        {
            _gameObject.Value.StopCoroutine(coroutine);
        }


        private sealed class MonoBeh : MonoBehaviour
        {
        }
    }
}