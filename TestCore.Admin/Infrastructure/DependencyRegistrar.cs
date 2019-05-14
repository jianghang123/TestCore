using Autofac;
using System.Reflection;
using TestCore.Common.Cache;
using TestCore.Common.Captch;
using TestCore.Common.Configuration;
using TestCore.Common.Encryption;
using TestCore.Common.Helper;
using TestCore.Common.Infrastructure;
using TestCore.Common.Infrastructure.DependencyManagement;
using TestCore.Common.Ioc;
using TestCore.Common.RedisClient;
using TestCore.Common.ResponseResult;
using TestCore.Common.Routing;

namespace TestCore.Admin.Infrastructure
{
    /// <summary>
    /// Dependency registrar
    /// </summary>
    public class DependencyRegistrar : IDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="config">Config</param>
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, TestCoreConfig config)
        {
            //file provider
            builder.RegisterType<TestCoreFileProvider>().As<ITestCoreFileProvider>().InstancePerLifetimeScope();
            //web helper
            builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerLifetimeScope();
            //verify code
            builder.RegisterType<VerifyCode>().As<IVerifyCode>().InstancePerLifetimeScope();
            //workcontext
            builder.RegisterType<WorkContext>().As<IWorkContext>().InstancePerLifetimeScope();
            //Encrypt
            builder.RegisterType<AESEncrypt>().SingleInstance();
            builder.RegisterType<DESEncrypt>().SingleInstance();
            //operation messages 
            builder.RegisterType<Messages>().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(MessagesData<>)).InstancePerLifetimeScope();
            //caiba cache
            builder.RegisterModule(new TestCoreCacheModule(config));
            //redis client
            builder.RegisterModule(new RedisClientModule());

            //builder.RegisterType<MessageTypeCache>().SingleInstance();
            //builder.RegisterType<RewardTypeCache>().SingleInstance();

            //仓储和服务接口注册
            var IRepository = Assembly.Load("TestCore.IRepository");
            var Repository = Assembly.Load("TestCore.Repository");
            var IServices = Assembly.Load("TestCore.IService");
            var Services = Assembly.Load("TestCore.Services");

            //根据名称约定（数据访问层的接口和实现均以Repository结尾），实现数据访问接口和数据访问实现的依赖
            builder.RegisterAssemblyTypes(IRepository)
             .Where(t => t.Name.EndsWith("Repository"))
             .AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(Repository)
              .Where(t => t.Name.EndsWith("Repository"))
              .AsImplementedInterfaces().InstancePerLifetimeScope();

            //根据名称约定（服务层的接口和实现均以Service结尾），实现服务接口和服务实现的依赖
            builder.RegisterAssemblyTypes(IServices)
              .Where(t => t.Name.EndsWith("Svc"))
              .AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(Services)
              .Where(t => t.Name.EndsWith("Svc"))
              .AsImplementedInterfaces().InstancePerLifetimeScope();
            //route publisher
            builder.RegisterType<RoutePublisher>().As<IRoutePublisher>().SingleInstance();
            //上传功能策略注册
            //builder.RegisterType<UploadStrategy>().As<IUploadStrategy>().InstancePerLifetimeScope();

        }

        /// <summary>
        /// Gets order of this dependency registrar implementation
        /// </summary>
        public int Order => 0; 
    }
}
