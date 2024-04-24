namespace Shared.Coroutines
{
    using System;
    using System.Collections;
    using JetBrains.Annotations;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public static class CoroutinesHelper
    {
        [CanBeNull] private static MonoBeh _gameObject;

        private static MonoBeh GameObject
        {
            get
            {
                if (_gameObject == null)
                    _gameObject = GetGameObject();

                return _gameObject;
            }
        }

        private static MonoBeh GetGameObject()
        {
            var obj = Object.FindObjectOfType<MonoBeh>();
            return obj != null ? obj : new GameObject().AddComponent<MonoBeh>();
        }

        public static Coroutine Start(IEnumerator enumerator) =>
            GameObject.StartCoroutine(enumerator);

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
            GameObject.StopCoroutine(coroutine);
        }


        private sealed class MonoBeh : MonoBehaviour
        {
        }
    }
}