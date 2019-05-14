using System;
using System.Collections.Generic;
using System.Text;
using TestCore.Domain.SysEntity;
using TestCore.IRepository.SysAdmin;
using TestCore.Repositories;

namespace TestCore.Repository.SysAdmin
{
    public class UserLogRepository : BaseRepository<SysUserLog>, IUserLogRepository
    {

    }
}
