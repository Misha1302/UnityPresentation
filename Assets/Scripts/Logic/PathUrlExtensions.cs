namespace Logic
{
    public static class PathUrlExtensions
    {
        public static string ToUrl(this string path) => $"file:///{path}";
    }
}