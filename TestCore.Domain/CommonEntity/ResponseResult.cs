using System;
using System.Collections.Generic;
using System.Text;

namespace TestCore.Domain.CommonEntity
{
    /// <summary>
    /// 操作结果（响应客户端）
    /// </summary>
    public class ResponseResult
    {
        /// <summary>
        ///  操作结果 0 失败， 1 成功
        /// </summary>
        public int Result { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 附加数据
        /// </summary>
        public object Data { get; set; }

    }

    /// <summary>
    /// 操作结果（内部传递）
    /// </summary>
    public class OperationResult
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }
    }
}
