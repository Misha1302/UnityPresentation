namespace View.Animations
{
    using System;
    using System.Collections;
    using Shared.Exceptions;
    using Shared.Extensions;
    using UnityEngine;
    using UnityEngine.UI;
    using View.Interfaces;
    using View.Slides;

    public class ObjectAnimator : MonoBehaviour, ISlideObjectAnimator
    {
        [SerializeField] private AnimationPlayMoment animationPlayMoment = AnimationPlayMoment.Start;
        [SerializeField] private AnimationType animationType = AnimationType.Appearance;
        [SerializeField] private float delay;
        [SerializeField] private float duration = 1f;

        private float _standardAlpha = -1;

        private void Start()
        {
            Validate();
        }

        private void OnEnable()
        {
            var graphic = GetComponent<Graphic>();
            if (Mathf.Approximately(_standardAlpha, -1))
                _standardAlpha = GetComponent<Graphic>().color.a;
            graphic.color = graphic.color.WithA(_standardAlpha);
        }

        public IEnumerator Hide() =>
            animationPlayMoment == AnimationPlayMoment.End ? PlayAnimation(-1) : null;

        public IEnumerator Init() =>
            IsNeedPlayInStart() ? PlayAnimation(delay) : null;

        private void Validate()
        {
            if (animationPlayMoment == AnimationPlayMoment.Invalid)
                Thrower.Throw<InvalidOperationException>($"{nameof(animationPlayMoment)} cannot be Invalid");
            if (animationType == AnimationType.Invalid)
                Thrower.Throw<InvalidOperationException>($"{nameof(animationType)} cannot be Invalid");
        }

        private IEnumerator PlayAnimation(float animationDelay)
        {
            var graphic = GetComponent<Graphic>();
            var slide = GetComponentInParent<SlideBase>();
            return slide.StartAnimation(graphic, animationType, animationDelay, duration, IsNeedRepeating());
        }

        private bool IsNeedPlayInStart() =>
            animationPlayMoment is AnimationPlayMoment.Start or AnimationPlayMoment.StartRepeating;

        private bool IsNeedRepeating() => animationPlayMoment == AnimationPlayMoment.StartRepeating;
    }
}