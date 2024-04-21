namespace Extensions
{
    using JetBrains.Annotations;

    public static class JsonExtensions
    {
        [Pure]
        public static string ToJsonString(this string str) =>
            str.Replace(@"\", @"\\");
    }
}