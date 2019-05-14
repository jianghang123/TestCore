using TestCore.Common.Ioc;
using TestCore.Domain.SysEntity;

namespace TestCore.IRepository.SysAdmin
{
    public interface IAdminRepository : IRepository<Admin>, ISingletonDependency
    {
    }
}
