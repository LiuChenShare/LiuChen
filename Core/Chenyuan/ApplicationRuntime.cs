using Chenyuan.VxIO;

namespace Chenyuan
{
    public static class ApplicationRuntime
    {
        public static IVxProviderInfo GetVirtualPathProvider(string key = null)
        {
            return null;
        }

        public static IVxProviderInfo DefaultVirtualPathProvider
        {
            get
            {
                return GetVirtualPathProvider();
            }
        }

        public static IVxPathProviderService VirtualPathProviderService
        {
            get
            {
                return null;
            }
        }
    }
}
