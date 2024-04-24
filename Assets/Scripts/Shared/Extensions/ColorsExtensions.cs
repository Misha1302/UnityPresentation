namespace Shared.Extensions
{
    using JetBrains.Annotations;
    using UnityEngine;

    public static class ColorsExtensions
    {
        [Pure]
        public static Color WithA(this Color color, float a)
        {
            color.a = a;
            return color;
        }
    }
}