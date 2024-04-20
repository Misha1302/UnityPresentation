namespace View
{
    using System;
    using System.Linq;
    using UnityEngine;

    public static class GameObjectExtensions
    {
        private static bool Requires(Type obj, Type requirement)
        {
            //also check for m_Type1 and m_Type2 if required
            return Attribute.IsDefined(obj, typeof(RequireComponent)) &&
                   Attribute.GetCustomAttributes(obj, typeof(RequireComponent)).OfType<RequireComponent>()
                       .Any(rc => rc.m_Type0.IsAssignableFrom(requirement));
        }

        internal static bool CanDestroy(this GameObject go, Type t)
        {
            return !go.GetComponents<Component>().Any(c => Requires(c.GetType(), t));
        }
    }
}