using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TestCore.Domain.Singleton;

namespace TestCore.IRepository.Singleton
{
    public partial interface ISettingRepository
    {
        int Save(DataTable dt, int groupId);
        IEnumerable<SettingEntity> GetConfigByGroupId(int groupId);
        string GetValuesByKeyAndGroupId(string keyName, int groupId);
    }
}
