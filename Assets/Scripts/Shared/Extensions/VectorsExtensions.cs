namespace Shared.Extensions
{
    using UnityEngine;

    public static class VectorsExtensions
    {
        public static Vector3 WithX(this Vector3 vec, float x)
        {
            vec.x = x;
            return vec;
        }

        public static Vector3 WithY(this Vector3 vec, float y)
        {
            vec.y = y;
            return vec;
        }

        public static Vector3 WithZ(this Vector3 vec, float z)
        {
            vec.z = z;
            return vec;
        }
    }
}