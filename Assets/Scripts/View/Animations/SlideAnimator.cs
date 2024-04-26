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
        }

        public override IEnumerator Init()
        {
            SetAlphas();
            return base.Init();
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