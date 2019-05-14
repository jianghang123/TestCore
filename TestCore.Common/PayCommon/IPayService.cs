using System;
using System.Collections.Generic;
using System.Text;

namespace TestCore.Common.PayCommon
{
    public interface IPayService
    {
        /// <summary>
        /// 生成支付二维码
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        string TradePrecreate(string orderId, decimal amount);
        /// <summary>
        /// 取消交易
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        bool TradeCancel(string orderId);
        /// <summary>
        /// 订单查询
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="trade_no"></param>
        /// <returns></returns>
        string TradeQuery(string orderId, string trade_no);
        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        bool CheckNotifySign(IDictionary<string, string> parameters);
    }
}
