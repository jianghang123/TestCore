using Autofac;
using Caiba.Core.Infrastructure;
using TestCore.Common.Configuration;

namespace TestCore.Common.Infrastructure.DependencyManagement
{
    /// <summary>
    /// 依赖注册接口
    /// </summary>
    public interface IDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="config">CaibaConfig</param>
        void Register(ContainerBuilder builder, ITypeFinder typeFinder, TestCoreConfig config);

        /// <summary>
        /// Gets order of this dependency registrar implementation
        /// </summary>
        int Order { get; }
    }
}
