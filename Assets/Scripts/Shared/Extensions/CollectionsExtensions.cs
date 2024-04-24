namespace Shared.Extensions
{
    using System;

    public static class CollectionsExtensions
    {
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