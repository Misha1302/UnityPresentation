namespace Shared.Extensions
{
    using View.Animations;

    public static class PlayAnimationMomentExtensions
    {
        public static bool IsNeedPlayInStart(this AnimationPlayMoment animationPlayMoment) =>
            animationPlayMoment is AnimationPlayMoment.Start or AnimationPlayMoment.StartRepeating;

        public static bool IsNeedRepeating(this AnimationPlayMoment animationPlayMoment) =>
            animationPlayMoment == AnimationPlayMoment.StartRepeating;

        public static bool IsNeedPlayInEnd(this AnimationPlayMoment animationPlayMoment) =>
            animationPlayMoment == AnimationPlayMoment.End;
    }
}