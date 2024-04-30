namespace View.Slides
{
    using System;
    using Shared.Coroutines;
    using Shared.Extensions;
    using UnityEngine;
    using View.Interfaces;

    [RequireComponent(typeof(SlideObjectsController))]
    [RequireComponent(typeof(SlideAnimationsManager))]
    public abstract class SlideBase : MonoBehaviour
    {
        private const int FinishingAnimationsCount = 1;

        [SerializeField] private Transform[] saveToNextSlide;

        private readonly CoroutineManager _coroutineManager = new();

        private readonly Lazy<SlideAnimationsManager> _slideAnimationManager;

        public SlideBase()
        {
            _slideAnimationManager = new Lazy<SlideAnimationsManager>(GetComponent<SlideAnimationsManager>);
        }

        public Transform[] SaveToNextSlide => saveToNextSlide;
        public SlideState SlideState { get; private set; }


        private void OnEnable()
        {
            SlideState = SlideState.WaitingToShow;
        }

        private void OnDisable()
        {
            SlideState = SlideState.Hided;

            StopAll();
        }

        public virtual void Show()
        {
            SlideState = SlideState.Showing;

            StopAll();
            gameObject.SetActive(true);

            GetComponentsInChildren<ISlideShowableHidable>().ForAll(x => x.Show());
            GetComponentsInChildren<ISlideObjectAnimator>().ForAll(
                x => _coroutineManager.StartCor(x.Show())
            );

            SlideState = SlideState.Ready;
        }


        public virtual void Hide()
        {
            SlideState = SlideState.Hiding;
            gameObject.SetActive(true);

            StopAll();
            GetComponentsInChildren<ISlideShowableHidable>().ForAll(x => x.Hide());
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

        private void StopAll()
        {
            _slideAnimationManager.Value.StopAnimations();
            _coroutineManager.StopCors();
        }

        public virtual void HideImmediately()
        {
            gameObject.SetActive(false);
        }

        public void Init()
        {
            GetComponentsInChildren<ISlideInitable>().ForAll(x => x.Init());
        }
    }
}