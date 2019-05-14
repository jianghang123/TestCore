using Autofac;
using TestCore.Common.Configuration;

namespace TestCore.Common.Cache
{
    /// <summary>
    /// 缓存模块IOC
    /// </summary>
    public class TestCoreCacheModule : Autofac.Module
    {
        public TestCoreConfig config { get; }

        public TestCoreCacheModule(TestCoreConfig config)
        {
            this.config = config;

        }
        protected override void Load(ContainerBuilder builder)
        {
            //cache manager
            builder.RegisterType<PerRequestCacheManager>().As<ICacheManager>().InstancePerLifetimeScope();

            //static cache manager
            if (config.RedisCachingEnabled)
            {
                builder.RegisterType<RedisConnectionWrapper>()
                    .As<ILocker>()
                    .As<IRedisConnectionWrapper>()
                    .SingleInstance();
                builder.RegisterType<RedisCacheManager>().As<IStaticCacheManager>().InstancePerLifetimeScope();
            }
            else
            {
                builder.RegisterType<MemoryCacheManager>()
                    .As<ILocker>()
                    .As<IStaticCacheManager>()
                    .SingleInstance();
            }
        }
    }
}
