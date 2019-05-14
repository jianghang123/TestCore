using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TestCore.Domain.Entity;
using TestCore.IRepository.User;
using TestCore.Repositories;
using TestCore.Data.Dapper;

namespace TestCore.Repository.User
{
    public class UserRepository: BaseRepository<Users>, IUserRepository
    {
        /// <summary>
        /// 更新用户分成
        /// </summary>
        /// <param name="list"></param>
        /// <param name="expelFields"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int InsertList(List<Userprice> list,string expelFields = null, IDbTransaction tran = null)
        {
            using (var conn = this.ConnectionFactory.OpenConnection())
            {
                tran = conn.BeginTransaction();
                try
                {
                    var res = conn.Delete<Userprice>(new { list[0].Userid }, tran);
                    res += conn.InsertList(list, null, tran);
                    tran.Commit();
                    return res;
                }
                catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
            }
        }
    }
}
