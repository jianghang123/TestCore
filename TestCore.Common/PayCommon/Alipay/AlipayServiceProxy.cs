using TestCore.Common.Helper;
using TestCore.Common.Log;
using TestCore.Common.Security;
using log4net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using TestCore.Common.Ioc;

namespace TestCore.Common.PayCommon.Alipay
{
    public class AlipayServiceProxy : IPayService
    {
        public AliPay Setting
        {
            get {
                return _settings.Value.PaySetting.AliPay;
            }
        }
        private ILog log = LogUtils.GetLogger(typeof(AlipayServiceProxy));
        protected readonly IOptions<AppSettings> _settings;
        public AlipayServiceProxy()
        {
            _settings = IoCBootstrapper.ServiceProvider.GetService<IOptions<AppSettings>>();
        }

        /// <summary>
        /// 统一收单线下交易预创建（二维码）
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public string TradePrecreate(string orderId, decimal amount)
        {
        
            string result = string.Empty;
            var bizContent = "{" +
        "    \"out_trade_no\":\"" + orderId + "\"," +
        "    \"total_amount\":\"" + amount + "\"," +
        "    \"subject\":\"在线充值\"," +
        "    \"store_id\":\"NJ_001\"," +
        "    \"timeout_express\":\"15m\"}";
            try
            {
                var dicParams = InitRequest("alipay.trade.precreate", bizContent, Setting.Notify_Url);
                result = AliDoPost(Setting.URL, dicParams, Setting.CHARSET);
                if (!string.IsNullOrEmpty(result))
                {
                    var reps = JsonConvert.DeserializeObject<AlipayTradePrecreateResponseT>(result);
                    if (reps != null)
                    {
                        if (CheckSign(JsonConvert.SerializeObject(reps.alipay_trade_precreate_response), reps.sign))
                        {
                            return reps.alipay_trade_precreate_response.QrCode;
                        }
                        else
                        {
                            log.ErrorFormat("签名错误", orderId, amount, result.ToString());
                        }
                    }
                    else {
                        log.ErrorFormat("AlipayTradePrecreateResponseT序列化失败:{0}", JsonConvert.SerializeObject(result));
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                log.InfoFormat("接口返回错误:URl_{0} bizContent_{1} result_{2}", Setting.URL, bizContent, JsonConvert.SerializeObject(result));
                log.Error("TradePrecreate:", ex);
                return string.Empty;
            }
        }
        /// <summary>
        /// 统一收单交易撤销接口
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public bool TradeCancel(string orderId)
        {
            string result = string.Empty;
            string method = "alipay.trade.cancel";
            var bizContent = "{" +
           "    \"out_trade_no\":\"" + orderId + "\"," +
           "    \"trade_no\":\"\"}"; //设置业务参数

            try
            {
                var dicParams = InitRequest(method, bizContent);
                result = AliDoPost(Setting.URL, dicParams, Setting.CHARSET);
                if (!string.IsNullOrEmpty(result))
                {
                    var reps = JsonConvert.DeserializeObject<AlipayTradeCancelResponseT>(result);
                    if (reps != null)
                    {
                        var signCotent = JsonConvert.SerializeObject(reps.alipay_trade_cancel_response);
                        if (CheckSign(signCotent, reps.sign))
                        {
                            return string.IsNullOrEmpty(reps.alipay_trade_cancel_response.SubCode);
                        }
                        else
                        {
                            log.ErrorFormat("{0}签名错误:sign:{1} signCotent{2}", method, orderId, signCotent);
                            return false;
                        }
                    }
                    else
                    {
                        log.ErrorFormat("AlipayTradeCancelResponseT序列化失败:{0}", JsonConvert.SerializeObject(result));
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                log.InfoFormat("TradeCancel接口返回错误:URl_{0} bizContent_{1} result_{2}", Setting.URL, bizContent, JsonConvert.SerializeObject(result));
                log.Error("TradeCancel:", ex);
                return false;
            }
        }

        /// <summary>
        ///  统一收单线下交易查询
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="trade_no"></param>
        /// <returns></returns>
        public string TradeQuery(string orderId,string trade_no)
        {
            string method = "alipay.trade.query";
            string result = string.Empty;
            var bizContent = "{" +
           "    \"out_trade_no\":\"" + orderId + "\"," +
           "    \"trade_no\":\""+ trade_no + "\"}"; //设置业务参数
            try
            {
                var dicParams = InitRequest(method, bizContent);
                result = AliDoPost(Setting.URL, dicParams, Setting.CHARSET);
                if (!string.IsNullOrEmpty(result))
                {
                    var reps = JsonConvert.DeserializeObject<AlipayTradeQueryResponseT>(result);
                    if (reps != null)
                    {
                        var signCotent = JsonConvert.SerializeObject(reps.alipay_trade_query_response);
                        if (CheckSign(signCotent, reps.sign))
                        {
                            return reps.alipay_trade_query_response.TradeStatus;
                        }
                        else
                        {
                            log.ErrorFormat("{0}签名错误:sign:{1} signCotent{2}", method,orderId, signCotent);
                            return string.Empty;
                        }
                    }
                    else
                    {
                        log.ErrorFormat("{0} AlipayTradeQueryResponseT序列化失败:{1}", method,JsonConvert.SerializeObject(result));
                    }
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("TradeCancel接口返回错误:URl_{0} bizContent_{1} result_{2}", Setting.URL, bizContent, JsonConvert.SerializeObject(result));
                log.Error("TradeCancel:", ex);
            }
            return string.Empty;
        }


        public class AlipayTradePrecreateResponseT
        {
            public AlipayTradePrecreateResponse alipay_trade_precreate_response { set; get; }
            public string sign { set; get; }
        }

        public class AlipayTradeCancelResponseT
        {
            public AlipayTradeCancelResponse alipay_trade_cancel_response { set; get; }
            public string sign { set; get; }
        }

        public class AlipayTradeQueryResponseT
        {
            public AlipayTradeQueryResponse alipay_trade_query_response { set; get; }
            public string sign { set; get; }
        }

        public AopDictionary InitRequest(string method,string biz_content,string notify_url="")
        {
           
            AopDictionary txtParams = new AopDictionary();
            txtParams.Add("app_id", Setting.APP_ID);
            txtParams.Add("method", method);
            txtParams.Add("format", Setting.FORMAT);
            txtParams.Add("charset", Setting.CHARSET);
            txtParams.Add("sign_type", Setting.SIGN_TYPE);
            txtParams.Add("timestamp", "yyyy-MM-dd HH:mm:ss");
            txtParams.Add("version", Setting.Version);
            txtParams.Add("biz_content", biz_content);
            if (!string.IsNullOrEmpty(notify_url))
            {
                txtParams.Add("notify_url", notify_url);
            }
            txtParams.Add("sign", Signature.RSASign(txtParams, Setting.APP_PRIVATE_KEY, Setting.CHARSET,Setting.SIGN_TYPE));
            return txtParams;
        }

        public bool CheckSign(string orgStr,string sign)
        {
            return Signature.RSACheckContent(orgStr, sign, Setting.ALIPAY_PUBLIC_KEY, Setting.CHARSET, Setting.SIGN_TYPE, false);
        }

        public string AliDoPost(string url, IDictionary<string, string> parameters, string charset)
        {

            string result = string.Empty;
            try
            {
                var data = BuildQuery(parameters, charset);
                result = NetUtils.Post(url, data, charset, ContentType.application_form_urlencoded,20, true);
            }
            catch (Exception ex)
            {
                throw;
            }
            return result.ToString();
        }

        /// <summary>
        /// 组装普通文本请求参数。
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典</param>
        /// <returns>URL编码后的请求数据</returns>
        public  string BuildQuery(IDictionary<string, string> parameters, string charset)
        {
            StringBuilder postData = new StringBuilder();
            bool hasParam = false;

            IEnumerator<KeyValuePair<string, string>> dem = parameters.GetEnumerator();
            while (dem.MoveNext())
            {
                string name = dem.Current.Key;
                string value = dem.Current.Value;
                // 忽略参数名或参数值为空的参数
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                {
                    if (hasParam)
                    {
                        postData.Append("&");
                    }

                    postData.Append(name);
                    postData.Append("=");
                    string encodedValue = WebUtility.UrlEncode(value);

                    postData.Append(encodedValue);
                    hasParam = true;
                }
            }

            return postData.ToString();
        }

        public bool CheckNotifySign(IDictionary<string, string> parameters)
        {
            return Signature.RSACheckV2(parameters, Setting.ALIPAY_PUBLIC_KEY, Setting.CHARSET, Setting.SIGN_TYPE, false);
        }
    }
}
