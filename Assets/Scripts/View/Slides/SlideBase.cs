namespace View.Slides
{
    using System;
    using System.Collections;
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
            Graphic image,
            AnimationType animationType,
            float delay,
            float duration,
            bool recursively = true)
        {
            return CoroutinesHelper.StartAfterCoroutine(
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
                AnimationType.Vanishing => () =>
                    _coroutineManager.StartCor(Animations.Vanishing(image, duration)),
                AnimationType.Appearance => () =>
                    _coroutineManager.StartCor(Animations.Appearance(image, duration)),
                _ => () => Thrower.ArgumentOutOfRange()
            };

            a();
        }
    }
}