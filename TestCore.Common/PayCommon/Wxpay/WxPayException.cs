using System;

namespace TestCore.Common.PayCommon.Wxpay
{
    public class WxPayException : Exception
    {
        public WxPayException(string msg) : base(msg)
        {

        }
    }
}
