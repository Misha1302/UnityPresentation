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
            var startValue = 0;
            const float endValue = 1f;

            do
            {
                yield return LerpCoroutine(duration,
                    t => image.color = image.color.WithA(Mathf.Lerp(startValue, endValue, t)));
                // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
            } while (repeat);
        }

        public static IEnumerator Vanishing(Graphic image, float duration, bool repeat)
        {
            var startValue = image.color.a;
            const float endValue = 0f;

            do
            {
                yield return LerpCoroutine(duration,
                    t => image.color = image.color.WithA(Mathf.Lerp(startValue, endValue, t)));
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

        public static IEnumerator JumpingOut(Graphic graphic, float duration)
        {
            var endPos = graphic.rectTransform.position;

            yield return LerpCoroutine(duration, x => JumpFunction(graphic, x, endPos));
        }

        private static void JumpFunction(Graphic graphic, float x, Vector3 endPos)
        {
            // see graph to understand this code:
            // https://www.desmos.com/Calculator/gsqpheznmy?lang=ru

            const float xMaxValue = 4.222f; // maximum value that x takes on the chart
            const float xCoefficient = 100f; // 1 x = xCoefficient meters in unity
            const float yCoefficient = 100f; // 1 y = yCoefficient meters in unity

            // move the position by 1 x to the left, as the object will move 1 * xCoefficient to the right
            endPos = endPos.WithX(endPos.x - xCoefficient);

            // implementation of the piecewise function
            var y = x switch
            {
                <= 1.87f / xMaxValue => -Mathf.Pow(x * xMaxValue, 2) + 5,
                <= 3.506f / xMaxValue => -Mathf.Pow(x * xMaxValue - 2.5765f, 2) * 3f + 3f,
                _ => -Mathf.Pow(x * xMaxValue - 3.8f, 2) * 4.5f + 0.8f
            };

            // convert X to a horizontal deviation, and the resulting Y to a vertical deflection
            var offset = new Vector3(x * xCoefficient, y * yCoefficient, 0);

            graphic.rectTransform.position = endPos + offset;
        }
    }
}