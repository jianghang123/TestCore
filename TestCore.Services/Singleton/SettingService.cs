using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using TestCore.Domain.Singleton;
using TestCore.IRepository.Singleton;
using TestCore.IService.Singleton;

namespace TestCore.Services.Singleton
{
    public partial class SettingService : ISettingService
    {
        #region Fields

        private readonly ISettingRepository settingRepository;

        #endregion

        #region Ctor

        public SettingService(ISettingRepository settingRepository)
        {
            this.settingRepository = settingRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 根据表添加数据
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="groupId">组编号</param>
        public int Save(DataTable dt, int groupId)
        {
            if (dt == null)
            {
                throw new ArgumentNullException(nameof(dt));
            }
            return settingRepository.Save(dt, groupId);
        }

        /// <summary>
        /// 根据组编号获取配置信息
        /// </summary>
        /// <param name="groupId">类型名称,若需要获取所有的，则groupId=0</param>
        /// <returns></returns>
        public Dictionary<string, string> GetConfigByGroupId(int groupId)
        {
            IEnumerable<SettingEntity> list = settingRepository.GetConfigByGroupId(groupId);
            if (list != null && list.Count() > 0)
            {
                Dictionary<string, string> settings = new Dictionary<string, string>();
                foreach (SettingEntity entity in list)
                {
                    settings.Add(entity.S_NAME, entity.S_VALUE);
                }
                return settings;
            }
            return null;
        }

        /// <summary>
        /// 根据键名和组名获取键值
        /// </summary>
        /// <param name="keyName">键名</param>
        /// <param name="groupId">组名编号</param>
        /// <returns></returns>
        public string GetValuesByKeyAndGroupId(string keyName, int groupId)
        {
            if (keyName == null)
            {
                throw new ArgumentNullException(nameof(keyName));
            }
            return settingRepository.GetValuesByKeyAndGroupId(keyName, groupId);
        }

        /// <summary>
        /// 添加站点配置数据
        /// </summary>
        /// <param name="model">站点配置Model</param> 
        public bool SaveSite(SiteSettings model)
        {
            int flag = (int)SettingEnum.Site;
            using (DataTable dt = new DataTable())
            {
                dt.Columns.Add("S_GROUPID", typeof(string));
                dt.Columns.Add("S_NAME", typeof(string));
                dt.Columns.Add("S_VALUE", typeof(string));
                PropertyInfo[] properties = model.GetType().GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = flag;
                    dr[1] = property.Name;
                    dr[2] = property.GetValue(model, null);
                    dt.Rows.Add(dr);
                }
                return this.Save(dt, flag) > 0;
            }
        }

        /// <summary>
        /// 添加附件配置数据
        /// </summary>
        /// <param name="model">附件配置Model</param> 
        public bool SaveAttachment(AttachmentSettings model)
        {
            int flag = (int)SettingEnum.Attachment;
            using (DataTable dt = new DataTable())
            {
                dt.Columns.Add("S_GROUPID", typeof(string));
                dt.Columns.Add("S_NAME", typeof(string));
                dt.Columns.Add("S_VALUE", typeof(string));
                PropertyInfo[] properties = model.GetType().GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = flag;
                    dr[1] = property.Name;
                    dr[2] = property.GetValue(model, null);
                    dt.Rows.Add(dr);
                }
                return this.Save(dt, flag) > 0;
            }
        }

        /// <summary>
        /// 添加邮箱配置数据
        /// </summary>
        /// <param name="model">邮箱配置Model</param> 
        public bool SaveEmail(EmailSettings model)
        {
            int flag = (int)SettingEnum.Email;
            using (DataTable dt = new DataTable())
            {
                dt.Columns.Add("S_GROUPID", typeof(string));
                dt.Columns.Add("S_NAME", typeof(string));
                dt.Columns.Add("S_VALUE", typeof(string));
                PropertyInfo[] properties = model.GetType().GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = flag;
                    dr[1] = property.Name;
                    dr[2] = property.GetValue(model, null);
                    dt.Rows.Add(dr);
                }
                return this.Save(dt, flag) > 0;
            }
        }

        /// <summary>
        /// 添加短信配置数据
        /// </summary>
        /// <param name="model">短信配置Model</param> 
        public bool SaveSMS(SMSSettings model)
        {
            int flag = (int)SettingEnum.SMS;
            using (DataTable dt = new DataTable())
            {
                dt.Columns.Add("S_GROUPID", typeof(string));
                dt.Columns.Add("S_NAME", typeof(string));
                dt.Columns.Add("S_VALUE", typeof(string));
                PropertyInfo[] properties = model.GetType().GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = flag;
                    dr[1] = property.Name;
                    dr[2] = property.GetValue(model, null);
                    dt.Rows.Add(dr);
                }
                return this.Save(dt, flag) > 0;
            }
        }

        /// <summary>
        /// 清理配置单例
        /// </summary>
        public void ClearSettingsSingleton()
        {
            SiteSettingsSingleton.Singleton = null;
            AttachmentSettingsSingleton.Singleton = null;
            EmailSettingsSingleton.Singleton = null;
            SMSSettingsSingleton.Singleton = null;
        }

        #endregion
    }
}
