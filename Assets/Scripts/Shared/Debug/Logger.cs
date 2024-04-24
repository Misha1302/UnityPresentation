namespace Shared.Debug
{
    using Debug = UnityEngine.Debug;

    public static class DataLogger
    {
        public static void Log(params object[] obj)
        {
#if !DEBUG
            return;
#endif
            Debug.Log(string.Join(" ", obj));
        }
    }
}