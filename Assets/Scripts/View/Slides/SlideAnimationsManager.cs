namespace View.Slides
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Shared.Coroutines;
    using Shared.Extensions;
    using UnityEngine;
    using UnityEngine.UI;
    using View.Animations;

    public class SlideAnimationsManager : MonoBehaviour
    {
        private readonly AnimationsAsActions _animationsAsActions = new();

        public IEnumerator StartAnimation(
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
                    // set recursively to false 'cause GetComponentsToAnimate returns all components on all levels
                    StartAnimation(img.gameObject, animationType, duration, repeat, false, includeAnimators);
            }

            var graphic = go.GetComponent<Graphic>();
            if (animationType.RequireGraphic() && graphic == null)
                return;

            _animationsAsActions.GetStartAnimationAction(animationType, duration, repeat, graphic)();
        }

        private static IEnumerable<Graphic> GetComponentsToAnimate(GameObject go, bool includeAnimators)
        {
            var img = go.GetComponent<Graphic>();

            var enumerable = go.GetComponentsInChildren<Graphic>(true)
                .Where(x => includeAnimators || !x.TryGetComponent<ObjectAnimator>(out _))
                .Where(x => x != img);

            return enumerable;
        }

        public void StopAnimations()
        {
            _animationsAsActions.StopCors();
        }
    }
}