
namespace TestCore.Common.ResponseResult
{
    public class Messages
    {  
        /// <summary>
        /// 是否成功，成功返回true,否则false
        /// </summary>
        public bool Success { get; set; } = false;
        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; } = "参数错误";
    }
}