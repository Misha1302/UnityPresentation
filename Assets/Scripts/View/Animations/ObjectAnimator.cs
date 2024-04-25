namespace View.Animations
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
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
        [SerializeField] private bool recursive = true;
        [SerializeField] private bool includeAnimators;


        private void Start()
        {
            Validate();
        }

        public virtual IEnumerator Hide() => animationPlayMoment.IsNeedPlayInEnd() ? PlayAnimation(-1) : null;

        public virtual IEnumerator Init() => animationPlayMoment.IsNeedPlayInStart() ? PlayAnimation(delay) : null;

        public List<Component> GetNecessaryComponents() => new() { GetComponent<Graphic>() };

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
            return slide.StartAnimation(graphic, animationType, animationDelay, duration,
                animationPlayMoment.IsNeedRepeating(), recursive, includeAnimators);
        }
    }
}