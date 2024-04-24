namespace View
{
    using System.Collections;
    using JetBrains.Annotations;
    using UnityEngine;

    public interface ISlideObjectAnimator
    {
        [CanBeNull] public IEnumerator Init();
        [CanBeNull] public IEnumerator Hide();
    }
}