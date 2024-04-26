namespace View.Slides
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Shared.Coroutines;
    using Shared.Exceptions;
    using Shared.Extensions;
    using UnityEngine;
    using UnityEngine.UI;
    using View.Animations;
    using View.Interfaces;

    public abstract class SlideBase : MonoBehaviour
    {
        private const int FinishingAnimationsCount = 1;

        private readonly CoroutineManager _coroutineManager = new();

        public SlideState SlideState { get; private set; }

        private void OnEnable()
        {
            SlideState = SlideState.WaitingToShow;
        }

        private void OnDisable()
        {
            SlideState = SlideState.Hided;

            _coroutineManager.StopCors();
        }

        public virtual void Show()
        {
            SlideState = SlideState.Showing;

            _coroutineManager.StopCors();
            gameObject.SetActive(true);

            GetComponentsInChildren<ISlideInitableHidable>().ForAll(x => x.Init());
            GetComponentsInChildren<ISlideObjectAnimator>().ForAll(
                x => _coroutineManager.StartCor(x.Init())
            );

            SlideState = SlideState.Ready;
        }


        public virtual void Hide()
        {
            SlideState = SlideState.Hiding;
            gameObject.SetActive(true);

            _coroutineManager.StopCors();
            GetComponentsInChildren<ISlideInitableHidable>().ForAll(x => x.Hide());
            GetComponentsInChildren<ISlideObjectAnimator>().ForAll(x =>
                _coroutineManager.StartCor(x.Hide())
            );

            _coroutineManager.StartCor(
                CoroutinesHelper.StartAfterCoroutine(
                    () => gameObject.SetActive(false),
                    () => _coroutineManager.CoroutinesCount != FinishingAnimationsCount
                )
            );
        }

        public virtual void HideImmediately()
        {
            gameObject.SetActive(false);
        }

        public virtual IEnumerator StartAnimation(
            GameObject go,
            AnimationType animationType,
            float delay,
            float duration,
            bool repeat = false,
            bool recursively = true,
            bool includeAnimators = false)
        {
            return CoroutinesHelper.StartAfterCoroutine(
                () => StartAnimation(go, animationType, duration, repeat, recursively, includeAnimators),
                delay
            );
        }

        private void StartAnimation(
            GameObject go,
            AnimationType animationType,
            float duration,
            bool repeat,
            bool recursively,
            bool includeAnimators)
        {
            if (recursively)
            {
                var enumerable = GetComponentsToAnimate(go, includeAnimators);
                foreach (var img in enumerable)
                    StartAnimation(img.gameObject, animationType, duration, repeat, false, includeAnimators);
            }

            var graphic = go.GetComponent<Graphic>();
            if (graphic == null) return;

            Action a = animationType switch
            {
                AnimationType.Vanishing => () =>
                    _coroutineManager.StartCor(Animations.Vanishing(graphic, duration, repeat)),
                AnimationType.Appearance => () =>
                    _coroutineManager.StartCor(Animations.Appearance(graphic, duration, repeat)),
                AnimationType.Rotating => () =>
                    _coroutineManager.StartCor(
                        Animations.Rotate(graphic, duration, Vector3.zero, Vector3.forward * 360f, repeat)
                    ),
                _ => () => Thrower.ArgumentOutOfRange()
            };

            a();
        }

        private static IEnumerable<Graphic> GetComponentsToAnimate(GameObject go, bool includeAnimators)
        {
            var img = go.GetComponent<Graphic>();

            var enumerable = go.GetComponentsInChildren<Graphic>(true)
                .Where(x => includeAnimators || !x.TryGetComponent<ObjectAnimator>(out _))
                .Where(x => x != img);

            return enumerable;
        }
    }
}