namespace Extensions
{
    using System;
    using System.Linq;
    using System.Reflection;
    using JetBrains.Annotations;
    using UnityEngine;

    public static class GameObjectExtensions
    {
        [Pure]
        private static bool Requires(MemberInfo obj, Type requirement)
        {
            return Attribute.IsDefined(obj, typeof(RequireComponent)) &&
                   Attribute.GetCustomAttributes(obj, typeof(RequireComponent)).OfType<RequireComponent>()
                       .Any(rc => rc.m_Type0.IsAssignableFrom(requirement));
        }

        [Pure]
        public static bool CanDestroy(this GameObject go, Type t)
        {
            return !go.GetComponents<Component>().Any(c => Requires(c.GetType(), t));
        }
    }
}