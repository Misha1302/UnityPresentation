namespace Shared.Extensions
{
    using JetBrains.Annotations;

    public static class PathUrlExtensions
    {
        [Pure]
        public static string PathToUrl(this string path) => $"file:///{path}";
    }
}