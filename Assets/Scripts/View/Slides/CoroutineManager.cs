namespace View.Slides
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class CoroutineManager
    {
        private readonly List<CustomCoroutine> _coroutines = new();
        public int CoroutinesCount => _coroutines.Count;

        public void StartCor(IEnumerator init, [CallerLineNumber] int lineNumber = -1)
        {
            AddAndRemoveCoroutine(init, $"{lineNumber}");
        }

        private void AddAndRemoveCoroutine(IEnumerator init, string debugInfo)
        {
            var coroutine = new CustomCoroutine(init, debugInfo);

            coroutine.Start(
                () => _coroutines.Add(coroutine),
                () => _coroutines.Remove(coroutine)
            );
        }

        public void StopCors()
        {
            foreach (var coroutine in _coroutines)
                coroutine.Stop();

            _coroutines.Clear();
        }
    }
}