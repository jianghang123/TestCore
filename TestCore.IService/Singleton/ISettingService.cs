using System.Collections.Generic;
using System.Data;
using TestCore.Domain.Singleton;

namespace TestCore.IService.Singleton
{
    public interface ISettingService
    {
        int Save(DataTable dt, int groupId);
        Dictionary<string, string> GetConfigByGroupId(int groupId);
        string GetValuesByKeyAndGroupId(string keyName, int groupId);
        bool SaveSite(SiteSettings model);
        bool SaveAttachment(AttachmentSettings model);
        bool SaveEmail(EmailSettings model);
        bool SaveSMS(SMSSettings model);
        void ClearSettingsSingleton();
    }
}
