using System;
using System.Collections.Generic;
using System.Text;

namespace TestCore.Domain.SysEntity
{
    public partial class SysWebConfig
    {
        public int Id { get; set; }
        public string Domain { get; set; }
        public string SiteTitle { get; set; }
        public string SiteSkin { get; set; }
        public string Lang { get; set; }
        public int CurrencyId { get; set; }
        public int Status { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public string Modifier { get; set; }
        public DateTime UpdateTime { get; set; }
        public int SortIndex { get; set; }
        public string MemId { set; get; }
        public int OperId { set; get; }
        public int StatCycle { set; get; }
        public decimal SalePrice { set; get; }
        public string Ip { get; set; }

        public string QQ { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
        public string WX { get; set; }

        public string Skype { get; set; }
        public string LogoImg { get; set; }
        public string Keyword { get; set; }
        public string WXImage { get; set; }

        public string Prefix { get; set; }

        public string SMSSendUrl { get; set; }

        public int SMSExpireTime { get; set; }

        public bool OpenSMS { get; set; }

        /// <summary>
        /// 默认佣金占成设定
        /// </summary>
        public string DefaultShares { get; set; }

        /// <summary>
        /// 开启两步验证
        /// </summary>
        public bool OpenGoogleAuthenticator { get; set; }


        /// <summary>
        /// 谷歌验证发行者
        /// </summary>
        public string GoogleAuthIssuer { get; set; }

    }
}
