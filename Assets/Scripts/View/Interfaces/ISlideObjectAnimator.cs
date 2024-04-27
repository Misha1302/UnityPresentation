namespace View.Interfaces
{
    using System.Collections;
    using JetBrains.Annotations;

    public interface ISlideObjectAnimator : ISlideObjectComponent
    {
        [CanBeNull] public IEnumerator Show();
        [CanBeNull] public IEnumerator Hide();
    }
}