using System.Diagnostics;

namespace Api.Services.UserManagement.Helper
{
    public static class CacheHelper
    {
        public static string GetCacheName()
        {
            var stackTrace = new StackTrace();
            string key = string.Empty;
            for (int i = 0; ; i++)
            {
                if (stackTrace.GetFrame(i).GetILOffset() != StackFrame.OFFSET_UNKNOWN)
                    key = stackTrace.GetFrame(i).GetMethod().Name;
                else
                    break;
            }
            return key;
        }
    }
}
