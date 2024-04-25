namespace Shared.Extensions
{
    using System.IO;
    using JetBrains.Annotations;
    using Shared.Debug;

    public static class PathsExtensions
    {
        private const string PathUrl = "file:///";

        [Pure]
        public static string PathToUrl(this string path) => $"{PathUrl}{path}";

        [Pure]
        public static string UrlToPath(this string path) => path.Replace(PathUrl, "");

        [Pure]
        public static bool IsValid(this string path)
        {
            path = path.UrlToPath();
            var exists = File.Exists(path);
            if (!exists) DataLogger.Log($"Path doesn't exists: {path}");

            return exists;
        }
    }
}