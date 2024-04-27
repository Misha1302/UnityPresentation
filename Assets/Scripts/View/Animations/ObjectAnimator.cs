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
        [SerializeField] protected AnimationPlayMoment animationPlayMoment = AnimationPlayMoment.Start;
        [SerializeField] protected AnimationType animationType = AnimationType.Appearance;
        [SerializeField] protected float delay;
        [SerializeField] protected float duration = 1f;
        [SerializeField] protected bool recursive = true;
        [SerializeField] protected bool includeAnimators;


        private void Start()
        {
            Validate();
        }

        public virtual IEnumerator Hide() => animationPlayMoment.IsNeedPlayInEnd() ? PlayAnimation(-1) : null;

        public virtual IEnumerator Show() => animationPlayMoment.IsNeedPlayInStart() ? PlayAnimation(delay) : null;

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
            var slide = GetComponentInParent<SlideBase>();
            return slide.StartAnimation(gameObject, animationType, animationDelay, duration,
                animationPlayMoment.IsNeedRepeating(), recursive, includeAnimators);
        }
    }
}