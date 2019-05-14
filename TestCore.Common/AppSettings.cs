using System;
using System.Collections.Generic;
using System.Text;

namespace TestCore.Common
{
    public class AppSettings
    {
        public string LoginPwdSecretKey { get; set; }

        public string WalletSecretKey { get; set; }

        public bool SingleSignIn { get; set; }

        public int SiteId { set; get; }

        public string IsMaintain { set; get; }

        public string MaintainIp { set; get; }
        

        public PaySetting PaySetting { set; get; }



        /// <summary>
        /// 跨域Domain
        /// </summary>
        public string CorsOrigins { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示是否在生产环境中显示完整错误。它在开发环境中被忽略（总是启用）
        /// </summary>
        public bool DisplayFullErrorStack { get; set; }
    }

    public class PaySetting
    {
        public AliPay AliPay { set; get; }

        public WxPay WxPay { set; get; }
    }

    public class AliPay
    {
        /// <summary>
        /// 支付宝网关（固定）
        /// </summary>
        public string URL { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string APP_ID { set; get; }
        /// <summary>
        /// 开发者应用私钥
        /// </summary>
        public string APP_PRIVATE_KEY { set; get; }
        /// <summary>
        /// 参数返回格式，只支持json
        /// </summary>
        public string FORMAT { set; get; }
        /// <summary>
        /// 请求和签名使用的字符编码格式，支持GBK和UTF-8
        /// </summary>
        public string CHARSET { set; get; }
        /// <summary>
        /// 支付宝公钥
        /// </summary>
        public string ALIPAY_PUBLIC_KEY { set; get; }
        /// <summary>
        /// 商户生成签名字符串所使用的签名算法类型，目前支持RSA2和RSA，推荐使用RSA2
        /// </summary>
        public string SIGN_TYPE { set; get; }
        public string Version { set; get; }

        public string Notify_Url { set; get; }



    }

    public class WxPay
    {
        /// <summary>
        /// 微信网关（固定）
        /// </summary>
        public string URL { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string APP_ID { set; get; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string MCHID { set; get; }
        /// <summary>
        /// 交易类型
        /// </summary>
        public string NATIVE { set; get; }
        /// <summary>
        /// 异步通知url未设置，则使用配置文件中的url
        /// </summary>
        public string Notify_Url { set; get; }
        /// <summary>
        /// 请求和签名使用的字符编码格式，支持GBK和UTF-8
        /// </summary>
        public string CHARSET { set; get; }
        /// <summary>
        /// 密钥
        /// </summary>
        public string Key { set; get; }

    }
}
