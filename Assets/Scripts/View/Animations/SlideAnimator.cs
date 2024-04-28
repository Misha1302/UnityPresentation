namespace View.Animations
{
    using System.Collections;
    using Shared.Extensions;

    public class SlideAnimator : ObjectAnimator
    {
        private void OnValidate()
        {
            recursive = true;

            if (animationPlayMoment.IsNeedPlayInStart()) includeAnimators = false;
            else if (animationPlayMoment.IsNeedPlayInEnd()) includeAnimators = true;

            recursive = animationType != AnimationType.DiagonalRectangleGrid;
        }

        public override IEnumerator Show() => InitAnimation(base.Show());

        private IEnumerator InitAnimation(IEnumerator anim)
        {
            yield return anim;
            GetComponentsInChildren<ISlideAfterAnimationInitable>()
                .ForAll(x => x.Init());
        }
    }
}