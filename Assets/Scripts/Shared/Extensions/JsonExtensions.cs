namespace Shared.Extensions
{
    using JetBrains.Annotations;

    public static class JsonExtensions
    {
        [Pure]
        public static string ToJsonString(this string str) =>
            str.Replace(@"\", @"\\");

        [Pure]
        public static string FromJsonString(this string str) =>
            str.Replace(@"\\", @"\");
    }
}