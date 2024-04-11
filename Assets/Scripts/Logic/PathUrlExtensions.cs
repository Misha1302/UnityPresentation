namespace Logic
{
    public static class PathUrlExtensions
    {
        public static string PathToUrl(this string path) => $"file:///{path}";
    }
}