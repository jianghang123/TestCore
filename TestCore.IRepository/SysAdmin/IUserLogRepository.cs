using System;
using System.Collections.Generic;
using System.Text;
using TestCore.Common.Ioc;
using TestCore.Domain.SysEntity;

namespace TestCore.IRepository.SysAdmin
{
    public interface IUserLogRepository : IRepository<SysUserLog>, ISingletonDependency
    {

    }
}
