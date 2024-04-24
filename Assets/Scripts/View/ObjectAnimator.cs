namespace View
{
    using System;
    using System.Collections;
    using Shared;
    using UnityEngine;
    using UnityEngine.UI;
    using View.Slides;

    public class ObjectAnimator : MonoBehaviour, ISlideObjectAnimator
    {
        [SerializeField] private AnimationPlayMoment animationPlayMoment = AnimationPlayMoment.Start;
        [SerializeField] private AnimationType animationType = AnimationType.Appearance;
        [SerializeField] private float delay;
        [SerializeField] private float duration = 1f;

        private void Start()
        {
            Validate();
        }

        public IEnumerator Hide() =>
            animationPlayMoment == AnimationPlayMoment.End ? PlayAnimation(-1) : null;

        public IEnumerator Init() =>
            animationPlayMoment == AnimationPlayMoment.Start ? PlayAnimation(delay) : null;

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
            return slide.StartAnimation(graphic, animationType, animationDelay, duration);
        }
    }
}