namespace Shared.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class CollectionsExtensions
    {
        public static IEnumerable<T> Except<T>(this IEnumerable<T> c, params int[] indices)
        {
            return c.Where((_, i) => !indices.Contains(i));
        }

        public static void ForAll<T>(this IEnumerable<T> c, Action<T> act)
        {
            foreach (var item in c)
                act(item);
        }

        public static void ForAll<T>(this T[] arr, Action<T> act)
        {
            arr.ForAll((_, item) => act(item));
        }

        public static void ForAll<T>(this T[] arr, Action<int, T> act)
        {
            for (var i = 0; i < arr.Length; i++)
                act(i, arr[i]);
        }
    }
}