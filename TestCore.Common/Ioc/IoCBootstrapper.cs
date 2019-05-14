using Autofac;
using Autofac.Extensions.DependencyInjection;
using TestCore.Common.Ioc;
using TestCore.Common.PayCommon;
using TestCore.Common.PayCommon.Alipay;
using TestCore.Common.PayCommon.Wxpay;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Loader;

namespace TestCore.Common.Ioc
{
    public class IoCBootstrapper
    {
        /// <summary>
        /// IoC容器
        /// </summary>
        public static IContainer AutoContainer { get; private set; }
        public static AutofacServiceProvider ServiceProvider { get; private set; }
        /// <summary>
        /// Ioc容器默认扫描dll文件名开头
        /// </summary>
        private const string IOC_SCAN_ASSEBLIY_NAME_START = "TestCore";
        public static IServiceProvider Startup(IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            builder.Populate(services);
            var assemblies = GetAssemblies();
            builder.RegisterAssemblyTypes(assemblies)
                .Where(x => x.IsAssignableTo<ISingletonDependency>() && !x.GetTypeInfo().IsAbstract)
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterAssemblyTypes(assemblies)
               .Where(x => x.IsAssignableTo<IInstancePerDependency>() && !x.GetTypeInfo().IsAbstract)
               .AsImplementedInterfaces()
               .InstancePerDependency();

            builder.RegisterType<WxpayServiceProxy>().Named<IPayService>("wxpay").SingleInstance();
            builder.RegisterType<AlipayServiceProxy>().Named<IPayService>("alipay").SingleInstance();

            AutoContainer = builder.Build();
            ServiceProvider = new AutofacServiceProvider(AutoContainer);
            return ServiceProvider;
        }
        private static Assembly[] GetAssemblies()
        {
            var assemblies = new List<Assembly>();

            foreach (ProcessModule module in Process.GetCurrentProcess().Modules)
            {
                try
                {
                    if (module.FileName.EndsWith(".exe") || module.FileName.Substring(module.FileName.LastIndexOf("\\") + 1).ToLower().IndexOf(IOC_SCAN_ASSEBLIY_NAME_START.ToLower()) < 0)
                        continue;
                    var assemblyName = AssemblyLoadContext.GetAssemblyName(module.FileName);
                    var assembly = Assembly.Load(assemblyName);
                    assemblies.Add(assembly);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return assemblies.ToArray();
        }
    }
}
