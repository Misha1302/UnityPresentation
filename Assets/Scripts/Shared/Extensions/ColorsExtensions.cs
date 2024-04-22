namespace Shared.Extensions
{
    using UnityEngine;

    public static class ColorsExtensions
    {
        public static Color WithA(this Color color, float a)
        {
            color.a = a;
            return color;
        }
    }
}