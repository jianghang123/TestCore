using System;
using System.Collections.Generic;
using System.Text;
using TestCore.Common.Ioc;
using TestCore.Domain.ApiEntity;
using TestCore.Domain.Entity;

namespace TestCore.IService.Common
{
    /// <summary>
    /// token服务
    /// </summary>
    public interface ITokenSvc : ISingletonDependency
    {
        AuthTokenDTO CreateToken(Users member);
    }
}
