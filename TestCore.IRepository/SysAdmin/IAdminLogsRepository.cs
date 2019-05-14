using TestCore.Common.Ioc;
using TestCore.Domain.SysEntity;

namespace TestCore.IRepository.SysAdmin
{
    public interface IAdminLogsRepository : IRepository<Adminlogs>, ISingletonDependency
    {
    }
}
