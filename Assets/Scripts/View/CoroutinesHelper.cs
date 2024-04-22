namespace View
{
    using System.Collections;
    using UnityEngine;

    public static class CoroutinesHelper
    {
        public static Coroutine Start(IEnumerator enumerator)
        {
            var gameObject = new GameObject();
            return gameObject.AddComponent<MonoBeh>().StartCoroutine(ExecuteAndSelfDestruct(enumerator, gameObject));
        }

        private static IEnumerator ExecuteAndSelfDestruct(IEnumerator enumerator, GameObject gameObject)
        {
            yield return enumerator;
            Object.DestroyImmediate(gameObject);
        }

        private sealed class MonoBeh : MonoBehaviour
        {
        }
    }
}