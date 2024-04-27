namespace View.Animations
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Shared.Extensions;
    using UnityEngine.UI;
    using UnityEngine.Video;

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
            HideVideos();
            return InitAnimation(base.Show());
        }

        private IEnumerator InitAnimation(IEnumerator anim)
        {
            yield return anim;
            GetComponentsInChildren<ISlideAfterAnimationInitable>()
                .ForAll(x => x.Init());
        }

        public override IEnumerator Hide()
        {
            HideVideos();
            return base.Hide();
        }

        private List<RawImage> GetVideoImages()
        {
            return GetComponentsInChildren<VideoPlayer>()
                .Select(x => x.GetComponent<RawImage>())
                .ToList();
        }

        private void HideVideos()
        {
            GetVideoImages().ForEach(x => x.color = x.color.WithA(0));
        }

        private void SetAlphas()
        {
            if (_standardAlphas == null)
            {
                var components = GetComponentsInChildren<Graphic>();
                _standardAlphas = components.ToDictionary(x => x, x => x.color.a);
            }
            else
            {
                foreach (var pair in _standardAlphas)
                    pair.Key.color = pair.Key.color.WithA(pair.Value);
            }
        }
    }
}