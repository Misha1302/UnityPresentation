namespace View.Slides
{
    using System;
    using System.Collections;
    using EasyTransition;
    using Shared.Coroutines;
    using Shared.Exceptions;
    using Shared.Extensions;
    using UnityEngine;
    using UnityEngine.UI;
    using View.Animations;

    public class AnimationsAsActions
    {
        private readonly CoroutineManager _coroutineManager = new();

        public void StopCors()
        {
            _coroutineManager.StopCors();
        }

        public Action GetStartAnimationAction(AnimationType animationType, float duration, bool repeat, Graphic graphic)
        {
            return animationType switch
            {
                AnimationType.Vanishing => () => Vanishing(duration, repeat, graphic),
                AnimationType.Appearance => () => Appearance(duration, repeat, graphic),
                AnimationType.Rotating => () => Rotating(duration, repeat, graphic),
                AnimationType.JumpingOut => () => JumpingOut(duration, graphic),
                AnimationType.FlyOutFromDown => () => FlyOutFromDown(duration, graphic),
                AnimationType.Pulsation => () => Pulsation(duration, repeat, graphic),
                AnimationType.Swing => () => Swing(duration, repeat, graphic),
                AnimationType.DiagonalRectangleGrid => () => DiagonalRectangleGrid(duration),
                _ => Thrower.Throw<ArgumentOutOfRangeException>
            };
        }

        private void DiagonalRectangleGrid(float duration)
        {
            _coroutineManager.StartCor(
                ActAndWait(() =>
                        TransitionManager.Instance().Transition(
                            Resources.Load<TransitionSettings>("Transitions/DiagonalRectangleGrid"), 0
                        ),
                    duration
                )
            );
        }

        private void Swing(float duration, bool repeat, Graphic graphic)
        {
            var rot = graphic.rectTransform.rotation.eulerAngles;
            _coroutineManager.StartCor(
                Animations.Rotate(graphic, duration, rot, rot.WithZ(rot.z + 20f), repeat)
            );
        }

        private void Pulsation(float duration, bool repeat, Graphic graphic)
        {
            _coroutineManager.StartCor(Animations.Pulsation(graphic, duration, repeat));
        }

        private void FlyOutFromDown(float duration, Graphic graphic)
        {
            _coroutineManager.StartCor(Animations.FlyOutFromDown(graphic, duration));
        }

        private void JumpingOut(float duration, Graphic graphic)
        {
            _coroutineManager.StartCor(Animations.JumpingOut(graphic, duration));
        }

        private void Rotating(float duration, bool repeat, Graphic graphic)
        {
            var rot = graphic.rectTransform.rotation.eulerAngles;
            _coroutineManager.StartCor(
                Animations.Rotate(graphic, duration, rot, rot.WithZ(rot.y + 360f), repeat)
            );
        }

        private void Appearance(float duration, bool repeat, Graphic graphic)
        {
            _coroutineManager.StartCor(Animations.Appearance(graphic, duration, repeat));
        }

        private void Vanishing(float duration, bool repeat, Graphic graphic)
        {
            _coroutineManager.StartCor(Animations.Vanishing(graphic, duration, repeat));
        }


        private static IEnumerator ActAndWait(Action action, float duration)
        {
            action();
            yield return new WaitForSeconds(duration);
        }
    }
}