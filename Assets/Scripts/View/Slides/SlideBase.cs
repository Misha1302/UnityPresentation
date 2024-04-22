namespace View.Slides
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Shared;
    using Shared.Extensions;
    using UnityEngine;
    using UnityEngine.UI;

    public abstract class SlideBase : MonoBehaviour
    {
        private readonly List<Coroutine> _coroutines = new();

        private void OnDisable()
        {
            foreach (var coroutine in _coroutines.Where(coroutine => coroutine != null))
                StopCoroutine(coroutine);

            _coroutines.Clear();
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);

            foreach (var initable in GetComponentsInChildren<ISlideInitable>())
                initable.Init();
        }

        public virtual void Hide()
        {
            var maxTime = GetComponentsInChildren<ISlideHidable>()
                .Select(initable => initable.Hide())
                .Prepend(0f)
                .Max();

            CoroutinesHelper.StartAfter(() => gameObject.SetActive(false), maxTime);
        }

        public virtual void StartAnimation(
            Graphic image,
            AnimationType animationType,
            float delay,
            float duration,
            bool recursively = true)
        {
            CoroutinesHelper.StartAfter(
                () => StartAnimation(image, animationType, duration, recursively),
                delay
            );
        }

        private void StartAnimation(Graphic image, AnimationType animationType, float duration, bool recursively)
        {
            if (recursively)
            {
                var anyOfThisLevel = image.GetComponents<Graphic>();
                foreach (var img in image.GetComponentsInChildren<Graphic>().Where(x => !anyOfThisLevel.Contains(x)))
                    StartAnimation(img, animationType, duration, true);
            }

            Action a = animationType switch
            {
                AnimationType.Vanishing => () => _coroutines.Add(CoroutinesHelper.Start(Vanishing(image, duration))),
                AnimationType.Appearance => () => _coroutines.Add(CoroutinesHelper.Start(Appearance(image, duration))),
                _ => () => Thrower.ArgumentOutOfRange()
            };

            a();
        }

        private static IEnumerator Appearance(Graphic image, float duration)
        {
            yield return AlphaLerpCoroutine(image, duration, t => t / duration);
        }

        private static IEnumerator Vanishing(Graphic image, float duration)
        {
            yield return AlphaLerpCoroutine(image, duration, t => 1 - t / duration);
        }

        private static IEnumerator AlphaLerpCoroutine(Graphic image, float duration, Func<float, float> alpha)
        {
            var t = 0f;

            while (t < duration)
            {
                image.color = image.color.WithA(alpha(t));
                yield return null;
                t += Time.deltaTime;
            }
        }
    }
}