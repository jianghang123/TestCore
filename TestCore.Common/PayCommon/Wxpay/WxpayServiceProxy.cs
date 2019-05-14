using TestCore.Common.Helper;
using TestCore.Common.Log;
using log4net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using TestCore.Common.Ioc;

namespace TestCore.Common.PayCommon.Wxpay
{
    public class WxpayServiceProxy : IPayService
    {

        public WxPay Setting
        {
            get
            {
                return _settings.Value.PaySetting.WxPay;
            }
        }
        private ILog _log = LogUtils.GetLogger(typeof(WxpayServiceProxy));

        protected readonly IOptions<AppSettings> _settings;
        public WxpayServiceProxy()
        {
            _settings = IoCBootstrapper.ServiceProvider.GetService<IOptions<AppSettings>>();
        }


        public bool TradeCancel(string orderId)
        {
            WxPayData data = new WxPayData();

            data.SetValue("out_trade_no", orderId);
            data.SetValue("appid", Setting.APP_ID);//公众账号ID
            data.SetValue("mch_id", Setting.MCHID);//商户号
            data.SetValue("nonce_str", Guid.NewGuid().ToString().Replace("-", ""));//随机字符串
            //签名
            data.SetValue("sign", data.MakeSign());
            string xml = data.ToXml();
            var start = DateTime.Now;
            _log.Debug("OrderQuery request : " + xml);
            string response = WxDoPost(Setting.URL + "/pay/orderquery", xml, Setting.CHARSET);
            _log.Debug("OrderQuery response : " + response);

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);//获得接口耗时

            //将xml格式的数据转化为对象以返回
            WxPayData result = new WxPayData();
            result.FromXml(response);
            if (result.GetValue("return_code").ToString() == "SUCCESS"
              && result.GetValue("result_code").ToString() == "SUCCESS")
            {
                return true;
            }
            return false;
        }

        public string TradePrecreate(string orderId, decimal amount)
        {
            WxPayData data = new WxPayData();
            data.SetValue("appid", Setting.APP_ID);//公众账号ID
            data.SetValue("mch_id", Setting.MCHID);//商户号
            data.SetValue("body", "test");//商品描述
            data.SetValue("attach", "test");//附加数据
            data.SetValue("out_trade_no", orderId);//随机字符串
            data.SetValue("total_fee", amount * 10);//总金额
            data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));//交易起始时间
            data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));//交易结束时间
            data.SetValue("trade_type", "NATIVE");//交易类型
            data.SetValue("product_id", orderId);//商品ID
            data.SetValue("spbill_create_ip", "");//终端ip
            data.SetValue("notify_url", Setting.Notify_Url);//异步通知url未设置，则使用配置文件中的url
            data.SetValue("nonce_str", Guid.NewGuid().ToString().Replace("-", ""));//随机字符串
            //签名
            data.SetValue("sign", data.MakeSign());
            string xml = data.ToXml();

            var start = DateTime.Now;

            _log.Debug("UnfiedOrder request : " + xml);

            string response = WxDoPost(Setting.URL + "/pay/unifiedorder", xml,  Setting.CHARSET);
            _log.Debug("UnfiedOrder response : " + response);

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);
            WxPayData result = new WxPayData();
            result.FromXml(response);
            return result.GetValue("code_url").ToString();//获得统一下单接口返回的二维码链接
        }

        public string TradeQuery(string orderId,string trade_no)
        {
            WxPayData data = new WxPayData();
            if (string.IsNullOrEmpty(trade_no))
            {
                data.SetValue("transaction_id", trade_no);
            }
            else
            {
                data.SetValue("out_trade_no", orderId);
            }
            data.SetValue("appid", Setting.APP_ID);//公众账号ID
            data.SetValue("mch_id", Setting.MCHID);//商户号
            data.SetValue("nonce_str", Guid.NewGuid().ToString().Replace("-", ""));//随机字符串
            //签名
            data.SetValue("sign", data.MakeSign());
            string xml = data.ToXml();
            var start = DateTime.Now;
            _log.Debug("OrderQuery request : " + xml);
            string response = WxDoPost(Setting.URL + "/pay/orderquery", xml, Setting.CHARSET);
            _log.Debug("OrderQuery response : " + response);

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);//获得接口耗时

            //将xml格式的数据转化为对象以返回
            WxPayData result = new WxPayData();
            result.FromXml(response);
            if (result.GetValue("return_code").ToString() == "SUCCESS"
              && result.GetValue("result_code").ToString() == "SUCCESS")
            {
                return result.GetValue("trade_state").ToString();
            }
            return string.Empty;
        }


        public static string WxDoPost(string url, string xml, string charset)
        {
            string result = string.Empty;
            try
            {
                result =NetUtils.Post(url, xml, charset, ContentType.text_xml, 6);
            }
            catch (Exception ex)
            {
                throw;
            }
            return result.ToString();
        }

        public bool CheckNotifySign(IDictionary<string, string> parameters)
        {
            var sortedDic = new SortedDictionary<string, object>();
            foreach (KeyValuePair<string, string> key in parameters)
            {
                sortedDic.Add(key.Key, key.Value);
            }
            WxPayData data = new WxPayData(sortedDic);
            return data.CheckSign();
        }
    }
}
