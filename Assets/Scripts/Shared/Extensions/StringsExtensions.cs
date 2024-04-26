namespace Shared.Extensions
{
    using System;
    using System.Linq;

    public static class StringsExtensions
    {
        private static readonly char[] _digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        public static int CompareStrings(this string a, string b)
        {
            if (a.IndexOfAny(_digits) >= 0 && b.IndexOfAny(_digits) >= 0)
                return a.CompareStringsByNumber(b);

            return String.Compare(a, b, StringComparison.Ordinal);
        }

        public static int CompareStringsByNumber(this string a, string b)
        {
            var aInt = a.GetIntFromString();
            var bInt = b.GetIntFromString();

            return aInt - bInt;
        }

        public static int GetIntFromString(this string a) =>
            int.Parse(string.Join("", a.Where(char.IsDigit)));
    }
}