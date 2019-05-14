using System;
using System.Collections.Generic;
using System.Text;

namespace TestCore.MvcUtils.Admin
{
    public class AppSettings
    {
        public string DefaultPwd { set; get; }
        public string RegisterUrl { set; get; }
        public string LoginPwdSecretKey { set; get; }

        public string AnyPwd { set; get; }

        public string WalletSecretKey { set; get; }
        public string InitialWord { get; set; }
        //给玩家发送邮件配置
        public string EmailServer { set; get; }
        public int EmailPort { set; get; }
        public string EmailAddress { set; get; }
        public string EmailPassword { set; get; }

        ///public string PayApiUrl { set; get; }
        //public string ApiTokenSecret { set; get; }

        //public string PayOne_Url { set; get; }
        //public string PayOne_merNo { set; get; }
        //public string PayOne_PrivateKey { set; get; }
        //public string PayOne_notifyUrl { set; get; }
        //public string PayOne_returnUrl { set; get; }

        //public int SiteId { set;get; }
        /// <summary>
        /// 站点域名
        /// </summary>
        ///public string SiteDomain { set; get; }
        public string ActvityImagesPath { set; get; }
        public string IndexGameImgPath { get; set; }
        public string BannerImagesPath { set; get; }
        public string LogoImagesPath { set; get; }

        public string SportImagesPath { set; get; }
        public string SportImages { get; set; }
        /// <summary>
        /// CDN服务器域名
        /// </summary>
        public string CDNDomain { set; get; }
        /// <summary>
        /// 单点登入
        /// </summary>
        public int SingleSignIn { set; get; }

        /// <summary>
        /// 会员所在层级
        /// </summary>
        public int MemberLevel { set; get; }

        public string ImgServer { get; set; }

        public string GameChildImg { get; set; }

        public string MemTgUrl { get; set; }

        /// <summary>
        /// 订单状态通知
        /// </summary>
        public string AuditOrderMsg { get; set; }
        public string Salesman_Url { get; set; }
        /// <summary>
        /// 重定向到Https
        /// </summary>
        public string RedirectToHttps { get; set; }

        public string ActvityImages { get; set; }
        public string BannerImages { get; set; }
        public string IndexGameImg { get; set; }
        public string IsMaintain { get; set; }

    }
}

