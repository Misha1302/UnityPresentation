namespace View.Animations
{
    using System;
    using System.Collections;
    using Shared.Extensions;
    using UnityEngine;
    using UnityEngine.UI;

    public static class Animations
    {
        public static IEnumerator Appearance(Graphic image, float duration, bool repeat)
        {
            do
            {
                yield return LerpCoroutine(duration, t => image.color = image.color.WithA(t));
                // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
            } while (repeat);
        }

        public static IEnumerator Vanishing(Graphic image, float duration, bool repeat)
        {
            do
            {
                yield return LerpCoroutine(duration, t => image.color = image.color.WithA(1 - t));
                // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
            } while (repeat);
        }

        public static IEnumerator Rotate(Graphic image, float duration, Vector3 rotStart, Vector3 rotEnd, bool repeat)
        {
            do
            {
                yield return LerpCoroutine(duration, t => image.transform.rotation = CalcRot(1 - t));
                yield return LerpCoroutine(duration, t => image.transform.rotation = CalcRot(t));
                // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
            } while (repeat);

            yield break;

            Quaternion CalcRot(float t) => Quaternion.Euler(Vector3.Lerp(rotStart, rotEnd, t));
        }

        private static IEnumerator LerpCoroutine(float duration, Action<float> action)
        {
            var t = 0f;

            while (t < duration)
            {
                action(t / duration);
                yield return null;
                t += Time.deltaTime;
            }
        }
    }
}