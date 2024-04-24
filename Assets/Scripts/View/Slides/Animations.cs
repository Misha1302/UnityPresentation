namespace View.Slides
{
    using System;
    using System.Collections;
    using Shared.Extensions;
    using UnityEngine;
    using UnityEngine.UI;

    public static class Animations
    {
        public static IEnumerator Appearance(Graphic image, float duration)
        {
            yield return AlphaLerpCoroutine(image, duration, t => t / duration);
        }

        public static IEnumerator Vanishing(Graphic image, float duration)
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