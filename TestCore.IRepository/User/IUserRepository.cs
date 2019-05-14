using System.Collections.Generic;
using System.Data;
using TestCore.Common.Ioc;
using TestCore.Domain.Entity;

namespace TestCore.IRepository.User
{
    public interface IUserRepository : IRepository<Users>, ISingletonDependency
    {
        /// <summary>
        /// 更新用户分成
        /// </summary>
        /// <param name="list"></param>
        /// <param name="expelFields"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        int InsertList(List<Userprice> list,string expelFields = null, IDbTransaction tran = null);
    }
}
