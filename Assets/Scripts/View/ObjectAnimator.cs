namespace View
{
    using System;
    using Shared;
    using UnityEngine;
    using UnityEngine.UI;
    using View.Slides;

    public class ObjectAnimator : MonoBehaviour, ISlideInitable, ISlideHidable
    {
        [SerializeField] private AnimationPlayMoment animationPlayMoment = AnimationPlayMoment.Start;
        [SerializeField] private AnimationType animationType = AnimationType.Appearance;
        [SerializeField] private float delay;
        [SerializeField] private float duration = 1f;

        private void Start()
        {
            Validate();
        }

        public float Hide()
        {
            if (animationPlayMoment == AnimationPlayMoment.End)
                PlayAnimation(0);

            return duration;
        }

        public void Init()
        {
            SubscribeAnimationIfNeed();
        }

        private void Validate()
        {
            if (animationPlayMoment == AnimationPlayMoment.Invalid)
                Thrower.Throw<InvalidOperationException>($"{nameof(animationPlayMoment)} cannot be Invalid");
            if (animationType == AnimationType.Invalid)
                Thrower.Throw<InvalidOperationException>($"{nameof(animationType)} cannot be Invalid");
        }

        private void SubscribeAnimationIfNeed()
        {
            if (animationPlayMoment == AnimationPlayMoment.Start)
                PlayAnimation(delay);
        }

        private void PlayAnimation(float animationDelay)
        {
            var graphic = GetComponent<Graphic>();
            var slide = GetComponentInParent<SlideBase>();
            slide.StartAnimation(graphic, animationType, animationDelay, duration);
        }
    }
}