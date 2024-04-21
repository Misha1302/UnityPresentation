namespace Shared
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;

    public static class Thrower
    {
        [DoesNotReturn] public static void ArgumentOutOfRange(string msg = null) =>
            Throw<ArgumentOutOfRangeException>(msg);

        [DoesNotReturn] public static void Throw<T>() where T : Exception, new() =>
            Throw<T>(null);

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Throw<T>(string msg) where T : Exception, new() =>
            throw ((T)Activator.CreateInstance(typeof(T), msg));
    }
}