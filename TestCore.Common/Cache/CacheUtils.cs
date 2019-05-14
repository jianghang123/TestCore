using Autofac;
using TestCore.Common.Ioc;

namespace TestCore.Common.Cache
{
    public  class CacheUtils
    {
        private static ICacheService cacheService;

        public static ICacheService CacheService
        {
            get
            {
                if (cacheService == null)
                {
                    cacheService = IoCBootstrapper.AutoContainer.Resolve<ICacheService>();
                }
                return cacheService;
            }
        }
    }
}
