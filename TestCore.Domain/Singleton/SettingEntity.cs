using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;
using System.Text;

namespace TestCore.Domain.Singleton
{
    /// <summary>
    /// 系统配置表实体
    /// </summary>
    [Table("C_SETTING")]
    public class SettingEntity : BaseEntity
    {
        #region Ctor

        public SettingEntity()
        { }

        public SettingEntity(string name, string value)
        {
            this.S_NAME = name;
            this.S_VALUE = value;
        }

        #endregion

        #region Fields

        public int S_GROUPID { get; set; }

        public string S_NAME { get; set; }

        public string S_VALUE { get; set; }

        #endregion

        #region Methods

        public override string ToString() => this?.S_NAME;

        #endregion
    }
}
