namespace View.Animations
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Shared.Extensions;
    using UnityEngine.UI;

    public class SlideAnimator : ObjectAnimator
    {
        private Dictionary<Graphic, float> _standardAlphas;

        private void OnValidate()
        {
            recursive = true;

            if (animationPlayMoment.IsNeedPlayInStart()) includeAnimators = false;
            else if (animationPlayMoment.IsNeedPlayInEnd()) includeAnimators = true;

            recursive = animationType != AnimationType.DiagonalRectangleGrid;
        }

        public override IEnumerator Show()
        {
            SetAlphas();
            return InitAnimation(base.Show());
        }

        private IEnumerator InitAnimation(IEnumerator anim)
        {
            yield return anim;
            GetComponentsInChildren<ISlideAfterAnimationInitable>()
                .ForAll(x => x.Init());
        }

        private void SetAlphas()
        {
            if (_standardAlphas == null)
            {
                var components = GetComponentsInChildren<Graphic>();
                _standardAlphas = components.ToDictionary(x => x, GetDefaultAlpha);
            }
            else
            {
                foreach (var pair in _standardAlphas)
                    pair.Key.color = pair.Key.color.WithA(pair.Value);
            }
        }

        private static float GetDefaultAlpha(Graphic x) => x.color.a;
    }
}