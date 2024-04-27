namespace Shared.Extensions
{
    using View.Animations;

    public static class AnimationTypeExtensions
    {
        public static bool RequireGraphic(this AnimationType animationType) =>
            animationType != AnimationType.DiagonalRectangleGrid;
    }
}