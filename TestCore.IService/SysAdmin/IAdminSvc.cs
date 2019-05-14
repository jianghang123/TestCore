using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestCore.Domain.CommonEntity;
using TestCore.Domain.InputEntity;
using TestCore.Domain.SysEntity;

namespace TestCore.IService.SysAdmin
{
    public interface IAdminSvc : IBaseSvc<Admin>
    {
        Task<(bool Succeeded, string Msg, int UserId)> Login(string userName, string userPwd);
    }
}
