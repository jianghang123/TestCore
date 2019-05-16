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
        public int InsertList(List<Userprice> list,int userId,string expelFields = null, IDbTransaction tran = null)
        {
            using (var conn = this.ConnectionFactory.OpenConnection())
            {
                tran = conn.BeginTransaction();
                try
                {
                    var res = conn.Delete<Userprice>(new { Userid = userId }, tran);
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


        /// <summary>
        /// 重置用户分成
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int ResetUserPrice(int userId)
        {
            using (var conn = this.ConnectionFactory.OpenConnection())
            {
               var tran = conn.BeginTransaction();
                try
                {
                    var res = conn.Delete<Userprice>(new { Userid = userId }, tran);
                    var userprice = conn.QueryList<Acc>(new { is_display = 0 }, null, tran);
                    List<Userprice> list = new List<Userprice>();
                    foreach (var item in userprice)
                    {
                        Userprice model = new Userprice
                        {
                            Userid = userId,
                            Channelid = item.Id,
                            Uprice = item.Uprice,
                            Gprice = item.Gprice,
                            Is_state = item.Is_state,
                            Addtime = DateTime.Now
                        };
                        list.Add(model);
                    }
                    res += conn.InsertList(list, null, tran);
                    tran.Commit();
                    return res;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw;
                }
            }
        }
    }
}
