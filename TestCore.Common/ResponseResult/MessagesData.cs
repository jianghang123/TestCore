

namespace TestCore.Common.ResponseResult
{
    public class MessagesData<T> : Messages
    { 
        /// <summary>
        /// 数据对象（如LIST、Model）
        /// </summary>
        public T Data { get; set; } = default(T);
    }
}
