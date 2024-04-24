namespace View.Slides
{
    using System;
    using System.Collections;
    using JetBrains.Annotations;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class CustomCoroutine
    {
        public readonly string DebugData;

        private readonly IEnumerator _enumerator;
        private Coroutine _coroutine;

        public CustomCoroutine(IEnumerator enumerator, string debugData)
        {
            _enumerator = enumerator;
            DebugData = debugData;
        }

        public void Start([CanBeNull] Action onStart, [CanBeNull] Action onEnd)
        {
            _coroutine = CoroutinesHelper.Start(StartCor(onStart, onEnd));
        }

        private IEnumerator StartCor([CanBeNull] Action onStart, [CanBeNull] Action onEnd)
        {
            onStart?.Invoke();
            yield return _enumerator;
            onEnd?.Invoke();
        }

        public void Stop()
        {
            if (SceneManager.GetActiveScene().isLoaded)
                CoroutinesHelper.Stop(_coroutine);
        }
    }
}