namespace Extensions
{
    using JetBrains.Annotations;

    public static class PathUrlExtensions
    {
        [Pure]
        public static string ToUrl(this string path) => $"file:///{path}";
    }
}